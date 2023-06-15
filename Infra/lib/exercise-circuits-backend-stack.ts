import * as cdk from 'aws-cdk-lib';
import { AttributeType, BillingMode, Table } from 'aws-cdk-lib/aws-dynamodb';
import { Construct } from 'constructs';
import { EnvironmentConfiguration } from './environment-configurations';
import { AccountRecovery, OAuthScope, ProviderAttribute, ResourceServerScope, UserPool, UserPoolClientIdentityProvider, UserPoolIdentityProviderGoogle } from 'aws-cdk-lib/aws-cognito';
import { Code, Function, Runtime } from 'aws-cdk-lib/aws-lambda';

interface ExerciseCircuitsBackendStackProps extends cdk.StackProps {
  envConfig: EnvironmentConfiguration
}

export class ExerciseCircuitsBackendStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props: ExerciseCircuitsBackendStackProps) {
    super(scope, id, props);

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
      handler: projectName,
      timeout: cdk.Duration.seconds(10),
      memorySize: 256,
      reservedConcurrentExecutions: 2,
      environment: {
        'TABLE_NAME': table.tableName 
      }
    });

    table.grantReadWriteData(lambdaFunction);
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
        callbackUrls: envConfig.cognitoUrls,
        logoutUrls: envConfig.cognitoUrls,
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
