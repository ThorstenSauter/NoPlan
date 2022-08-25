variable "appconfig-label" {
  type        = string
  default     = "prod"
  description = "The App Configuration label to apply to keys"
}

variable "azuread-configuration" {
  type        = string
  default     = <<-EOT
      {
        "Instance": "https://login.microsoftonline.com/",
        "Domain": "thorstensauter.dev",
        "TenantId": "ffab38df-cddf-433f-859a-6cfa161a5ceb",
        "ClientId": "d64c2b98-7a16-44ac-a368-7267da975eb9",
        "Audience": "https://noplan.thorstensauter.dev"
      }
    EOT
  description = "The Azure AD configuration to use"
}

variable "default-connectionstring" {
  type        = string
  sensitive   = true
  description = "The default connection string for the NoPlan API"
}

variable "developer-group" {
  type = map(string)
  default = {
    id   = "914fff5e-bedf-443b-82ad-c4ceccb192c3"
    name = "NoPlan Developers"
  }
  description = "The AAD group containing the app developers"
}

variable "env" {
  type        = string
  default     = "dev"
  description = "The environment to deploy to"
}

variable "github-actions-principal-id" {
  type        = string
  default     = "49e7a027-01d8-4712-aca6-6ecc3c462c61"
  description = "The object id of the service principal running GitHub Actions"
}

variable "location" {
  type        = string
  default     = "westeurope"
  description = "The location to deploy Azure resources to"
}

variable "sentinel-value" {
  type        = number
  default     = 0
  description = "The arbitrary sentinel value for uniformly reloading configuration"
}

variable "tags" {
  type = map(string)
  default = {
    "environment" : "Development"
    "project" : "NoPlan"
    "managed-by" : "Terraform"
  }
  description = "The default tags for Azure resources"
}
