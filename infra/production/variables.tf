variable "location" {
  type        = string
  description = "The location/region where the instances in this environment are created."
  default     = "westeurope"
}

variable "resource_suffix" {
  type        = string
  description = "The suffix to use for all resources in this environment."
  default     = "001"
}

variable "tags" {
  type = map(string)
  default = {
    "environment" : "Production"
    "project" : "NoPlan"
    "managed-by" : "Terraform"
  }
  description = "The default tags for Azure resources"
}
