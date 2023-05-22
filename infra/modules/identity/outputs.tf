output "identity_api_id" {
  value = azurerm_user_assigned_identity.uami.id
}

output "identity_client_id" {
  value = azurerm_user_assigned_identity.uami.client_id
}

output "identity_api_principal_id" {
  value = azurerm_user_assigned_identity.uami.principal_id
}
