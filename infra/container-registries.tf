resource "azurerm_container_registry" "core" {
  name                = "crnoplan${var.env}001"
  resource_group_name = azurerm_resource_group.core.name
  location            = azurerm_resource_group.core.location
  sku                 = "Basic"
  admin_enabled       = false
  tags                = var.tags
}

resource "azurerm_role_assignment" "uami-noplan-acr-pull-role" {
  scope                = azurerm_container_registry.core.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.noplan.principal_id
}

resource "azurerm_role_assignment" "github-actions-acr-push-role" {
  scope                = azurerm_container_registry.core.id
  role_definition_name = "AcrPush"
  principal_id         = var.github-actions-principal-id
}
