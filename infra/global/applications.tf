data "azuread_application_published_app_ids" "well_known" {}

data "azuread_domains" "default_domain" {
  only_default = true
}

locals {
  api_user_scope_id = "b2aa6d1e-e1ce-4266-b42e-84788bc676a2"
}

resource "azuread_application" "api" {
  display_name = "NoPlan API"
  identifier_uris = [
    "https://noplan.thorstensauter.dev"
  ]
  owners = [
    data.azuread_client_config.current.object_id
  ]
  sign_in_audience = "AzureADMyOrg"

  feature_tags {
    hide = true
  }

  api {
    oauth2_permission_scope {
      id                         = local.api_user_scope_id
      admin_consent_description  = "Access the NoPlan API as a user"
      admin_consent_display_name = "Access the NoPlan API as a user"
      enabled                    = true
      type                       = "Admin"
      user_consent_description   = "Access the NoPlan API as a user"
      user_consent_display_name  = "Access the NoPlan API as a user"
      value                      = "User"
    }
  }

  required_resource_access {
    resource_app_id = data.azuread_application_published_app_ids.well_known.result.MicrosoftGraph
    resource_access {
      id   = azuread_service_principal.msgraph.oauth2_permission_scope_ids["User.Read"]
      type = "Scope"
    }
  }

  web {
    homepage_url = "https://noplan.thorstensauter.dev/api"
    redirect_uris = [
      "https://jwt.ms/",
      "https://oauth.pstmn.io/v1/callback"
    ]
  }
}

resource "azuread_application" "integration_testing" {
  display_name                   = "NoPlan Integration Testing"
  fallback_public_client_enabled = true
  owners = [
    data.azuread_client_config.current.object_id
  ]
  sign_in_audience = "AzureADMyOrg"

  feature_tags {
    hide = true
  }

  required_resource_access {
    resource_app_id = data.azuread_application_published_app_ids.well_known.result.MicrosoftGraph
    resource_access {
      id   = azuread_service_principal.msgraph.oauth2_permission_scope_ids["User.Read"]
      type = "Scope"
    }
  }

  required_resource_access {
    resource_app_id = azuread_application.api.client_id
    resource_access {
      id   = local.api_user_scope_id
      type = "Scope"
    }
  }
}

resource "azuread_service_principal" "api" {
  client_id                    = azuread_application.api.client_id
  app_role_assignment_required = false
  owners = [
    data.azuread_client_config.current.object_id
  ]
}

resource "azuread_service_principal" "integration_testing" {
  client_id                    = azuread_application.integration_testing.client_id
  app_role_assignment_required = false
  owners = [
    data.azuread_client_config.current.object_id
  ]
}

resource "azuread_service_principal" "msgraph" {
  client_id    = data.azuread_application_published_app_ids.well_known.result.MicrosoftGraph
  use_existing = true
}
