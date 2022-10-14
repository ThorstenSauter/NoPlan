terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.27.0"
    }
  }

  cloud {
    organization = "ThorstenSauter"
    workspaces {
      name = "NoPlan-staging"
    }
  }

  required_version = ">= 1.3.0"
}

data "azurerm_client_config" "current" {}

module "resource_group" {
  source   = "../modules/resource_group"
  name     = "rg-noplan-staging-${var.location}-${var.resource_suffix}"
  location = var.location
  tags     = var.tags
}
