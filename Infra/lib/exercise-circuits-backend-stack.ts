import * as cdk from 'aws-cdk-lib';
import { AttributeType, BillingMode, Table } from 'aws-cdk-lib/aws-dynamodb';
import { Construct } from 'constructs';
import { EnvironmentConfiguration } from './environment-configurations';

interface ExerciseCircuitsBackendStackProps extends cdk.StackProps {
  envConfig: EnvironmentConfiguration
}

export class ExerciseCircuitsBackendStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props: ExerciseCircuitsBackendStackProps) {
    super(scope, id, props);

    const projectName = props.envConfig.projectName;
    const stage = props.envConfig.stage;
    const table = new Table(this, `${projectName}-table-${this.region}-${stage}`, {
      partitionKey: { name: 'PK', type: AttributeType.STRING },
      sortKey: { name: 'SK', type: AttributeType.STRING },
      billingMode: BillingMode.PAY_PER_REQUEST
    });
  }
}
