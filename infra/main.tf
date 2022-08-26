terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.20.0"
    }
  }

  backend "azurerm" {
  }

  required_version = ">= 1.2.0"
}

data "azurerm_client_config" "current" {}
