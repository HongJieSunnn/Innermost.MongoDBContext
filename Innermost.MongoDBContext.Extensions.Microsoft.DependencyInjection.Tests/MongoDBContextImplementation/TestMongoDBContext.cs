using Innermost.MongoDBContext.Configurations;
using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.Tests.TestModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.Tests.MongoDBContextImplementation
{
    public class TestMongoDBContext:MongoDBContextBase
    {
        public IMongoCollection<TestModel1> TestModels1 { get; set; }
        public IMongoCollection<TestModel2> TestModels2 { get; set; }
        public TestMongoDBContext(MongoDBContextConfiguration<TestMongoDBContext> contextConfiguration):base(contextConfiguration)
        {

        }

        public TestMongoDBContext():base()
        {

        }
    }
}
