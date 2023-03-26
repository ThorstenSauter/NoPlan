output "container_registry_id" {
  value = module.container_registry.container_registry_id
}

output "container_registry_endpoint" {
  value = module.container_registry.container_registry_endpoint
}

output "container_registry_name" {
  value = module.container_registry.container_registry_name
}

output "subscription_id" {
  value = data.azurerm_client_config.current.subscription_id
}

output "tenant_id" {
  value = data.azurerm_client_config.current.tenant_id
}

output "resource_group_name" {
  value = module.resource_group.name
}
