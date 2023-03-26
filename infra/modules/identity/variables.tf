variable "identity_name" {
  type        = string
  description = "The name of the user-assigned managed identity."
}

variable "location" {
  description = "The location to deploy Azure resources to."
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group to deploy to."
  type        = string
}

variable "tags" {
  type        = map(string)
  description = "The default tags for Azure resources."
}
