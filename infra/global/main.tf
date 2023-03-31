terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.50.0"
    }
    tfe = {
      source  = "hashicorp/tfe"
      version = "~> 0.43.0"
    }
  }

  cloud {
    organization = "ThorstenSauter"
    workspaces {
      name = "NoPlan-global"
    }
  }

  required_version = ">= 1.4.0"
}

data "azurerm_client_config" "current" {}

data "tfe_outputs" "production" {
  organization = "ThorstenSauter"
  workspace    = "NoPlan-production"
}

data "tfe_outputs" "staging" {
  organization = "ThorstenSauter"
  workspace    = "NoPlan-staging"
}

locals {
  acr_pull_principals = compact([
    try(nonsensitive(data.tfe_outputs.production.values.identity_api_principal_id), ""),
    try(nonsensitive(data.tfe_outputs.staging.values.identity_api_principal_id), "")
  ])
  acr_push_principals = [
    data.azurerm_client_config.current.object_id
  ]
}

module "container_registry" {
  source                   = "../modules/container_registry"
  registry_name            = "acrnoplan${var.env}${var.location}${var.resource_id}"
  resource_group_name      = module.resource_group.name
  location                 = module.resource_group.location
  acr_pull_role_principals = local.acr_pull_principals
  acr_push_role_principals = local.acr_push_principals
  tags                     = var.tags
}

module "resource_group" {
  source   = "../modules/resource_group"
  name     = "rg-noplan-${var.env}-${var.location}-${var.resource_id}"
  location = var.location
  tags     = var.tags
}
