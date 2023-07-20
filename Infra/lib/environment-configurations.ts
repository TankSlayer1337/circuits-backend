import { Environment } from "aws-cdk-lib"

export interface EnvironmentConfiguration {
  awsEnv: Environment,
  projectName: string,
  stage: string,
  stageSubDomain: string,
  origins: string[],
  cognitoHostedUiDomainPrefix: string
}

const stockholm: Environment = { region: 'eu-north-1', account: process.env.CDK_DEFAULT_ACCOUNT };

export const devConfiguration: EnvironmentConfiguration = {
  awsEnv: stockholm,
  projectName: 'exercise-circuits-backend',
  stage: 'dev',
  stageSubDomain: 'dev.',
  origins: [
    'http://localhost:5173',
    'https://dev.exercise-circuits.cloudchaotic.com'
  ],
  cognitoHostedUiDomainPrefix: 'exercise-circuits-dev'
}

export const stagingConfiguration: EnvironmentConfiguration = {
  awsEnv: stockholm,
  projectName: 'exercise-circuits-backend',
  stage: 'staging',
  stageSubDomain: 'staging.',
  origins: [ 'https://staging.exercise-circuits.cloudchaotic.com' ],
  cognitoHostedUiDomainPrefix: 'exercise-circuits-staging'
}

export const prodConfiguration: EnvironmentConfiguration = {
  awsEnv: stockholm,
  projectName: 'exercise-circuits-backend',
  stage: 'prod',
  stageSubDomain: '',
  origins: [ 'https://exercise-circuits.cloudchaotic.com' ],
  cognitoHostedUiDomainPrefix: 'exercise-circuits-prod'
}

export const environmentConfigurations: EnvironmentConfiguration[] = [ devConfiguration, stagingConfiguration, prodConfiguration ]