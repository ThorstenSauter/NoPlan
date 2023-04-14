variable "azure_ad_instance" {
  type        = string
  description = "The Microsoft login instance."
  default     = "https://login.microsoftonline.com"
}

variable "container_image" {
  type        = string
  description = "The name of the container image to run."
}

variable "database_server_administrator_login" {
  type        = string
  description = "The login for the database server."
  default     = "noplanadmin"
}

variable "database_server_administrator_password" {
  type        = string
  description = "The password for the database server."
  sensitive   = true
}

variable "env" {
  type        = string
  description = "The shorthand name of the environment the infrastructure is deployed to. E.g. 'dev', 'test', 'prod'"
  default     = "stage"
}

variable "location" {
  type        = string
  description = "The location to deploy Azure resources to."
  default     = "westeurope"
}

variable "resource_id" {
  type        = string
  description = "The id appended to resource names in order to make them unique. E.g. '001', '002', '003'"
  default     = "001"
}

variable "tags" {
  type = map(string)
  default = {
    "environment" : "Staging"
    "project" : "NoPlan"
    "managed-by" : "Terraform"
  }
  description = "The default tags for Azure resources."
}
