resource "azurerm_container_app" "container_app" {
  name                         = var.container_app_name
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = var.revision_mode
  tags                         = var.tags
  registry {
    server   = var.container_registry
    identity = var.managed_identity_id
  }
  ingress {
    allow_insecure_connections = false
    external_enabled           = true
    target_port                = var.target_port
    transport                  = var.transport
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
  identity {
    type         = "UserAssigned"
    identity_ids = [var.managed_identity_id]
  }
  template {
    container {
      name   = var.container_name
      image  = var.container_image
      cpu    = var.container_cpu
      memory = var.container_memory
      liveness_probe {
        path             = var.liveness_probe_path
        port             = var.target_port
        transport        = var.health_check_transport
        initial_delay    = 1
        interval_seconds = 1
        timeout          = 1
      }
      readiness_probe {
        path                    = var.readiness_probe_path
        port                    = var.target_port
        transport               = var.health_check_transport
        interval_seconds        = 10
        timeout                 = 5
        success_count_threshold = 1
      }
      env {
        name        = "APPLICATIONINSIGHTS_CONNECTION_STRING"
        secret_name = "applicationinsights-connectionstring"
      }
      env {
        name  = "AzureAD__Audience"
        value = var.azure_ad_audience
      }
      env {
        name  = "AzureAD__ClientId"
        value = var.azure_ad_client_id
      }
      env {
        name  = "AzureAD__Domain"
        value = var.azure_ad_domain
      }
      env {
        name  = "AzureAD__Instance"
        value = var.azure_ad_instance
      }
      env {
        name  = "AzureAD__TenantId"
        value = var.azure_ad_tenant_id
      }
      env {
        name  = "AZURE_CLIENT_ID"
        value = var.managed_identity_client_id
      }
      env {
        name        = "ConnectionStrings__Default"
        secret_name = "connectionstrings-default"
      }
      env {
        name  = "OTEL_SERVICE_NAME"
        value = var.container_app_name
      }
    }
  }
  secret {
    name  = "applicationinsights-connectionstring"
    value = var.application_insights_connectionstring
  }
  secret {
    name  = "connectionstrings-default"
    value = var.default_connectionstring
  }
}
