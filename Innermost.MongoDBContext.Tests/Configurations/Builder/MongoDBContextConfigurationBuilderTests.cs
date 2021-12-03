using Microsoft.VisualStudio.TestTools.UnitTesting;
using Innermost.MongoDBContext.Configurations.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innermost.MongoDBContext.Tests.MongoDBContextImplementation;

namespace Innermost.MongoDBContext.Configurations.Builder.Tests
{
    [TestClass()]
    public class MongoDBContextConfigurationBuilderTests
    {
        [TestMethod()]
        public void BuildTest_null_connectionString()
        {
            const string connectionString = null;
            const string databaseName = "Innermost.MongoDBContext.Tests";
            var configurationBuilder = new MongoDBContextConfigurationBuilder<TestMongoDBContext>();
            configurationBuilder
                .WithConnectionString(connectionString)
                .WithDatabase(databaseName);

            Assert.ThrowsException<ArgumentNullException>(() => configurationBuilder.Build());
        }

        [TestMethod()]
        public void BuildTest_null_databaseName()
        {
            const string connectionString = "mongodb://localhost:27017";
            const string databaseName = null;
            var configurationBuilder = new MongoDBContextConfigurationBuilder<TestMongoDBContext>();
            configurationBuilder
                .WithConnectionString(connectionString)
                .WithDatabase(databaseName);

            Assert.ThrowsException<ArgumentNullException>(() => configurationBuilder.Build());
        }

        [TestMethod]
        public void BuildTest_returns_configuration()
        {
            const string connectionString = "mongodb://localhost:27017";
            const string databaseName = "Innermost.MongoDBContext.Tests";
            var configurationBuilder = new MongoDBContextConfigurationBuilder<TestMongoDBContext>();
            configurationBuilder
                .WithConnectionString(connectionString)
                .WithDatabase(databaseName);

            var configuration = configurationBuilder.Build();

            Assert.IsNotNull(configuration);
            Assert.IsNotNull(configuration.ContextType);
            Assert.AreEqual(configuration.ContextType, typeof(TestMongoDBContext));
            Assert.IsNotNull(configuration.ConnectionString);
            Assert.IsNotNull(configuration.DatabaseName);
        }
    }
}