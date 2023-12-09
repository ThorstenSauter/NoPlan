terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.84.0"
    }
    tfe = {
      source  = "hashicorp/tfe"
      version = "~> 0.50.0"
    }
  }

  cloud {
  }

  required_version = ">= 1.6.0"
}

provider "azurerm" {
  storage_use_azuread = true
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}

provider "tfe" {}
