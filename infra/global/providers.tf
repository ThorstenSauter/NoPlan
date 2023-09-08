terraform {
  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 2.41.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.72.0"
    }
    tfe = {
      source  = "hashicorp/tfe"
      version = "~> 0.48.0"
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

provider "azuread" {}

provider "azurerm" {
  storage_use_azuread = true
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}

provider "tfe" {}
