﻿config {
    format = "compact"
    module = true
}

plugin "azurerm" {
    enabled = true
    version = "0.17.1"
    source  = "github.com/terraform-linters/tflint-ruleset-azurerm"
}