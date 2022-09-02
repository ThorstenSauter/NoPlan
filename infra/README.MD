# Infrastructure

This directory contains the Infrastructure-as-Code (`IaC`) definitions for the [NoPlan API](../src/NoPlan.Api) project using [Terraform](https://terraform.io).

Currently all resources are managed in a single top-level Terraform module, but down the line these could be split into modules where appropriate. One example would be splitting resources that can be shared between different environments (e.g. the container registry) into their own core resource group.

This directory also contains the configuration for [TFLint](https://github.com/terraform-linters/tflint), which is used to analyze the resource definitions on every pull request.
