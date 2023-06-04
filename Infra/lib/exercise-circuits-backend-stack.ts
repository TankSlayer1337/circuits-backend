import * as cdk from 'aws-cdk-lib';
import { AttributeType, BillingMode, Table } from 'aws-cdk-lib/aws-dynamodb';
import { Construct } from 'constructs';
import { EnvironmentConfiguration } from './environment-configurations';
import { OAuthScope, ResourceServerScope, UserPool, UserPoolClientIdentityProvider, UserPoolIdentityProviderGoogle } from 'aws-cdk-lib/aws-cognito';
import { Code, Function, Runtime } from 'aws-cdk-lib/aws-lambda';

interface ExerciseCircuitsBackendStackProps extends cdk.StackProps {
  envConfig: EnvironmentConfiguration
}

export class ExerciseCircuitsBackendStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props: ExerciseCircuitsBackendStackProps) {
    super(scope, id, props);

    const projectName = props.envConfig.projectName;
    const stage = props.envConfig.stage;

    var userPool = this.setupCognitoUserPool(projectName, stage);

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

  private setupCognitoUserPool(projectName: string, stage: string): UserPool {
    const userPool = new UserPool(this, `${projectName}-user-pool-${this.region}-${stage}`, {
      selfSignUpEnabled: true,
      userPoolName: `${projectName}-user-pool-${this.region}-${stage}`,
      signInAliases: { username: true, email: true }
    });

    const googleProvider = new UserPoolIdentityProviderGoogle(this, 'Google', {
      userPool: userPool,
      clientId: 'REPLACE-ME',
      clientSecret: 'REPLACE-ME'
    });

    const fullAccessScope = new ResourceServerScope({ scopeName: '*', scopeDescription: 'Full access' });
    const userDataServer = userPool.addResourceServer('ResourceServer', {
      identifier: 'user-data',
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
        callbackUrls: [
          'https://REPLACE-ME'
        ],
        scopes: [OAuthScope.resourceServer(userDataServer, fullAccessScope)]
      }
    });
    client.node.addDependency(googleProvider);

    return userPool;
  }
}
