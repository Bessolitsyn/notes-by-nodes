using MongoDB.Driver;

namespace MongoDBStorage
{
    public class MongoStorage
    {
        MongoClient _client;
        IMongoDatabase _mongoDatabase;
        MongoStorage()
        {            
            _client = InitClient();
        }
        public static MongoClient InitClient()
        {
            var connectionString = "mongodb://localhost:27017";
            return new MongoClient(connectionString);
        }
    }
}
