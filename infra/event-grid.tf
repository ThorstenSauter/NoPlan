resource "azurerm_eventgrid_system_topic" "appconfig-changes" {
  name                   = "appconfig-changes"
  resource_group_name    = azurerm_resource_group.core.name
  location               = azurerm_resource_group.core.location
  source_arm_resource_id = azurerm_app_configuration.noplan.id
  topic_type             = "Microsoft.AppConfiguration.ConfigurationStores"
  tags                   = var.tags
  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.noplan.id]
  }
}

resource "azurerm_eventgrid_system_topic_event_subscription" "appconfig-changes" {
  name                                 = "appconfig-changes"
  system_topic                         = azurerm_eventgrid_system_topic.appconfig-changes.name
  resource_group_name                  = azurerm_resource_group.core.name
  service_bus_topic_endpoint_id        = azurerm_servicebus_topic.appconfig-changes.id
  advanced_filtering_on_arrays_enabled = true
  delivery_identity {
    type                   = "UserAssigned"
    user_assigned_identity = azurerm_user_assigned_identity.noplan.id
  }
}
