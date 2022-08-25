resource "azurerm_app_configuration" "noplan" {
  name                = "appcs-noplan-${var.env}"
  resource_group_name = azurerm_resource_group.core.name
  location            = azurerm_resource_group.core.location
  sku                 = "free"
  tags                = var.tags
}

resource "azurerm_role_assignment" "github-actions-configuration-data-owner-role" {
  scope                = azurerm_app_configuration.noplan.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = var.github-actions-principal-id
}

resource "azurerm_role_assignment" "dev-group-configuration-data-owner-role" {
  scope                = azurerm_app_configuration.noplan.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = var.developer-group.id
}

resource "azurerm_role_assignment" "uami-noplan-app-configuration-data-reader-role" {
  scope                = azurerm_app_configuration.noplan.id
  role_definition_name = "App Configuration Data Reader"
  principal_id         = azurerm_user_assigned_identity.noplan.principal_id
}

resource "azurerm_app_configuration_key" "applicationinsights-connectionstring" {
  configuration_store_id = azurerm_app_configuration.noplan.id
  key                    = "ApplicationInsights:ConnectionString"
  label                  = var.appconfig-label
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.appinsights-connectionstring.versionless_id
  depends_on = [
    azurerm_role_assignment.github-actions-configuration-data-owner-role
  ]
}

resource "azurerm_app_configuration_key" "azuread" {
  configuration_store_id = azurerm_app_configuration.noplan.id
  key                    = "AzureAd"
  label                  = var.appconfig-label
  value                  = var.azuread-configuration
  content_type           = "application/json"
  depends_on = [
    azurerm_role_assignment.github-actions-configuration-data-owner-role
  ]
}

resource "azurerm_app_configuration_key" "default-connectionstring" {
  configuration_store_id = azurerm_app_configuration.noplan.id
  key                    = "ConnectionStrings:Default"
  label                  = var.appconfig-label
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.default-connectionstring.versionless_id
  depends_on = [
    azurerm_role_assignment.github-actions-configuration-data-owner-role
  ]
}

resource "azurerm_app_configuration_key" "sentinel" {
  configuration_store_id = azurerm_app_configuration.noplan.id
  key                    = "Sentinel"
  label                  = var.appconfig-label
  value                  = var.sentinel-value
  content_type           = "text/plain"
  depends_on = [
    azurerm_role_assignment.github-actions-configuration-data-owner-role
  ]
}
