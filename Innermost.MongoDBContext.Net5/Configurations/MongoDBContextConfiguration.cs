using Innermost.MongoDBContext.Configurations.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Configurations
{
    /// <summary>
    /// Configuration to confige custom MongoDBContext.
    /// </summary>
    /// <typeparam name="TMongoDBContext">Custom MongoDBContext which inherit MongoDBContextBase</typeparam>
    public class MongoDBContextConfiguration<TMongoDBContext> : MongoDBContextConfiguration where TMongoDBContext : MongoDBContextBase
    {
        /// <summary>
        /// CustomMongoDBContext type.Which is typeof(TMongoDBContext).
        /// </summary>
        public override Type ContextType => typeof(TMongoDBContext);

        protected MongoDBContextConfiguration()
        {

        }

        public MongoDBContextConfiguration(string connectionString, string databaseName, MongoDatabaseSettings? databaseSettings = null, MongoCollectionSettings? collectionSettings = null)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            DatabaseSettings = databaseSettings;
            CollectionSettings = collectionSettings;
        }

        public MongoDBContextConfiguration(string connectionString, string databaseName, Action<MongoDBContextConfiguration<TMongoDBContext>> options)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            options(this);
        }
    }
}
