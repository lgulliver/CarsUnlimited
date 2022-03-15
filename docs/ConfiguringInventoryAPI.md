# Configuring Inventory API and Worker

# API

These settings can be overridden with environment variables.

## Default Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "InventoryDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CarsUnlimitedDb"
  },
  "TracingConfiguration": {
    "Exporter": -1
  },
  "InventoryApiKey": "THIS_IS_MY_KEY"
}
```

## Logging

Logging uses the default .NET logging options. Please see [Configuring logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0#configure-logging-1) in the Microsoft documentation for more information.

## Allowed Hosts

`AllowedHosts` is used to restrict access to the API. By default it is set to `*` which allows all hosts. If you want to restrict access to a specific host or a list of hosts, you can set `AllowedHosts` to a semi-colon (`;`) separated list of hosts.

```json
{
  "AllowedHosts": "example.com;localhost"
}
```

## Tracing Configuration

See [Configuring Telemetry](ConfiguringTelemetry.md) for more information.

## Inventory Database Settings

| Property | Description | Default Value |
| -------- | ----------- | ------------- |
| ConnectionString | Connection string to the MongoDB database | mongodb://localhost:27017 |
| DatabaseName | Name of the MongoDB database | CarsUnlimitedDb |

The `ConnectionString` is used to connect to the MongoDB database. The `DatabaseName` is used to specify the name of the database.

Change the `ConnectionString` to connect to your MongoDB instance when deploying.

## Inventory API Key

The `InventoryApiKey` is the value needed by other components to authenticate the Inventory API.

This can be left as this value for now.

__Note:__ This is not the best way to do this and in future it will be replaced with a more secure method.

# Worker

These settings can be overridden with environment variables.

## Default Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ServiceBusConfiguration": {
    "HostName": "localhost",
    "UserName": "user",
    "Password": "user"
  },
  "InventoryApiUrl": "http://localhost:5010/api/Inventory/",
  "InventoryApiKey": "THIS_IS_MY_KEY"
}
```

## Logging

Logging uses the default .NET logging options. Please see [Configuring logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0#configure-logging-1) in the Microsoft documentation for more information.

## Service Bus Configuration

The worker uses RabbitMQ to handle messages for the Inventory API. Any `AMPQ` compatible service bus should also work, though only RabbitMQ has been tested.

The `ServiceBusConfiguration` section is used to configure the connection to the Service Bus.

| Property | Description | Default Value |
| -------- | ----------- | ------------- |
| HostName | Host name of the Service Bus | localhost |
| UserName | User name to connect to the Service Bus | user |
| Password | Password to connect to the Service Bus | user |

## Inventory API Url

This is the full qualified URL to the Inventory API.

## Inventory API Key

This is the API key specified in the app settings for the API. It *must* match the API key specified in the app settings for the API.
