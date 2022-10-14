variable "app_configuration_data_owner_role_principals" {
  type        = list(string)
  description = "The list of principals to assign the App Configuration Data Owner role to."
}

variable "app_configuration_data_reader_role_principals" {
  type        = list(string)
  description = "The list of principals to assign the App Configuration Data Reader role to."
}

variable "app_configuration_name" {
  type        = string
  description = "The name of the Azure App Configuration resource."
}

variable "local_auth_enabled" {
  type        = bool
  description = "Whether or not to enable local authentication using access keys."
  default     = false
}

variable "location" {
  type        = string
  description = "The location to deploy Azure resources to."
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group to deploy to."
}

variable "sku" {
  type        = string
  description = "The SKU of Azure App configuration to deploy."
  default     = "standard"
  validation {
    condition     = contains(["free", "standard"], var.sku)
    error_message = "The SKU must be either free or standard."
  }
}

variable "tags" {
  type        = map(string)
  description = "The default tags for Azure resources."
}
