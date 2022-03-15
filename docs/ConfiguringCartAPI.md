# Configuring Cart API and Worker

# API

All configuration can be overridden with environment vars.

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
  "TracingConfiguration": {
    "JaegerEndpoint": {
      "Host": "localhost",
      "Port": 6831
    }
  },
  "RedisSettings": {
    "Host": "localhost",
    "Port": 6379,
    "Ssl": false,
    "Password": ""
  },
  "ServiceBusConfiguration": {
    "HostName": "localhost",
    "UserName": "user",
    "Password": "user"
  }
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

## Redis Settings

| Property | Description | Default Value |
| -------- | ----------- | ------------- |
| Host | Hostname of the Redis server | localhost |
| Port | Port of the Redis server | 6379 |
| Ssl | Whether to use SSL to connect to the Redis server | false |
| Password | Password to use when connecting to the Redis server | "" |

## Service Bus Configuration

| Property | Description | Default Value |
| -------- | ----------- | ------------- |
| HostName | Hostname of the Service Bus | localhost |
| UserName | Username to use when connecting to the Service Bus | user |
| Password | Password to use when connecting to the Service Bus | user |

# Worker

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
  "CartApiUrl": "http://localhost:5020/api/Cart/",
  "CartApiKey": "THIS_IS_MY_KEY"
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

## Cart API Url

This is the full qualified URL to the Cart API.

## Cart API Key

This is the API key specified in the app settings for the API. It *must* match the API key specified in the app settings for the API.