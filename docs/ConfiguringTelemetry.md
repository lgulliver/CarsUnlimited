# Configuring Telemetry

Cars Unlimited is built using Open Telemetry, with support for multiple trace exporters.

- Console [DEFAULT]
- Jaeger
- Zipkin
- Open Telemetry Protocol

In both Inventory and Cart APIs, this is configured using either `appsettings.json` or environment variables.

In `appsettings.json` this is configured under the `TracingConfiguration` block.

`Exporter` accepts `-1` for Console, `0` for Jaeger, `1` for Zipkin and `2` for Open Telemetry Protocol.

## Jaeger

```json
"TracingConfiguration": {
    "Exporter": 0,
    "JaegerEndpoint": {
      "Host": "<jaeger host name>",
      "Port": 6831
    }
}
```