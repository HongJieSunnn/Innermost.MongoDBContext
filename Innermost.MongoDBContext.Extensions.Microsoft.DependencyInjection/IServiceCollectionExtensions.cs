using Innermost.MongoDBContext.Configurations;
using Innermost.MongoDBContext.Configurations.Abstractions;
using Innermost.MongoDBContext.Configurations.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDBContext<TMongoDBContext>(
            this IServiceCollection services,
            Action<MongoDBContextConfigurationBuilder<TMongoDBContext>> configurationAction, 
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, 
            ServiceLifetime configurationLifetime = ServiceLifetime.Scoped
            )
            where TMongoDBContext : MongoDBContext
        {
            //add configuration to serviceCollection
            services.AddMongoDBContextConfiguration(configurationAction, configurationLifetime);

            Func<IServiceProvider, TMongoDBContext> implementationFactory = (services) =>
            {
                var configuration = services.GetRequiredService<MongoDBContextConfiguration>();
                var mongoDBContext = new MongoDBContext(configuration);//MongoDBContext class use reflection to initial the collections in implemented class(TMongoDBContext).
                return (TMongoDBContext)mongoDBContext;
            };
            //Add MongoDBContext to ServiceCollection.
            switch (contextLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TMongoDBContext>(implementationFactory);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<TMongoDBContext>(implementationFactory);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<TMongoDBContext>(implementationFactory);
                    break;
                default:
                    break;
            }

            return services;
        }

        private static IServiceCollection AddMongoDBContextConfiguration<TMongoDBContext>(
            this IServiceCollection services,
            Action<MongoDBContextConfigurationBuilder<TMongoDBContext>> configurationAction,
            ServiceLifetime configurationLifetime
            )
            where TMongoDBContext : MongoDBContext
        {
            //Build mongoDBContextConfiguration to inject.
            var mongoDBContextConfigurationBuilder = new MongoDBContextConfigurationBuilder<TMongoDBContext>();
            configurationAction(mongoDBContextConfigurationBuilder);
            var mongoDBContextConfiguration = mongoDBContextConfigurationBuilder.Build();

            switch (configurationLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<MongoDBContextConfiguration>(mongoDBContextConfiguration);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<MongoDBContextConfiguration>((s) => mongoDBContextConfiguration);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<MongoDBContextConfiguration>((s) => mongoDBContextConfiguration);
                    break;
                default:
                    break;
            }

            return services;
        }

    }
}
