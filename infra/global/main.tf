terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.26.0"
    }
  }

  cloud {
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

module "monitoring" {
  source                       = "../modules/monitoring"
  app_insights_name            = "appi-noplan-${var.location}-${var.resource_suffix}"
  location                     = module.resource_group.location
  log_analytics_workspace_name = "law-noplan-${var.location}-${var.resource_suffix}"
  resource_group_name          = module.resource_group.name
  tags                         = var.tags
}
