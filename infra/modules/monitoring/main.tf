resource "azurerm_application_insights" "appinsights" {
  name                          = var.app_insights_name
  location                      = var.location
  resource_group_name           = var.resource_group_name
  workspace_id                  = var.log_analytics_workspace_id
  application_type              = "web"
  local_authentication_disabled = true
  sampling_percentage           = 0
  tags                          = var.tags
}

resource "azurerm_role_assignment" "monitoring_metrics_publisher_role" {
  scope                = azurerm_application_insights.appinsights.id
  role_definition_name = "Monitoring Metrics Publisher"
  principal_id         = var.managed_identity_principal_id
}
