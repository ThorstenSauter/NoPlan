output "api_application_id" {
  value = azuread_application.api.application_id
}

output "api_audience" {
  value = one(azuread_application.api.identifier_uris)
}

output "api_tenant_id" {
  value = data.azuread_client_config.current.tenant_id
}

output "container_registry_id" {
  value = module.container_registry.container_registry_id
}

output "container_registry_endpoint" {
  value = module.container_registry.container_registry_endpoint
}

output "container_registry_name" {
  value = module.container_registry.container_registry_name
}

output "default_domain" {
  value = data.azuread_domains.default_domain.domains.0.domain_name
}

output "integration_testing_application_id" {
  value = azuread_application.integration_testing.application_id
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
