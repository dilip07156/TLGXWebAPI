using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DistributionWebApi.Mongo
{
    public static class MongoDBHandler
    {
        static IMongoClient _client;
        static IMongoDatabase _database;
        static string MongoDBConnectionString;

        static IMongoClient mClientConnection()
        {
            MongoDBConnectionString = System.Configuration.ConfigurationManager.AppSettings["MongoDBConnectionString"];
            _client = new MongoClient(MongoDBConnectionString);
            return _client;
        }

        public static IMongoDatabase mDatabase()
        {
            if (_client == null)
            {
                _client = mClientConnection();
            }

            if (_database == null)
            {
                var mongoUrl = new MongoUrl(MongoDBConnectionString);
                _database = _client.GetDatabase(mongoUrl.DatabaseName);
            }

            return _database;

        }
    }
}

