using Innermost.MongoDBContext.Configurations.Abstractions;
using Innermost.MongoDBContext.Configurations.Builder.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Configurations.Builder
{
    public class MongoDBContextConfigurationBuilder<TMongoDBContext> : MongoDBContextConfigurationBuilder
        where TMongoDBContext : MongoDBContextBase
    {
        public override MongoDBContextConfiguration<TMongoDBContext> Build()
        {
            if (_connectionString == null)
                throw new ArgumentNullException("ConnectionString can not be null.");
            if (_databaseName == null)
                throw new ArgumentNullException("DatabaseName can not be null.");

            return new MongoDBContextConfiguration<TMongoDBContext>(_connectionString, _databaseName, _databaseSettings, _collectionSettings);
        }
    }
}
