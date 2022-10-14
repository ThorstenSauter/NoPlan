output "terraform_service_principal_id" {
  value = data.azurerm_client_config.current.object_id
}
