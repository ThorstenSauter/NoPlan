variable "location" {
  type        = string
  description = "The location/region where the resource group is created."
}

variable "name" {
  type        = string
  description = "The name of the resource group."
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resources."
}
