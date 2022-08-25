resource "azurerm_key_vault" "noplan" {
  name                        = "kv-noplan-${var.env}-001"
  location                    = azurerm_resource_group.core.location
  resource_group_name         = azurerm_resource_group.core.name
  enabled_for_disk_encryption = false
  enable_rbac_authorization   = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 90
  #tfsec:ignore:azure-keyvault-no-purge
  purge_protection_enabled = false
  sku_name                 = "standard"
  tags                     = var.tags
  network_acls {
    bypass = "AzureServices"
    #tfsec:ignore:azure-keyvault-specify-network-acl
    default_action = "Allow"
  }
}

resource "azurerm_role_assignment" "dev-group-noplan-secrets-administrator-role" {
  scope                = azurerm_key_vault.noplan.id
  role_definition_name = "Key Vault Administrator"
  principal_id         = var.developer-group.id
}

resource "azurerm_role_assignment" "github-actions-noplan-secrets-officer-role" {
  scope                = azurerm_key_vault.noplan.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = var.github-actions-principal-id
}

resource "azurerm_role_assignment" "uami-noplan-noplan-secrets-user-role" {
  scope                = azurerm_key_vault.noplan.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_user_assigned_identity.noplan.principal_id
}

#tfsec:ignore:azure-keyvault-ensure-secret-expiry
resource "azurerm_key_vault_secret" "default-connectionstring" {
  name         = "ConnectionStrings--Default"
  value        = var.default-connectionstring
  key_vault_id = azurerm_key_vault.noplan.id
  content_type = "text/plain"
}

#tfsec:ignore:azure-keyvault-ensure-secret-expiry
resource "azurerm_key_vault_secret" "appinsights-connectionstring" {
  name         = "AppInsights--ConnectionString"
  value        = azurerm_application_insights.noplan.connection_string
  key_vault_id = azurerm_key_vault.noplan.id
  content_type = "text/plain"
}

resource "azurerm_key_vault" "integration-test" {
  name                        = "kv-noplan-i-t-001"
  location                    = azurerm_resource_group.core.location
  resource_group_name         = azurerm_resource_group.core.name
  enabled_for_disk_encryption = false
  enable_rbac_authorization   = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 90
  #tfsec:ignore:azure-keyvault-no-purge
  purge_protection_enabled = false
  sku_name                 = "standard"
  tags                     = var.tags
  network_acls {
    bypass = "AzureServices"
    #tfsec:ignore:azure-keyvault-specify-network-acl
    default_action = "Allow"
  }
}

resource "azurerm_role_assignment" "dev-group-integration-test-secrets-administrator-role" {
  scope                = azurerm_key_vault.integration-test.id
  role_definition_name = "Key Vault Administrator"
  principal_id         = var.developer-group.id
}

resource "azurerm_role_assignment" "github-actions-integration-test-secrets-officer-role" {
  scope                = azurerm_key_vault.integration-test.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = var.github-actions-principal-id
}

# TODO add secrets
