resource "azurerm_container_registry" "acr" {
  name                = var.registry_name
  resource_group_name = var.resource_group_name
  location            = var.location
  admin_enabled       = var.admin_enabled
  sku                 = var.sku
  tags                = var.tags
}

resource "azurerm_role_assignment" "acr_pull_role" {
  for_each             = toset(var.acr_pull_role_principals)
  scope                = azurerm_container_registry.acr.id
  role_definition_name = "AcrPull"
  principal_id         = each.key
}

resource "azurerm_role_assignment" "acr_push_role" {
  for_each             = toset(var.acr_push_role_principals)
  scope                = azurerm_container_registry.acr.id
  role_definition_name = "AcrPush"
  principal_id         = each.key
}
