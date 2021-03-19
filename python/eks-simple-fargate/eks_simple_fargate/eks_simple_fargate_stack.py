from aws_cdk import core, aws_eks
from .eks_base import EKSBase
from .alb_ingress import ALBIngressController


class EksSimpleFargateStack(core.Stack):

    def __init__(self, scope: core.Construct, construct_id: str, 
    eks_version=aws_eks.KubernetesVersion.V1_19, cluster_name=None,  
    capacity_details='small', fargate_enabled=False, bottlerocket_asg=False,**kwargs) -> None:
        super().__init__(scope, construct_id, **kwargs)
        self.eks_version = eks_version
        self.cluster_name = cluster_name
        self.capacity_details = capacity_details
        self.fargate_enabled = fargate_enabled
        self.bottlerocket_asg = bottlerocket_asg

        config_dict = {
            'eks_version': self.eks_version,
            'cluster_name': self.cluster_name,
            'capacity_details': self.capacity_details,
            'fargate_enabled': self.fargate_enabled,
            'bottlerocket_asg': self.bottlerocket_asg
        }
        
        base_cluster = EKSBase(self, "Base", cluster_configuration=config_dict)
        alb_ingress = ALBIngressController(self, "ALBIngress", cluster=base_cluster.cluster)

    # The code that defines your stack goes here
