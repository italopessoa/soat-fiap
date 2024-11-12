variable "rds_cluster_identifier" {
  type    = string
  default = "techchallenge-mysql-default"
}

variable "profile" {
  description = "AWS profile name"
  type        = string
  default     = "default"
}

variable "region" {
  description = "AWS region"
  type        = string
  default     = "us-east-1"
}

variable "eks_cluster_name" {
  type    = string
  default = "quixada"
}

variable "apgw_name" {
  type    = string
  default = "authenticator"
}

variable "jwt_signing_key" {
  type      = string
  sensitive = true
}

variable "jwt_issuer" {
  type      = string
  sensitive = false
  default   = "https://localhost:7004"
}

variable "jwt_aud" {
  type      = string
  sensitive = false
  default   = "https://localhost:7004"
}

variable "api_docker_image" {
  type    = string
  default = "ghcr.io/soat-fiap/fiap.techchallenge.bytemeburger/api:sha-b83177c"
}

variable "internal_elb_name" {
  type    = string
  default = "api-internal"
}

variable "db_user" {
  type      = string
  sensitive = true
  default   = "db_user"
}

variable "db_pwd" {
  type      = string
  sensitive = true
  default   = "db_password"
}

variable "api_access_key_id" {
  type      = string
  nullable  = false
  sensitive = true
}

variable "api_secret_access_key" {
  type      = string
  nullable  = false
  sensitive = true
}

variable "user_pool_name" {
  type    = string
  default = "bmb-users-pool-local"
}
