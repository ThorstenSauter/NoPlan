data "azurerm_client_config" "current" {}

data "tfe_outputs" "global" {
  organization = "ThorstenSauter"
  workspace    = "NoPlan-global"
}

locals {
  api_application_id      = nonsensitive(data.tfe_outputs.global.values.api_application_id)
  api_audience            = nonsensitive(data.tfe_outputs.global.values.api_audience)
  default_domain          = nonsensitive(data.tfe_outputs.global.values.default_domain)
  api_tenant_id           = nonsensitive(data.tfe_outputs.global.values.api_tenant_id)
  container_registry_name = nonsensitive(data.tfe_outputs.global.values.container_registry_name)
}

module "container_app" {
  source                                = "../modules/containerapp"
  application_insights_connectionstring = module.monitoring.app_insights_connection_string
  azure_ad_audience                     = local.api_audience
  azure_ad_client_id                    = local.api_application_id
  azure_ad_domain                       = local.default_domain
  azure_ad_instance                     = var.azure_ad_instance
  azure_ad_tenant_id                    = local.api_tenant_id
  default_connectionstring              = module.database.connectionstring
  container_app_environment_id          = module.containerapp_environment.container_app_environment_id
  container_app_name                    = "aca-noplan-api-${var.env}-${var.resource_id}"
  container_image                       = var.container_image
  container_name                        = "noplan-api"
  container_registry                    = "${local.container_registry_name}.azurecr.io"
  managed_identity_id                   = module.identity.identity_api_id
  resource_group_name                   = module.resource_group.name
  tags                                  = var.tags
}

module "containerapp_environment" {
  source                         = "../modules/containerapp_environment"
  container_app_environment_name = "acae-noplan-${var.env}-${var.resource_id}"
  location                       = module.resource_group.location
  log_analytics_workspace_name   = "log-noplan-${var.env}-${var.resource_id}"
  resource_group_name            = module.resource_group.name
  tags                           = var.tags
}

module "database" {
  source                                 = "../modules/database"
  database_server_name                   = "sqlsrv-noplan-${var.env}-${var.resource_id}"
  database_server_administrator_login    = var.database_server_administrator_login
  database_server_administrator_password = var.database_server_administrator_password
  location                               = module.resource_group.location
  resource_group_name                    = module.resource_group.name
  tags                                   = var.tags
}

module "identity" {
  source              = "../modules/identity"
  identity_name       = "id-noplan-${var.env}-${var.resource_id}"
  location            = module.resource_group.location
  resource_group_name = module.resource_group.name
  tags                = var.tags
}

module "monitoring" {
  source                     = "../modules/monitoring"
  app_insights_name          = "appi-noplan-${var.env}-${var.resource_id}"
  location                   = module.resource_group.location
  resource_group_name        = module.resource_group.name
  tags                       = var.tags
  log_analytics_workspace_id = module.containerapp_environment.log_analytics_workspace_id
}

module "resource_group" {
  source   = "../modules/resource_group"
  name     = "rg-noplan-${var.env}-${var.location}-${var.resource_id}"
  location = var.location
  tags     = var.tags
}
