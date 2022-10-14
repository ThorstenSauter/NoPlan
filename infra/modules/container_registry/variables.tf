variable "acr_pull_role_principals" {
  type        = list(string)
  description = "The list of principal ids that should be assigned the AcrPull role on the Azure Container Registry."
}

variable "acr_push_role_principals" {
  type        = list(string)
  description = "The list of principal ids that should be assigned the AcrPush role on the Azure Container Registry."
}

variable "admin_enabled" {
  type        = bool
  description = "Whether or not the admin user is enabled for the Azure Container Registry."
  default     = false
}

variable "location" {
  type        = string
  description = "The location to deploy Azure resources to."
}

variable "registry_name" {
  type        = string
  description = "The name of the Azure Container Registry."
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group to deploy to."
}

variable "tags" {
  type        = map(string)
  description = "The default tags for Azure resources."
}

variable "sku" {
  type        = string
  description = "The SKU of the Azure Container Registry."
  default     = "Basic"
  validation {
    condition     = contains(["Basic", "Standard", "Premium"], var.sku)
    error_message = "The SKU must be one of Basic, Standard, or Premium."
  }
}
