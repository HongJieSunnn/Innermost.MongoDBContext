using Microsoft.VisualStudio.TestTools.UnitTesting;
using Innermost.MongoDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innermost.MongoDBContext.Configurations.Abstractions;
using Innermost.MongoDBContext.Configurations;
using Moq;
using Innermost.MongoDBContext.Tests.MongoDBContextImplementation;

namespace Innermost.MongoDBContext.Tests
{
    [TestClass()]
    public class MongoDBContextBaseTests
    {
        [TestMethod()]
        public void MongoDBContextBaseTest_configuration_null()
        {
            MongoDBContextConfiguration mockMongoDBContextConfiguration = null;

            Assert.ThrowsException<ArgumentNullException>(() => new MongoDBContextBase(mockMongoDBContextConfiguration));
        }

        [TestMethod()]
        public void MongoDBContextTest_construct_MongoDBContextBase_without_databaseSettings_and_collectionSettings()
        {
            const string MockConnectionString = "mongodb://localhost:27017";
            const string MockDatbaseName = "Innermost-MongoDBContext-Tests";
            var mockMongoDBContextConfiguration = new MongoDBContextConfiguration<MongoDBContextBase>(MockConnectionString,MockDatbaseName);

            Assert.ThrowsException<NotSupportedException>(
                () => new MongoDBContextBase(mockMongoDBContextConfiguration),
                "Do not construct a MongoDBContextBase instance.");
        }

        [TestMethod()]
        public void MongoDBContextTest_construct_MongoDBContextBase_with_other_type_configuration()
        {
            const string MockConnectionString = "mongodb://localhost:27017";
            const string MockDatbaseName = "Innermost-MongoDBContext-Tests";
            var mockMongoDBContextConfiguration = new MongoDBContextConfiguration<TestMongoDBContext>(MockConnectionString, MockDatbaseName);

            Assert.ThrowsException<NotSupportedException>(
                () => new MongoDBContextBase(mockMongoDBContextConfiguration),
                "Type TMongoDBContext(MongoDBContextConfiguration<TMongoDBContext>) must equeal to the MongoDBContext Type which define the Configuration.");
        }
    }
}