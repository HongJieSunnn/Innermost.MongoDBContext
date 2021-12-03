using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.MongoDBContext.Configurations.Abstractions
{
    /// <summary>
    /// MongoDBContextConfiguration abstract class.
    /// This abstract class is used in MongoDBContextBase to make inject configuration more easily.
    /// </summary>
    public abstract class MongoDBContextConfiguration
    {
        public abstract Type ContextType { get; }
        /// <summary>
        /// MongoDB connectionString which is necessary.
        /// </summary>
        public virtual string ConnectionString { get; init; }
        /// <summary>
        /// MongoDB databaseName connect to.
        /// </summary>
        public virtual string DatabaseName { get; set; }
        /// <summary>
        /// DatabaseSettings.Default is null.
        /// </summary>
        public virtual MongoDatabaseSettings? DatabaseSettings { get; set; }
        /// <summary>
        /// CollectionSettings.Default is null.
        /// </summary>
        public virtual MongoCollectionSettings? CollectionSettings { get; set; }
    }
}
