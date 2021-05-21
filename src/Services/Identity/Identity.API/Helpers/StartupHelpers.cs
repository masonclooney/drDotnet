using System;
using Autofac;
using drDotnet.BuildingBlocks.EventBus;
using drDotnet.BuildingBlocks.EventBus.Abstractions;
using drDotnet.BuildingBlocks.EventBusRabbitMQ;
using drDotnet.Services.Identity.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace drDotnet.Services.Identity.API.Helpers
{
    public static class StartupHelpers
    {
        static readonly string CorsName = "drDotnetWebCors";

        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsName, builder =>
                {
                    builder.WithOrigins("https://localhost:6001").AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public static void UseCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors(CorsName);
        }

        public static void AddOidcConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IClientRequestParametersProvider, DefaultClientRequestParametersProvider>();
            services.AddSingleton<IAbsoluteUrlFactory, AbsoluteUrlFactory>();
        }

        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(optionBuilder => optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void RegisterDbContextsStaging(this IServiceCollection services)
        {
            var identityDatabaseName = Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(optionBuilder => optionBuilder.UseInMemoryDatabase(identityDatabaseName));
        }

        public static void AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                var retryCount = 5;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
        }

        public static void AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = configuration["SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, iLifetimeScope, retryCount, subscriptionClientName);
            });
        }
    }
}