using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Innermost.MongoDBContext.Configurations.Builder;
using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.Tests.MongoDBContextImplementation;
using Innermost.MongoDBContext.Configurations;

namespace Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.Tests
{
    [TestClass()]
    public class IServiceCollectionExtensionsTests
    {
        [TestMethod()]
        public void AddMongoDBContextTest_has_constructor_with_configuration_scoped()
        {
            const string MockConnectionString = "mongodb://localhost:27017";
            const string MockDatbaseName = "Innermost-MongoDBContext-Tests";
            Action<MongoDBContextConfigurationBuilder<TestMongoDBContext>> builder = c =>
            {
                c.WithConnectionString(MockConnectionString);
                c.WithDatabase(MockDatbaseName);
            };
            IServiceCollection services = new ServiceCollection();
            var mongoDBContextConfigurationBuilder = new MongoDBContextConfigurationBuilder<TestMongoDBContext>();
            builder(mongoDBContextConfigurationBuilder);
            var mongoDBContextConfiguration = mongoDBContextConfigurationBuilder.Build();

            services.AddMongoDBContext(builder,ServiceLifetime.Transient,ServiceLifetime.Transient);
            var provider = services.BuildServiceProvider();
            var count = services.Count;
            MongoDBContextConfiguration<TestMongoDBContext>? configuration = null;
            TestMongoDBContext? dbContextInstance = null;
            MongoDBContextConfiguration<TestMongoDBContext>? scopedConfiguration = null;
            TestMongoDBContext? scopedDBContextInstance = null;

            using (var scopedProvider = provider.CreateScope())
            {
                configuration = scopedProvider.ServiceProvider.GetService<MongoDBContextConfiguration<TestMongoDBContext>>();
                dbContextInstance = scopedProvider.ServiceProvider.GetService<TestMongoDBContext>();
                using (var nestScopedProvider=scopedProvider.ServiceProvider.CreateScope())
                {
                    scopedConfiguration = nestScopedProvider.ServiceProvider.GetService<MongoDBContextConfiguration<TestMongoDBContext>>();
                    scopedDBContextInstance = nestScopedProvider.ServiceProvider.GetService<TestMongoDBContext>();
                }
            }

            Assert.AreNotEqual(count,0);
            Assert.IsNotNull(provider);
            Assert.IsNotNull(configuration);
            Assert.IsNotNull(dbContextInstance);
            Assert.IsNotNull(scopedConfiguration);
            Assert.IsNotNull(scopedDBContextInstance);
            Assert.AreSame(scopedConfiguration, configuration);
            Assert.AreNotSame(scopedDBContextInstance, dbContextInstance);
        }

        [TestMethod()]
        public void AddMongoDBContextTest_has_constructor_with_configuration_singleton()
        {
            const string MockConnectionString = "mongodb://localhost:27017";
            const string MockDatbaseName = "Innermost-MongoDBContext-Tests";
            Action<MongoDBContextConfigurationBuilder<TestMongoDBContext>> builder = c =>
            {
                c.WithConnectionString(MockConnectionString);
                c.WithDatabase(MockDatbaseName);
            };
            IServiceCollection services = new ServiceCollection();
            var mongoDBContextConfigurationBuilder = new MongoDBContextConfigurationBuilder<TestMongoDBContext>();
            builder(mongoDBContextConfigurationBuilder);
            var mongoDBContextConfiguration = mongoDBContextConfigurationBuilder.Build();

            services.AddMongoDBContext(builder,ServiceLifetime.Singleton,ServiceLifetime.Singleton);
            var provider = services.BuildServiceProvider();
            var count = services.Count;
            var configuration = provider.GetService<MongoDBContextConfiguration<TestMongoDBContext>>();
            var dbcontextInstance = provider.GetService<TestMongoDBContext>();
            MongoDBContextConfiguration<TestMongoDBContext>? scopedConfiguration = null;
            TestMongoDBContext? scopedDBContextInstance = null;
            using (var scopedProvider = provider.CreateScope())
            {
                scopedConfiguration = scopedProvider.ServiceProvider.GetService<MongoDBContextConfiguration<TestMongoDBContext>>();
                scopedDBContextInstance = scopedProvider.ServiceProvider.GetService<TestMongoDBContext>();
            }

            Assert.AreNotEqual(count, 0);
            Assert.IsNotNull(provider);
            Assert.IsNotNull(configuration);
            Assert.IsNotNull(dbcontextInstance);
            Assert.IsNotNull(scopedConfiguration);
            Assert.IsNotNull(scopedDBContextInstance);
            Assert.AreEqual(scopedConfiguration, configuration);
            Assert.AreEqual(scopedDBContextInstance, dbcontextInstance);
        }
    }
}