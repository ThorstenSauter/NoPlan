﻿config {
    format = "compact"
    module = true
}

plugin "azurerm" {
    enabled = true
    version = "0.25.0"
    source  = "github.com/terraform-linters/tflint-ruleset-azurerm"
}
