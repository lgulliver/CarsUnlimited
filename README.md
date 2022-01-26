# Cars Unlimited

This project is to replace v1 of Cars Unlimited used in Docker, Azure DevOps and Kubernetes workshops. It's purpose is to provide a portable, multi-service architecture utilising modern technologies such as Open Telemetry, Open API and Kubernetes. It will also serve as an example for polyglot architectures much in the same way as eShopOnContainers but in a much more rudimentary format.

It is a work in progress.

## Pre-Requisites

- [ ] TODO - Add instructions regarding Mongo installation (consider adding this into a one off setup script that can be ran by new users)
- [ ] TODO - Test URLs for Swagger
- [ ] TODO - Test scenarios that can be ran in Postman
- [ ] TODO - Instructions on how to locally debug Each API
- [ ] TODO - Instructions around Testing via Docker


## Overview

![Cars Unlimited](/docs/CarsUnlimitedv2.png)

## Technologies

This is a list of technologies used (or intended to be used).

- Docker
- Kubernetes
- Blazor Server
- Xamarin
- Kong API Gateway
- MongoDB
- Redis
- .NET 5
- Prometheus
- Grafana
- Kiali
- Golang

## Docker Compose
Docker compose is to be used for local dev testing. The compose files are aplit up between the master docker-compose.yml file which contains all services and dependencies to spin up the full service. There are individual compose files the spin up individual services plus their dependencies for testing in isolation.

## Useful Links

- [Cars Unlimited v1](https://github.com/MMTDigital/CarsUnlimited)
