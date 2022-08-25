resource "azurerm_servicebus_namespace" "noplan" {
  name                = "sb-noplan-${var.env}-001"
  location            = azurerm_resource_group.core.location
  resource_group_name = azurerm_resource_group.core.name
  sku                 = "Standard"
  local_auth_enabled  = false
  tags                = var.tags
}

resource "azurerm_servicebus_topic" "appconfig-changes" {
  name                      = "appconfig-changes"
  namespace_id              = azurerm_servicebus_namespace.noplan.id
  enable_batched_operations = false
  enable_partitioning       = false
  support_ordering          = false
}

resource "azurerm_servicebus_subscription" "appconfig-changes" {
  name                                      = "appconfig-changes"
  topic_id                                  = azurerm_servicebus_topic.appconfig-changes.id
  dead_lettering_on_filter_evaluation_error = false
  enable_batched_operations                 = false
  max_delivery_count                        = 3
  dead_lettering_on_message_expiration      = true
  default_message_ttl                       = "P1D"
}

resource "azurerm_role_assignment" "github-actions-service-bus-data-owner-role" {
  principal_id         = var.github-actions-principal-id
  role_definition_name = "Azure Service Bus Data Owner"
  scope                = azurerm_servicebus_namespace.noplan.id
}

resource "azurerm_role_assignment" "uami-noplan-data-sender-role" {
  principal_id         = azurerm_user_assigned_identity.noplan.principal_id
  role_definition_name = "Azure Service Bus Data Sender"
  scope                = azurerm_servicebus_namespace.noplan.id
}

resource "azurerm_role_assignment" "uami-noplan-data-receiver-role" {
  principal_id         = azurerm_user_assigned_identity.noplan.principal_id
  role_definition_name = "Azure Service Bus Data Receiver"
  scope                = azurerm_servicebus_namespace.noplan.id
}
