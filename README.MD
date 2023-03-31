# NoPlan

## Goals

This project aims to deliver task tracking capabilities and is primarily to learn about new technologies. The goal is to
follow best practices whenever possible.

## Status

![Staging and global](https://github.com/ThorstenSauter/NoPlan/actions/workflows/deploy-global-and-staging.yml/badge.svg) ![Production](https://github.com/ThorstenSauter/NoPlan/actions/workflows/deploy-production.yml/badge.svg)

![Code scans](https://github.com/ThorstenSauter/NoPlan/actions/workflows/codeql-analysis.yml/badge.svg) [![codecov](https://codecov.io/gh/ThorstenSauter/NoPlan/branch/main/graph/badge.svg?token=ZSYIYFC9D6)](https://codecov.io/gh/ThorstenSauter/NoPlan)

## Calling the API

In order to coveniently call the API during development or after deployment, a [Postman](https://www.postman.com/)
collection
is available to be forked. Make sure to set up the proper authentication parameters before calling any protected
endpoints.

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/11545383-44b0d3ed-b834-48df-a31a-bac27c54e41d?action=collection%2Ffork&collection-url=entityId%3D11545383-44b0d3ed-b834-48df-a31a-bac27c54e41d%26entityType%3Dcollection%26workspaceId%3Dcb449a10-b5f3-439f-812b-e1ac13437c9b)

## Infrastructure

The projects required infrastructure is hosted in [Microsoft Azure](https://azure.microsoft.com) and is managed
using [Terraform](https://terraform.io). The resource definitions can be found in the [infra](infra) directory.

The Terraform state is managed by Terraform Cloud. Pushes to the `main` branch result in the global and staging
environments being deployed. The publishing of a GitHub release results in the production environment being deployed.

## Health checks

The API implements the following two HTTP-based health checks:

### Liveness probe

A liveness probe, which simply sends an HTTP response and does not run any additional health checks, is available on
the `/health/live` endpoint.

### Readiness probe

A readiness probe is available on the `/health/ready` endpoint and runs the following health checks:

- The Azure SQL Server instance

## Observability

Metrics, traces and logs are all handled using [OpenTelemetry](https://opentelemetry.io).

During development the OpenTelemetry Collector is the target for exporting data and subsequently transferring it to
Prometheus, Grafaka Loki and Jaeger. The complete stack can be run locally using
the provided [Docker Compose file](local-development-stack/docker-compose.yaml).

In production all data is exported using the Azure Monitor Exporter, which makes the observability data available in
Azure Application Insights.
