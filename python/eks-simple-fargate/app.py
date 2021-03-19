#!/usr/bin/env python3

from aws_cdk import core as cdk

# For consistency with TypeScript code, `cdk` is the preferred import name for
# the CDK's core module.  The following line also imports it as `core` for use
# with examples from the CDK Developer's Guide, which are in the process of
# being updated to use `cdk`.  You may delete this import if you don't need it.
from aws_cdk import core

from eks_simple_fargate.eks_simple_fargate_stack import EksSimpleFargateStack

# Cluster name: If none, will autogenerate
cluster_name = None 
# Capacity details: Cluster size of small/med/large
capacity_details = "medium"
# Fargate enabled: Create a fargate profile on the cluster
fargate_enabled = True
# Bottlerocket ASG: Create a self managed node group of Bottlerocket nodes
bottlerocket_asg = False

app = core.App()
EksSimpleFargateStack(app, "EksSimpleFargateStack", fargate_enabled=fargate_enabled, capacity_details=capacity_details, bottlerocket_asg=bottlerocket_asg)

app.synth()
