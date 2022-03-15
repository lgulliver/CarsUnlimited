# Configuring the Purchase API

The Purchase API is written in Golang. It is written to use environment variables to configure the API as opposed to an `appsettings.json` file, however it will support an `appsettings.json` file.

## Configuration settings

| Property | Description | Default Value |
| -------- | ----------- | ------------- |
| SERVICE_BUS_CONNECTION_STRING | Connection string to the Service Bus | amqp://user:bitnami@rabbitmq:5672/ |