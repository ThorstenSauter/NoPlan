variable "application_insights_connectionstring" {
  type        = string
  description = "The connection string for sending observability data to Azure App Insights."
}

variable "azure_ad_audience" {
  type        = string
  description = "The Azure AD app registration audience."
}

variable "azure_ad_client_id" {
  type        = string
  description = "The Azure AD app registration client id."
}

variable "azure_ad_domain" {
  type        = string
  description = "The Azure AD app registration domain."
}

variable "azure_ad_instance" {
  type        = string
  description = "The Microsoft login instance."
}

variable "azure_ad_tenant_id" {
  type        = string
  description = "The Azure AD app registration tenant id."
}

variable "container_app_name" {
  type        = string
  description = "The name of the Container App resource."
}

variable "container_app_environment_id" {
  type        = string
  description = "The id of the container app environment to associate the container app with."
}

variable "container_cpu" {
  type        = number
  description = "The amount of CPU to assign to the container"
  default     = 0.5
  validation {
    condition     = contains([0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0], var.container_cpu)
    error_message = "The amount of CPU has to be one of: 0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0"
  }
}

variable "container_image" {
  type        = string
  description = "The container image name to run."
}

variable "container_memory" {
  type        = string
  description = "The amount of memory to assign to the container."
  default     = "1Gi"
  validation {
    condition = contains([
      "0.5Gi", "1Gi", "1.5Gi", "2Gi", "2.5Gi", "3Gi", "3.5Gi", "4Gi"
    ], var.container_memory)
    error_message = "The amount of CPU has to be one of: '0.5Gi', '1Gi', '1.5Gi', '2Gi', '2.5Gi', '3Gi', '3.5Gi', '4Gi'."
  }
}

variable "container_name" {
  type        = string
  description = "The name of the container."
}

variable "container_registry" {
  type        = string
  description = "The URI of the Azure Container Registry holding the container image."
}

variable "default_connectionstring" {
  type        = string
  description = "The connection string for the default database."
}

variable "health_check_transport" {
  type        = string
  description = "The type of transport to use for health check probes."
  default     = "HTTP"
  validation {
    condition     = contains(["HTTP", "HTTPS", "TCP"], var.health_check_transport)
    error_message = "The value must be one of: 'HTTP', 'HTTPS' or 'TCP'."
  }
}

variable "liveness_probe_path" {
  type        = string
  description = "The relative HTTP path for the liveness probe."
  default     = "/health/live"
}

variable "managed_identity_id" {
  type        = string
  description = "The resource id of the user-assigned managed identity used to pull images."
}

variable "readiness_probe_path" {
  type        = string
  description = "The relative HTTP path for the readiness probe."
  default     = "/health/ready"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group to deploy to."
}

variable "revision_mode" {
  type        = string
  description = "The type of revision mode to use."
  default     = "Single"
  validation {
    condition     = contains(["Single", "Multiple"], var.revision_mode)
    error_message = "The revision mode must be either 'Single' or 'Multiple'."
  }
}

variable "tags" {
  type        = map(string)
  description = "The default tags for Azure resources."
}

variable "target_port" {
  type        = number
  description = "The port where the application listens on."
  default     = 8080
  validation {
    condition     = var.target_port >= 1 && var.target_port <= 65535
    error_message = "The port has to be within the range [1, 65535]."
  }
}

variable "transport" {
  type        = string
  description = "The transport to use for communication with the app."
  default     = "auto"
  validation {
    condition     = contains(["auto", "http", "http2"], var.transport)
    error_message = "The transport has to be one of: 'auto', 'http' or 'http2'."
  }
}
