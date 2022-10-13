terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.26.0"
    }
  }

  backend "azurerm" {
  }

  required_version = ">= 1.3.0"
}

data "azurerm_client_config" "current" {}
