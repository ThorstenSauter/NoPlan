variable "container_app_environment_name" {
  type        = string
  description = "The name of the container app environment resource."
}

variable "location" {
  type        = string
  description = "The location to deploy Azure resources to."
}

variable "log_analytics_workspace_name" {
  type        = string
  description = "The name of the Log Analytics Workspace resource."
}

variable "log_analytics_workspace_retention_days" {
  type        = number
  description = "The number of days to keep data."
  default     = 30
}

variable "log_analytics_workspace_sku" {
  type        = string
  description = "The SKU of Log Analytics Workspace to use."
  default     = "PerGB2018"
  validation {
    condition = contains([
      "Free", "PerNode", "Premium", "Standard", "Standalone", "Unlimited", "CapacityReservation", "PerGB2018"
    ], var.log_analytics_workspace_sku)
    error_message = "SKU has to be one of: 'Free', 'PerNode', 'Premium', 'Standard', 'Standalone', 'Unlimited', 'CapacityReservation', 'PerGB2018'."
  }
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group to deploy to."
}

variable "tags" {
  type        = map(string)
  description = "The default tags for Azure resources."
}
