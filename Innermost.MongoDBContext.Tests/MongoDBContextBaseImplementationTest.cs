using Innermost.MongoDBContext.Configurations;
using Innermost.MongoDBContext.Tests.MongoDBContextImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Tests
{
    [TestClass]
    public class MongoDBContextBaseImplementationTest
    {
        [TestMethod]
        public void MongoDBContextBaseImplementationTest_construct_TestMongoDBContext_by_MongoDBContextBase()
        {
            const string MockConnectionString = "mongodb://localhost:27017";
            const string MockDatbaseName = "Innermost-MongoDBContext-Tests";
            var mockMongoDBContextConfiguration = new MongoDBContextConfiguration<TestMongoDBContext>(MockConnectionString, MockDatbaseName,null,null);

            Assert.ThrowsException<NotSupportedException>(() => (TestMongoDBContext)new MongoDBContextBase(mockMongoDBContextConfiguration), "Do not construct a MongoDBContextBase instance.");
        }

        [TestMethod]
        public void MongoDBContextBaseImplementationTest_construct_TestMongoDBContext_by_TestMongoDBContext()
        {
            const string MockConnectionString = "mongodb://localhost:27017";
            const string MockDatbaseName = "Innermost-MongoDBContext-Tests";
            var mockMongoDBContextConfiguration = new MongoDBContextConfiguration<TestMongoDBContext>(MockConnectionString, MockDatbaseName,null, null);

            var mockMongoDBContext = new TestMongoDBContext(mockMongoDBContextConfiguration);

            Assert.IsNotNull(mockMongoDBContext.TestModels1);
            Assert.IsNotNull(mockMongoDBContext.TestModels2);
        }

        [TestMethod]
        public void MongoDBContextBaseImplementationTest_construct_TestMongoDBContext_by_empty_params_MongoDBContextBase_constructor()
        {
            Assert.ThrowsException<NotSupportedException>(() => new TestMongoDBContext());
        }
    }
}
