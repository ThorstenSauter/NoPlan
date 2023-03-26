output "container_app_environment_default_domain" {
  value = azurerm_container_app_environment.container_environment.default_domain
}

output "container_app_environment_id" {
  value = azurerm_container_app_environment.container_environment.id
}

output "log_analytics_workspace_id" {
  value = azurerm_log_analytics_workspace.workspace.id
}
