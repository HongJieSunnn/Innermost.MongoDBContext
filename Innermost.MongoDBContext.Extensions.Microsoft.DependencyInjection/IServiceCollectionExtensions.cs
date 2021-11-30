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
            where TMongoDBContext : MongoDBContextBase
        {
            //add configuration to serviceCollection
            services.AddMongoDBContextConfiguration(configurationAction, configurationLifetime);

            Func<IServiceProvider, TMongoDBContext> implementationFactory = (services) =>
            {
                var configuration = services.GetRequiredService<MongoDBContextConfiguration<TMongoDBContext>>();
                var mongoDBContext = new MongoDBContextBase(configuration);//MongoDBContext class use reflection to initial the collections in implemented class(TMongoDBContext).
                return (TMongoDBContext)mongoDBContext;
            };
            //Add MongoDBContext to ServiceCollection.
            switch (contextLifetime)
            {
                case ServiceLifetime.Singleton:
                    //If TMongoDBContext has not Constructor with MongoDBContextConfiguration<TMongoDBContext> we should create TMongoDBContext by MongoDBContextBase.
                    //Or we can just inject TMongoDBContext and ASPNETCore will create TMongoDBContext by MongoDBContextConfiguration<TMongoDBContext> we injected in AddMongoDBContextConfiguration.
                    //TODO.Actually,there can be easiler.We can just remove the injection of MongoDBContextConfiguration<TMongoDBContext> and create TMongoDBContext by MongoDBContextBase.
                    if (!HasConstructorWithConfiguration<TMongoDBContext>())
                        services.AddSingleton<TMongoDBContext>(implementationFactory);
                    else
                        services.AddSingleton<TMongoDBContext>();
                    break;

                case ServiceLifetime.Scoped:
                    if (!HasConstructorWithConfiguration<TMongoDBContext>())
                        services.AddScoped<TMongoDBContext>(implementationFactory);
                    else
                        services.AddScoped<TMongoDBContext>();
                    break;

                case ServiceLifetime.Transient:
                    if (!HasConstructorWithConfiguration<TMongoDBContext>())
                        services.AddTransient<TMongoDBContext>(implementationFactory);
                    else
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

            switch (configurationLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<MongoDBContextConfiguration<TMongoDBContext>>(mongoDBContextConfiguration);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<MongoDBContextConfiguration<TMongoDBContext>>((s) => mongoDBContextConfiguration);
                    break;
                case ServiceLifetime.Transient:
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
            var constructor = typeof(TMongoDBContext)
                                .GetMethods()
                                .Where(m =>
                                {
                                    return m.MemberType is System.Reflection.MemberTypes.Constructor && m.GetParameters().Any(p => p.ParameterType is MongoDBContextConfiguration<TMongoDBContext>);
                                });

            if (constructor == null)
                return false;

            return constructor.Any();
        }
    }
}
