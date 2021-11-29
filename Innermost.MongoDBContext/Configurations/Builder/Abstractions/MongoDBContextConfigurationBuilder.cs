using Innermost.MongoDBContext.Configurations.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Configurations.Builder.Abstractions
{
    public abstract class MongoDBContextConfigurationBuilder
    {
        protected string _connectionString;
        protected string _databaseName;
        protected MongoDatabaseSettings? _databaseSettings;
        protected MongoCollectionSettings? _collectionSettings;

        public MongoDBContextConfigurationBuilder()
        {

        }

        public virtual MongoDBContextConfigurationBuilder WithConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public virtual MongoDBContextConfigurationBuilder WithDatabase(string databaseName)
        {
            _databaseName = databaseName;
            return this;
        }

        public virtual MongoDBContextConfigurationBuilder WithDatabaseSettings(MongoDatabaseSettings mongoDatabaseSettings)
        {
            _databaseSettings = mongoDatabaseSettings;
            return this;
        }

        public virtual MongoDBContextConfigurationBuilder WithCollectionSettings(MongoCollectionSettings mongoCollectionSettings)
        {
            _collectionSettings = mongoCollectionSettings;
            return this;
        }

        public abstract MongoDBContextConfiguration Build();
    }
}
