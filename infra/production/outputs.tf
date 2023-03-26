output "container_app_custom_domain_verification_id" {
  value     = module.container_app.custom_domain_verification_id
  sensitive = true
}

output "container_app_outbound_ip_addresses" {
  value = module.container_app.outbound_ip_addresses
}

output "identity_api_id" {
  value = module.identity.identity_api_id
}

output "identity_api_principal_id" {
  value = module.identity.identity_api_principal_id
}

output "terraform_principle_id" {
  value = data.azurerm_client_config.current.object_id
}
