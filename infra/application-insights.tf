resource "azurerm_application_insights" "noplan" {
  name                = "appi-noplan-${var.env}-001"
  location            = azurerm_resource_group.core.location
  resource_group_name = azurerm_resource_group.core.name
  workspace_id        = azurerm_log_analytics_workspace.noplan-appinsights-workspace.id
  application_type    = "web"
  sampling_percentage = 0
  tags                = var.tags
}

resource "azurerm_log_analytics_workspace" "noplan-appinsights-workspace" {
  name                = "log-noplan-${var.env}-001"
  location            = azurerm_resource_group.core.location
  resource_group_name = azurerm_resource_group.core.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = var.tags
}
