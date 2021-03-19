using Amazon.CDK;
using Amazon.CDK.AWS.EKS;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using ec2 = Amazon.CDK.AWS.EC2;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EksSample
{
    public class EksSampleStack : Stack
    {
        internal EksSampleStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
              var clusterAdmin = new Role(this, Constants.ADMIN_ROLE, new RoleProps {
                AssumedBy = new AccountRootPrincipal()
            });

            IVpc vpc = new Vpc(this, Constants.VPC_ID, new VpcProps{
                Cidr = Constants.VPC_CIDR
            });            

            var cluster = new Cluster(this, Constants.CLUSTER_ID, new ClusterProps {
                MastersRole = clusterAdmin,
                Version = KubernetesVersion.V1_16,
                KubectlEnabled = true,
                DefaultCapacity = 0,
                Vpc = vpc                
            });
            
            var tags = new Dictionary<string, string>();
            tags.Add("name",Constants.CDK8s);

            var eksEC2sNodeGroup = cluster.AddNodegroup(Constants.CLUSTER_NODE_GRP_ID, new NodegroupOptions{
                InstanceType = new InstanceType(Constants.EC2_INSTANCE_TYPE),
                MinSize = 2,
                Subnets = new SubnetSelection{ Subnets = vpc.PrivateSubnets},
                Tags = tags
            });

            string[] ManagedPolicyArns = GetNodeRoleManagedPolicyARNs();
              foreach(string arn in ManagedPolicyArns){
                eksEC2sNodeGroup.Role.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName(arn));
            }

            var eksSecGrp = ec2.SecurityGroup.FromSecurityGroupId(this, Constants.EKS_SECURITY_GRP, cluster.ClusterSecurityGroupId);            
            secGrp.AddIngressRule(eksSecGrp, ec2.Port.Tcp(3306), description: Constants.EC2_INGRESS_DESCRIPTION);

            var privateSubnets = new List<string>();
            foreach(Subnet subnet in vpc.PrivateSubnets){
                privateSubnets.Add(subnet.SubnetId);
            }

            public static string[] GetNodeRoleManagedPolicyARNs(){ 
            string[] taskDefinitionManagedRoleActions = new string[]{
              "AmazonRDSFullAccess",
              "AmazonSSMFullAccess",
              "AmazonEC2ContainerServiceFullAccess",
              "service-role/AmazonEC2ContainerServiceforEC2Role"
          };
          return taskDefinitionManagedRoleActions;
        }
        }
    }
}
