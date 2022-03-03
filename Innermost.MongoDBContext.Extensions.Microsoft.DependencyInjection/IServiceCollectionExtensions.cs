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
        /// <summary>
        /// To inject MongoDBContext more easily.
        /// </summary>
        /// <typeparam name="TMongoDBContext">Custom MongoDBContext which inherit MongoDBContextBase.</typeparam>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configurationAction">To build configuration for MongoDBContext and to inject.</param>
        /// <param name="contextLifetime">ContextLifeTime defualt is Scoped.</param>
        /// <param name="configurationLifetime">ConfigurationLifeTime defualt is Scoped.</param>
        /// <returns>ServiceCollection</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddMongoDBContext<TMongoDBContext>(
            this IServiceCollection services,
            Action<MongoDBContextConfigurationBuilder<TMongoDBContext>> configurationAction, 
            ServiceLifetime contextLifetime = ServiceLifetime.Singleton, 
            ServiceLifetime configurationLifetime = ServiceLifetime.Scoped
            )
            where TMongoDBContext : MongoDBContextBase
        {
            //add configuration to serviceCollection
            services.AddMongoDBContextConfiguration(configurationAction, configurationLifetime);

            if (!HasConstructorWithConfiguration<TMongoDBContext>())
                throw new ArgumentException("MongoDBContext inherit to MongoDBContextBase must has a constructor with a MongoDBContextConfiguration<TMongoDBContext> param and pass it to MongoDBContextBase.");

            //Add MongoDBContext to ServiceCollection.
            switch (contextLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TMongoDBContext>();
                    break;

                case ServiceLifetime.Scoped:
                    services.AddScoped<TMongoDBContext>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<TMongoDBContext>();
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
            where TMongoDBContext : MongoDBContextBase
        {
            //Build mongoDBContextConfiguration to inject.
            var mongoDBContextConfigurationBuilder = new MongoDBContextConfigurationBuilder<TMongoDBContext>();
            configurationAction(mongoDBContextConfigurationBuilder);
            var mongoDBContextConfiguration = mongoDBContextConfigurationBuilder.Build();
            //ServiceProvider will get the same instance of MongoDBContextConfigurationBuilder<TMongoDBContext> whatever ServiceLifetime is defined.
            //In other words,mongoDBContextConfiguration is singoton.
            switch (configurationLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<MongoDBContextConfiguration<TMongoDBContext>>(mongoDBContextConfiguration);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<MongoDBContextConfiguration<TMongoDBContext>>((s) => mongoDBContextConfiguration);
                    break;
                case ServiceLifetime.Transient:
                    //We return mongoDBContextConfiguration so even AddTransient we will get same reference of MongoDBContextConfiguration.
                    services.AddTransient<MongoDBContextConfiguration<TMongoDBContext>>((s) => mongoDBContextConfiguration);
                    break;
                default:
                    break;
            }

            return services;
        }

        /// <summary>
        /// To judge that is there has ConstructorWithConfiguration of TMongoDBContext.
        /// </summary>
        /// <typeparam name="TMongoDBContext"></typeparam>
        /// <returns></returns>
        private static bool HasConstructorWithConfiguration<TMongoDBContext>() where TMongoDBContext : MongoDBContextBase
        {
            var hasConstructor = typeof(TMongoDBContext)
                                .GetConstructors()
                                .Any((c) => c.GetParameters().Any(p => p.ParameterType == typeof(MongoDBContextConfiguration<TMongoDBContext>)));

            return hasConstructor;
        }
    }
}
