#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from 'aws-cdk-lib';
import { ExerciseCircuitsBackendStack } from '../lib/exercise-circuits-backend-stack';
import { environmentConfigurations } from '../lib/environment-configurations';

const app = new cdk.App();

environmentConfigurations.forEach(envConfig => {
  const env = envConfig.awsEnv;
  new ExerciseCircuitsBackendStack(app, `${envConfig.projectName}-${env.region}-${envConfig.stage}`, {
    envConfig: envConfig,
    env: env
  });
});