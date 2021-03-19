#!/usr/bin/env python3

from aws_cdk import core as cdk

# For consistency with TypeScript code, `cdk` is the preferred import name for
# the CDK's core module.  The following line also imports it as `core` for use
# with examples from the CDK Developer's Guide, which are in the process of
# being updated to use `cdk`.  You may delete this import if you don't need it.
from aws_cdk import core

from eks_simple_ec2.eks_simple_ec2_stack import EksSimpleEc2Stack


app = core.App()
EksSimpleEc2Stack(app, "EksSimpleEc2Stack")

app.synth()
