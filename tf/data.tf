##############################
# EKS CLUSTER
##############################

data "aws_eks_cluster" "techchallenge_cluster" {
  name = var.eks_cluster_name
}

##############################
# API GATEWAY
##############################

# data "aws_apigatewayv2_apis" "api_id" {
#   name = var.apgw_name
# }

# data "aws_apigatewayv2_api" "example" {
#   api_id = tolist(data.aws_apigatewayv2_apis.api_id.ids)[0]
# }

##############################
# COGNITO USER POOL
##############################

data "aws_cognito_user_pools" "user_pool" {
  name = var.user_pool_name
}

data "aws_cognito_user_pool_clients" "api_client" {
  user_pool_id = data.aws_cognito_user_pools.user_pool.ids[0]
}

##############################
# DATABASE
##############################

data "aws_rds_cluster" "example" {
  cluster_identifier = var.rds_cluster_identifier
}