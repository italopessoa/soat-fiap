variable "rds_cluster_identifier" {
  type    = string
  default = "techchallenge-mysql"
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

variable "mercadopago_webhook_secret" {
  type      = string
  sensitive = true
  default   = ""
}

variable "mercadopago_accesstoken" {
  type      = string
  sensitive = true
  default   = ""
}

variable "jwt_signing_key" {
  type      = string
  sensitive = true
}

variable "jwt_issuer" {
  type      = string
  sensitive = true
  default   = "https://localhost:7004"
}

variable "jwt_aud" {
  type      = string
  sensitive = true
  default   = "https://localhost:7004"
}

variable "api_docker_image" {
  type    = string
  default = "ghcr.io/soat-fiap/fiap.techchallenge.bytemeburger/api:sha-b83177c"
}


variable "internal_elb_name" {
  type    = string
  default = "api_internal_elb"
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