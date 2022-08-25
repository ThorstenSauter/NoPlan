resource "azurerm_user_assigned_identity" "noplan" {
  name                = "id-noplan-${var.env}-westeurope-001"
  resource_group_name = azurerm_resource_group.core.name
  location            = azurerm_resource_group.core.location
  tags                = var.tags
}
