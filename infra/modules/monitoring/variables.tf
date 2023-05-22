variable "app_insights_name" {
  type        = string
  description = "The name of the Application Insights resource."
}

variable "location" {
  type        = string
  description = "The location to deploy Azure resources to."
}

variable "log_analytics_workspace_id" {
  type        = string
  description = "The resource id of the Log Analytics Workspace."
}

variable "managed_identity_principal_id" {
  type        = string
  description = "The principle id of the user-assigned managed identity permitted to publish monitoring data."
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group to deploy to."
}

variable "tags" {
  type        = map(string)
  description = "The default tags for Azure resources."
}
