namespace EksSimple
{
    public class Constants
    {
        public static string ADMIN_ROLE = "AdminRole";

        public static string STACK_PREFIX = "eks-cdk-";
        public static string CLUSTER_ID = "cdk-eks";
        public static string CLUSTER_NODE_GRP_ID = "nodegroup";
        public static string EKS_SECURITY_GRP = "eks-sg";
        public static string EC2_INSTANCE_TYPE = "t3.medium";
        public static string EC2_INGRESS_DESCRIPTION = "EKS cluster Ingress";

        public static string VPC_ID = "eks-vpc";

        public static string VPC_CIDR = "10.0.0.0/16";
    }
}