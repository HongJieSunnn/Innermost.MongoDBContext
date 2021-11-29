using Innermost.MongoDBContext.Configurations.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext
{
    public class MongoDBContext
    {
        protected MongoDBContext()
        {
            throw new NotSupportedException("Should use MongoDBConfiguration to construct MongoDBContext");
        }
        public MongoDBContext(MongoDBContextConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var client = new MongoClient(configuration.ConnectionString);
            var database = client.GetDatabase(configuration.DatabaseName, configuration.DatabaseSettings);

            //To get all IMongoCollection Properties and set them by IMongoDatabase.GetCollection() Method.
            var collections = configuration.ContextType.GetProperties().Where(x => x.PropertyType.Name.StartsWith("IMongoCollection")).ToList();
            foreach (var collection in collections)
            {
                var collectionType = collection.GetType();
                var collectionDataType = collectionType.GenericTypeArguments[0];
                var collectionName = collectionType.Name;

                collection.SetValue(this, database.GetType()?.GetMethod("GetCollection")?.MakeGenericMethod(collectionType).Invoke(collectionName, new object?[] { configuration.CollectionSettings }));
            }
        }
    }
}
