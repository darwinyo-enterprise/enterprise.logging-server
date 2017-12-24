using Enterprise.Enums.NetStandard;
using Enterprise.Extension.NetStandard;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.DataLayers.Mongo
{
    public class LogMongoContext
    {
        //private static readonly MongoCredential Credentials = MongoCredential.CreateCredential("admin", "root", "*****");
        private readonly MongoServerAddress _mongoServerAddress;
        private readonly MongoClientSettings _mongoClientSettings;

        // This is ok... Normally, they would be put into
        // an IoC container.
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public LogMongoContext(string connectionString)
        {
            _mongoServerAddress = new MongoServerAddress(connectionString);

            _mongoClientSettings = new MongoClientSettings
            {
                //Credentials = new[] { Credentials },
                Server = _mongoServerAddress
            };
            _client = new MongoClient(_mongoClientSettings);
            _database = _client.GetDatabase(MongoDatabaseEnum.Logging.GetDescription());
        }
        public IMongoClient Client => _client;

        public IMongoCollection<ErrorLog> ErrorLogCollection => _database.GetCollection<ErrorLog>(MongoLoggingCollectionEnum.ErrorLog.GetDescription());
        public IMongoCollection<AuditLog> AuditLogCollection => _database.GetCollection<AuditLog>(MongoLoggingCollectionEnum.AuditLog.GetDescription());
        public IMongoCollection<DebugLog> DebugLogCollection => _database.GetCollection<DebugLog>(MongoLoggingCollectionEnum.DebugLog.GetDescription());
    }
}
