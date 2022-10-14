resource "azurerm_app_configuration" "app_config" {
  name                = var.app_configuration_name
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = var.sku
  local_auth_enabled  = var.local_auth_enabled
  tags                = var.tags
}

resource "azurerm_role_assignment" "app_configuration_data_owner_role" {
  for_each             = toset(var.app_configuration_data_owner_role_principals)
  scope                = azurerm_app_configuration.app_config.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = each.key
}

resource "azurerm_role_assignment" "app_configuration_data_reader_role" {
  for_each             = toset(var.app_configuration_data_reader_role_principals)
  scope                = azurerm_app_configuration.app_config.id
  role_definition_name = "App Configuration Data Reader"
  principal_id         = each.key
}
