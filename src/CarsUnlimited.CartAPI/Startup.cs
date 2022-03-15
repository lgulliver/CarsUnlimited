using CarsUnlimited.CartAPI.Configuration;
using CarsUnlimited.CartAPI.Services;
using CarsUnlimited.Shared.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;
using System;

namespace CarsUnlimited.CartAPI
{
    public class Startup
    {
        private readonly string _serviceName = "CarsUnlimited.CartAPI";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            TracingConfiguration tracingConfig = Configuration.GetSection("TracingConfiguration").Get<TracingConfiguration>();
            RedisSettings redisSettings = Configuration.GetSection("RedisSettings").Get<RedisSettings>();

            var redisConfiguration = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                Hosts = new RedisHost[]
                {
                    new RedisHost(){Host = redisSettings.Host, Port = redisSettings.Port},
                },
                AllowAdmin = false,
                Database = 0,
                ConnectTimeout = 10000,
                Ssl = redisSettings.Ssl,
                Password = redisSettings.Password,
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                },
                MaxValueLength = 1024,
                PoolSize = 5
            };

            TraceExporterOptions exporter = (TraceExporterOptions)tracingConfig.Exporter;

            switch (exporter)
            {
                case TraceExporterOptions.JAEGER:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(_serviceName))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddJaegerExporter(jaegerOptions =>
                        {
                            jaegerOptions.AgentHost = tracingConfig.JaegerEndpoint.Host;
                            jaegerOptions.AgentPort = tracingConfig.JaegerEndpoint.Port;
                        }));
                    break;
                case TraceExporterOptions.ZIPKIN:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddZipkinExporter(zipkinOptions =>
                        {
                            zipkinOptions.Endpoint = new Uri(tracingConfig.ZipkinEndpoint);
                        }));
                    break;
                case TraceExporterOptions.OPENTELEMETRY_PROTOCOL:
                    // Adding the OtlpExporter creates a GrpcChannel.
                    // This switch must be set before creating a GrpcChannel/HttpClient when calling an insecure gRPC service.
                    // See: https://docs.microsoft.com/aspnet/core/grpc/troubleshoot#call-insecure-grpc-services-with-net-core-client
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(_serviceName))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = new Uri(tracingConfig.OltpEndpoint);
                        }));
                    break;
                default:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter());
                    break;
            }

            services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(redisConfiguration);

            services.AddScoped<IUpdateCartService, UpdateCartService>();
            services.AddScoped<IGetCartItems, GetCartItems>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cars Unlimited Cart API", Version = "v1" });
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cars Unlimited Cart API v1"));
                app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz");
            });
        }
    }
}
