output "eks_cluster" {
  value     = data.aws_eks_cluster.techchallenge_cluster
  sensitive = true
}

output "userpool_id" {
  value = local.cognito_user_pool_id
}