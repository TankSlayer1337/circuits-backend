import * as cdk from 'aws-cdk-lib';
import { AttributeType, BillingMode, Table } from 'aws-cdk-lib/aws-dynamodb';
import { Construct } from 'constructs';
import { EnvironmentConfiguration } from './environment-configurations';
import { AccountRecovery, OAuthScope, ProviderAttribute, ResourceServerScope, UserPool, UserPoolClientIdentityProvider, UserPoolIdentityProviderGoogle } from 'aws-cdk-lib/aws-cognito';
import { Code, Function, Runtime } from 'aws-cdk-lib/aws-lambda';
import { CognitoUserPoolsAuthorizer, LambdaIntegration, RestApi } from 'aws-cdk-lib/aws-apigateway';
import { DnsValidatedCertificate } from 'aws-cdk-lib/aws-certificatemanager';
import { ARecord, HostedZone, RecordTarget } from 'aws-cdk-lib/aws-route53';
import { ApiGateway } from 'aws-cdk-lib/aws-route53-targets';

interface ExerciseCircuitsBackendStackProps extends cdk.StackProps {
  envConfig: EnvironmentConfiguration
}

export class ExerciseCircuitsBackendStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props: ExerciseCircuitsBackendStackProps) {
    super(scope, id, props);

    const envConfig = props.envConfig;
    const projectName = props.envConfig.projectName;
    const stage = props.envConfig.stage;

    const userPool = this.setupCognitoUserPool(projectName, props.envConfig);

    const table = new Table(this, `DynamoDBTable`, {
      tableName: `${projectName}-table-${this.region}-${stage}`,
      partitionKey: { name: 'PK', type: AttributeType.STRING },
      sortKey: { name: 'SK', type: AttributeType.STRING },
      billingMode: BillingMode.PAY_PER_REQUEST,
      removalPolicy: cdk.RemovalPolicy.DESTROY
    });

    const lambdaFunction = new Function(this, 'UserDataLambda', {
      functionName: `${projectName}-user-data-${this.region}-${stage}`,
      runtime: Runtime.DOTNET_6,
      code: Code.fromAsset(`../Circuits/Circuits.Public/src/Circuits.Public/bin/build-package.zip`),
      handler: 'Circuits.Public',
      timeout: cdk.Duration.seconds(10),
      memorySize: 256,
      reservedConcurrentExecutions: 2,
      environment: {
        'TABLE_NAME': table.tableName 
      }
    });
    table.grantReadWriteData(lambdaFunction);
    const lambdaIntegration = new LambdaIntegration(lambdaFunction);

    const hostedZone = HostedZone.fromLookup(this, 'Z05241663C6V8VT2JGS2K', {
      domainName: 'cloudchaotic.com'
    });
    const apiDomainName = `${envConfig.stageSubDomain}exercise-circuits.api.cloudchaotic.com`
    const certificate = new DnsValidatedCertificate(this, 'Certificate', {
      domainName: apiDomainName,
      hostedZone: hostedZone,
      cleanupRoute53Records: true // not recommended for production use
    });
    const api = new RestApi(this, 'Exercise Circuits API', {
      restApiName: `${projectName}-api-${this.region}-${stage}`,
      domainName: {
        domainName: apiDomainName,
        certificate: certificate
      }
    });
    const publicProxyResource = api.root.addResource('{proxy+}');
    publicProxyResource.addMethod('ANY', lambdaIntegration, {
      authorizer: new CognitoUserPoolsAuthorizer(this, 'CognitoAuthorizer', {
        cognitoUserPools: [userPool]
      }),
      authorizationScopes: [ `https://${apiDomainName}/*` ]
    });
    /*
    The admin proxy resource and cors preflight are set explicitly to avoid the cors OPTIONS method 
    using the Cognito Authorizer, which would cause the cors preflight check to fail. Curiously enough,
    cors also has to be set in the .NET application.
    */
    publicProxyResource.addCorsPreflight({
      allowOrigins: envConfig.origins
    });

    new ARecord(this, 'Exercise Circuits API ARecord', {
      zone: hostedZone,
      recordName: `${envConfig.stageSubDomain}exercise-circuits.api`,
      target: RecordTarget.fromAlias(new ApiGateway(api)),
      ttl: cdk.Duration.seconds(0)
    });
  }

  private setupCognitoUserPool(projectName: string, envConfig: EnvironmentConfiguration): UserPool {
    const stage = envConfig.stage;
    const userPool = new UserPool(this, `${projectName}-user-pool-${this.region}-${stage}`, {
      selfSignUpEnabled: true,
      userPoolName: `${projectName}-user-pool-${this.region}-${stage}`,
      signInAliases: { username: true, email: true },
      accountRecovery: AccountRecovery.EMAIL_ONLY,
      standardAttributes: {
        email: { required: true }
      },
      removalPolicy: cdk.RemovalPolicy.DESTROY
    });
    userPool.addDomain('UserPoolDomain', {
      cognitoDomain: {
        domainPrefix: envConfig.cognitoHostedUiDomainPrefix
      }
    });

    const fullAccessScope = new ResourceServerScope({ scopeName: '*', scopeDescription: 'Full access' });
    const userDataServer = userPool.addResourceServer('ResourceServer', {
      userPoolResourceServerName: 'Exercise Circuits API',
      identifier: `https://${stage}.exercise-circuits.api.cloudchaotic.com`,
      scopes: [fullAccessScope]
    });

    const client = userPool.addClient('exercise-circuits', {
      supportedIdentityProviders: [
        UserPoolClientIdentityProvider.GOOGLE
      ],
      oAuth: {
        flows: {
          authorizationCodeGrant: true
        },
        callbackUrls: envConfig.origins,
        logoutUrls: envConfig.origins,
        scopes: [
          OAuthScope.resourceServer(userDataServer, fullAccessScope),
          OAuthScope.PROFILE,
          OAuthScope.EMAIL
        ]
      }
    });

    const googleProvider = new UserPoolIdentityProviderGoogle(this, 'Google', {
      userPool: userPool,
      // client ID and secret are replaced in the console.
      clientId: 'REPLACE-ME',
      clientSecret: 'REPLACE-ME',
      scopes: ['profile', 'email'],
      attributeMapping: {
        email: ProviderAttribute.GOOGLE_EMAIL
      }
    });
    client.node.addDependency(googleProvider);

    return userPool;
  }
}
