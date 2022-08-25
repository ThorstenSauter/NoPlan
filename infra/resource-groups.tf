resource "azurerm_resource_group" "core" {
  name     = "rg-noplan-${var.env}-001"
  location = var.location
  tags     = var.tags
}
