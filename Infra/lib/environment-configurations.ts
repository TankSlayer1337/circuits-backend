import { Environment } from "aws-cdk-lib"

export interface EnvironmentConfiguration {
    awsEnv: Environment,
    projectName: string,
    stage: string
}

const stockholm: Environment = { region: 'eu-north-1' };

export const devConfiguration: EnvironmentConfiguration = {
    awsEnv: stockholm,
    projectName: 'exercise-circuits-backend',
    stage: 'dev'
}

export const environmentConfigurations: EnvironmentConfiguration[] = [ devConfiguration ]