variable "env" {
  type        = string
  description = "The shorthand name of the environment the infrastructure is deployed to. E.g. 'dev', 'test', 'prod'"
  default     = "global"
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
    "environment" : "Global"
    "project" : "NoPlan"
    "managed-by" : "Terraform"
  }
  description = "The default tags for Azure resources."
}
