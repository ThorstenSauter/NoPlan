variable "database_name" {
  type        = string
  description = "The name of the database to create."
  default     = "NoPlan"
}

variable "database_server_name" {
  type        = string
  description = "The name of the database server to create."
}

variable "database_server_administrator_login" {
  type        = string
  description = "The login for the database server."
}

variable "database_server_administrator_password" {
  type        = string
  description = "The password for the database server."
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
