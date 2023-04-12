resource "azurerm_mssql_server" "database_server" {
  name                         = var.database_server_name
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = "12.0"
  administrator_login          = var.database_server_administrator_login
  administrator_login_password = var.database_server_administrator_password
  minimum_tls_version          = "1.2"
}

resource "azurerm_mssql_firewall_rule" "firewall_rule_allow_azure_services" {
  name             = "FirewallRule1"
  server_id        = azurerm_mssql_server.database_server.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

resource "azurerm_mssql_database" "database" {
  name                        = var.database_name
  server_id                   = azurerm_mssql_server.database_server.id
  collation                   = "SQL_Latin1_General_CP1_CI_AS"
  auto_pause_delay_in_minutes = 60
  max_size_gb                 = 1
  min_capacity                = 0.5
  sku_name                    = "GP_S_Gen5_1"
  geo_backup_enabled          = true
  zone_redundant              = false
  tags                        = var.tags
}
