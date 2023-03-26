# Infrastructure

This directory contains the Infrastructure-as-Code (`IaC`) definitions for the [NoPlan API](../src/NoPlan.Api) project
using [Terraform](https://terraform.io) and [Terraform Cloud](https://app.terraform.io).

## Environments

It contains three separate environments:

## Global environment

This environment contains the resources that are shared between the other environments. This helps to reduce cost (e.g.
by using a shared Azure Container Registry) and allows for easy promotion of images.

## Linting

This directory also contains the central configuration for [TFLint](https://github.com/terraform-linters/tflint), which
is used to analyze the resource definitions on every pull request.

## Security

The infrastructure definitions are checked using [tfsec](https://github.com/aquasecurity/tfsec).
