output "custom_domain_verification_id" {
  value = azurerm_container_app.container_app.custom_domain_verification_id
}

output "outbound_ip_addresses" {
  value = azurerm_container_app.container_app.outbound_ip_addresses
}
