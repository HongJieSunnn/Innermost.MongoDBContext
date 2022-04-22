using Innermost.MongoDBContext.Configurations.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext
{
    /// <summary>
    /// MongoDBContextBase is the base class to realize MongoDBContext.
    /// We can use it like using DbContext.
    /// </summary>
    public class MongoDBContextBase
    {
        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }
        protected MongoDBContextBase()
        {
            throw new NotSupportedException("Should use MongoDBConfiguration to construct MongoDBContext");
        }
        public MongoDBContextBase(MongoDBContextConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (this.GetType() == typeof(MongoDBContextBase)) throw new NotSupportedException("Do not construct a MongoDBContextBase instance.");
            if (this.GetType() != configuration.ContextType) throw new NotSupportedException($"Type TMongoDBContext(MongoDBContextConfiguration<TMongoDBContext>) must equeal to the MongoDBContext Type which define the Configuration.");

            Client = new MongoClient(configuration.ConnectionString);
            Database = Client.GetDatabase(configuration.DatabaseName, configuration.DatabaseSettings);

            //To get all IMongoCollection Properties and set them by IMongoDatabase.GetCollection() Method.
            var collections = this.GetType().GetProperties().Where(x => x.PropertyType.FullName!.StartsWith("MongoDB.Driver.IMongoCollection")).ToList();
            if (!collections.Any())
                throw new NotSupportedException("MongoDBContext you customed must have one IMongoCollection at least.");

            foreach (var collection in collections)
            {
                var collectionType = collection.PropertyType;
                var collectionDataType = collectionType.GenericTypeArguments[0];
                var collectionName = collectionDataType.Name;

                collection.SetValue(this, Database.GetType()?.GetMethod("GetCollection")?.MakeGenericMethod(collectionDataType).Invoke(Database, new object?[] { collectionName, configuration.CollectionSettings }));
            }
        }
    }
}
