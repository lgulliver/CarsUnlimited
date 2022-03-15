using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using CarsUnlimited.InventoryAPI.Repository;
using CarsUnlimited.InventoryAPI.Services;
using CarsUnlimited.InventoryAPI.Configuration;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using CarsUnlimited.Shared.Configuration;

namespace CarsUnlimited.InventoryAPI
{
    public class Startup
    {
        private readonly string _serviceName = "CarsUnlimited.InventoryAPI";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            TracingConfiguration tracingConfig = Configuration.GetSection("TracingConfiguration").Get<TracingConfiguration>();

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
                        })
                        .AddMongoDBInstrumentation());
                    break;
                case TraceExporterOptions.ZIPKIN:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddZipkinExporter(zipkinOptions =>
                        {
                            zipkinOptions.Endpoint = new Uri(tracingConfig.ZipkinEndpoint);
                        })
                        .AddMongoDBInstrumentation());
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
                        })
                        .AddMongoDBInstrumentation());
                    break;
                default:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter()
                        .AddMongoDBInstrumentation());
                    break;
            }

            services.Configure<InventoryDatabaseSettings>(
                 Configuration.GetSection(nameof(InventoryDatabaseSettings)));

            services.AddSingleton<IInventoryDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<InventoryDatabaseSettings>>().Value);

            

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            services.AddScoped<IInventoryService, InventoryService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cars Unlimited Inventory API", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cars Unlimited Inventory API v1"));
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
