variable "app_insights_name" {
  type        = string
  description = "The name of the Application Insights instance to create."
}

variable "location" {
  type        = string
  description = "The location/region where the Application Insights and Log Analytics workspace instances are created."
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group in which to create the Application Insights and Log Analytics workspace instances."
}

variable "log_analytics_workspace_name" {
  type        = string
  description = "The name of the Log Analytics workspace to create."
}

variable "log_analytics_workspace_sku" {
  type        = string
  description = "The SKU of the Log Analytics workspace to create."
  default     = "PerGB2018"
}

variable "retention_in_days" {
  type        = number
  description = "The number of days to retain data in the Log Analytics workspace."
  default     = 30
  validation {
    condition     = (var.retention_in_days >= 30 && var.retention_in_days <= 730)
    error_message = "The retention_in_days value must be between 30 and 730."
  }
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resources."
}
