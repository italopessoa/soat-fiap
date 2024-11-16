variable "rds_cluster_identifier" {
  type    = string
  default = "gh-techchallenge-mysql"
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
  default = "eks_dev_quixada"
}

variable "apgw_name" {
  type    = string
  default = "authenticator"
}

variable "jwt_signing_key" {
  type      = string
  sensitive = true
  default = ""
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
  default = "ghcr.io/soat-fiap/fiap.techchallenge.bytemeburger/api:latest"
}

variable "internal_elb_name" {
  type    = string
  default = "api-internal"
}

variable "db_user" {
  type      = string
  sensitive = true
  default   = "test"
}
variable "db_pwd" {
  type      = string
  sensitive = true
  default   = "test"
}

variable "api_access_key_id" {
  type      = string
  nullable  = false
  sensitive = true
  default = ""
}

variable "api_secret_access_key" {
  type      = string
  nullable  = false
  sensitive = true
  default = ""
}
variable "user_pool_name" {
  type    = string
  default = "bmb-users-pool-dev"
}
