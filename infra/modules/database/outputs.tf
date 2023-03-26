output "fully_qualified_domain_name" {
  value = azurerm_mssql_server.database_server.fully_qualified_domain_name
}

output "connectionstring" {
  value = "Server=tcp:${azurerm_mssql_server.database_server.fully_qualified_domain_name},1433;Database=${var.database_name};User Id=${var.database_server_administrator_login};Password=${var.database_server_administrator_password};"
}
