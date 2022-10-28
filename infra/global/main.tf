terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.29.0"
    }
  }

  cloud {
    organization = "ThorstenSauter"
    workspaces {
      name = "NoPlan-global"
    }
  }

  required_version = ">= 1.3.0"
}

data "azurerm_client_config" "current" {}

module "resource_group" {
  source   = "../modules/resource_group"
  name     = "rg-noplan-global-${var.location}-${var.resource_suffix}"
  location = var.location
  tags     = var.tags
}

module "app_configuration" {
  source                                        = "../modules/app_configuration"
  app_configuration_name                        = "appcs-noplan-global-${var.location}-${var.resource_suffix}"
  resource_group_name                           = module.resource_group.name
  location                                      = module.resource_group.location
  app_configuration_data_owner_role_principals  = []
  app_configuration_data_reader_role_principals = []
  sku                                           = "free"
  tags                                          = var.tags
}

module "container_registry" {
  source                   = "../modules/container_registry"
  registry_name            = "acrnoplanglobal${var.location}${var.resource_suffix}"
  resource_group_name      = module.resource_group.name
  location                 = module.resource_group.location
  acr_pull_role_principals = []
  acr_push_role_principals = []
  tags                     = var.tags
}

module "monitoring" {
  source                       = "../modules/monitoring"
  app_insights_name            = "appi-noplan-${var.location}-${var.resource_suffix}"
  location                     = module.resource_group.location
  log_analytics_workspace_name = "law-noplan-${var.location}-${var.resource_suffix}"
  resource_group_name          = module.resource_group.name
  tags                         = var.tags
}
