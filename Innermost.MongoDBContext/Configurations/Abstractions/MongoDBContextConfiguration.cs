using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Configurations.Abstractions
{
    public abstract class MongoDBContextConfiguration
    {
        public abstract Type ContextType { get; }
        public virtual string ConnectionString { get; init; }
        public virtual string DatabaseName { get; set; }
        public virtual MongoDatabaseSettings? DatabaseSettings { get; set; }
        public virtual MongoCollectionSettings? CollectionSettings { get; set; }
    }
}
