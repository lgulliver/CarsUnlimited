# Cars Unlimited

This project is to replace v1 of Cars Unlimited used in Docker, Azure DevOps and Kubernetes workshops. It's purpose is to provide a portable, multi-service architecture utilising modern technologies such as Open Telemetry, Open API and Kubernetes. It will also serve as an example for polyglot architectures much in the same way as eShopOnContainers but in a much more rudimentary format.

It is a work in progress.

# Overview

![Cars Unlimited](/docs/CarsUnlimitedv2.png)

- .NET 5 is used for:
    - Inventory API
    - Inventory Worker
    - Cart API
    - Cart Worker
- Golang is used for:
    - Purchase API
- MongoDB is used for:
    - Inventory data    
- Redis is used for:
    - Cart data
- Docker is used for:
    - Inventory
    - Purchase
    - Cart
    - The UI does not presently use Docker
- React is used for:
    - The Web Front End client

Not in this diagram is some supporting local services such as:

- Jaeger for Open Telemetry (see: [Configuring Telemetry](/docs/ConfiguringTelemetry.md))

**NOTE:** Jaeger is currently only used locally and in docker compose, but telemetry is configured to output to console by default.

# Open API

Open API is enabled on all services with the exception of the Purchase API at this time.

When running locally either in debug or with Docker/Docker Compose, you can access the Open API interface under `/swagger`

# Configuration

- [Configuring Telemetry](/docs/ConfiguringTelemetry.md)
- [Configuring Inventory API and Worker](/docs/ConfiguringInventoryAPI.md)
- [Configuring Cart API and Worker](/docs/ConfiguringCartAPI.md)
- [Configuring Purchase API](/docs/ConfiguringPurchaseAPI.md)
- [Configuring Web Client](/docs/ConfiguringWebClient.md)
# Useful Links

- [Cars Unlimited v1](https://github.com/MMTDigital/CarsUnlimited)

# Getting Started Locally

You will need the following:

- Docker
- .NET 5
- Node 14.x
- Go 1.16 or better

## Docker Compose

There are several docker compose files in this repository. Navigating to the `/src` directory and running `docker compose up --build` will bring up all services, components and dependencies, with the exception of the UI.

## Web UI

Navigate to the UI project and run `npm start .env.development` to start the UI locally. Once the UI is running, you can navigate to `http://localhost:3000` to see the UI.

## Seeding data

The supplied MongoDB container automatically seeds data. It's used by the Inventory API and will be displayed in the UI once present.