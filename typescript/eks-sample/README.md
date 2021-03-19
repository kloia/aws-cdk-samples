 # CDK Example of EKS

First, this is an *example*. This is not production ready. The point of this
repository is demonstrate some of the features available with CDK.

**If you choose to run this stack you are responsible for any AWS costs that
are incurred. The default values are designed to be cost conscious.**

The `cdk.json` file tells the CDK Toolkit how to execute your app.

## Useful commands

 * `npm run build`   compile typescript to js
 * `npm run watch`   watch for changes and compile
 * `npm run test`    perform the jest unit tests
 * `cdk deploy`      deploy this stack to your default AWS account/region
 * `cdk diff`        compare deployed stack with current state
 * `cdk synth`       emits the synthesized CloudFormation template


#### CDK Setup

If you don't already have the CDK installed please follow the
[guide](https://awslabs.github.io/aws-cdk/getting-started.html).

We will be using Typescript for these examples.

Before going any further clone this repository and run the following commands:

```
# from root of this repo
npm install
npm run build
```

#### EKS Cluster Control Plane - Stack 1

The first stack we will be creating is the EKS Cluster and Control Plane.

The context allows you to set your desired EKS Cluster Name, but if you do not
alter `cdk.json` or pass in a command line argument the default will be used.
The stack will also create a VPC and NAT Gateway.

Using the defaults the command would be: 

```
# from root of this repo
cdk deploy EksCluster


The creation of the EKS cluster can take up to 15 minutes. After the stack
completes we can verify we have the credentials necessary to use `kubectl`

```
#### EKS Worker Nodes - Stack 2

Now that AWS is running our Kubernetes API Server and required components for
us, we need to create worker nodes. There are configuration options for the
workers, but for now if you just want to deploy some nodes the defaults will
work.

```
# from root of this repo
cdk deploy EksWorkers
# this output a similar success message at the end
```

The defaults for the workers can be found in the [cdk.json](cdk.json). The only
aspect that might be confusing is the optional [bastion](https://en.wikipedia.org/wiki/Bastion_host) configuration. 
If you want a bastion host the best option is to edit the [cdk.json](cdk.json)
file and the values for your configuration. The edits will be made to the
`bastion`, `key-name`, and `ssh-allowed-cidr` json keys. 

`key-name` is an [AWS EC2 Keypair](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/ec2-key-pairs.html).

`ssh-allowed-cidr` is a list of IP addresses that will be allowed to SSH to the
bastion host. You can lookup your external IP via [AWS](http://checkip.amazonaws.com/). At a minimum you will want to add that IP as a `/32` below.

Your file might look similar to this: 

```json
{
  "app": "node bin/eks-example.js",
  "context": {
    "cluster-name": "EksExample",
    "key-name": "MyKeyPair",
    "node-group-max-size": 5,
    "node-group-min-size": 1,
    "node-group-desired-size": 3,
    "node-group-instance-type": "t3.medium",
    "bastion": true,
    "ssh-allowed-cidr": ["1.2.3.4/32"]
}
```

If you change these values after deploying you will need to re-deploy the stack
in order to apply the updates. That can be done:

```
npm run build
cdk diff EksWorkers
# make sure the diff matches what you think is happening
cdk deploy EksWorkers
# example success 
Outputs:
EksWorkers.WorkerRoleArn = arn:aws:iam::667237269012:role/EksWorkers-WorkersInstanceRole510CB30C-QFC0D1PV61B
# note this ARN for the next step
```

Once you have your workers deployed we need to join them to the cluster. This
currently must be done using `kubectl`. In order to do this update the file in
this repo called [aws-auth-cm.yaml](aws-auth-cm.yaml) with the Role ARN from the
EksWorkers state output. Specifically replace this line with your value.

```
    - rolearn: '<your role arn here>'
```

This file gives the Kubernetes permission to join the cluster specifically to
the role attached to these nodes.

```
kubectl apply -f aws-auth-cm.yaml
kubectl get nodes --watch # this will follow the k8s events CTRL-C to break

```
#### Cleaning Up the Example

The CDK comes equipped with destroy commands:
```
cdk destroy EksCluster
# follow the prompts
cdk destroy EksWorkers
```
That should delete all the resources we created in this example



