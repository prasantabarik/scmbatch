using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository
{
    public class LogisticChannelRepository
    {
        public List<BsonDocument> GetLogisticChannelsByStore(int storeId)
        {
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://deliverymoment-cosmos-mongo-db:0LTdfL6WYtDKS4zINtXwkbUbt9UV4sZt0RA0KCqTgrn0qmXHWMQrgXmGQd7uYueYgClYYUaXjav8EzDCcty1xg==@deliverymoment-cosmos-mongo-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@deliverymoment-cosmos-mongo-db@");
                IMongoDatabase db = dbClient.GetDatabase("deliverymoment-db");
  
                var logisticChannelsCollection = db.GetCollection<BsonDocument>("logistic-channel");

                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("storeNumber", storeId);
                var logisticChannels = logisticChannelsCollection.Find(filter).ToList();

                return logisticChannels;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
