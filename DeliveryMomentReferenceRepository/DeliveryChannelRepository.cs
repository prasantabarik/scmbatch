using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository
{
   public class DeliveryChannelRepository
    {
        public List<BsonDocument> GetDeliveryChannelsByStore(int storeId)
        {
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://deliverymoment-cosmos-mongo-db:0LTdfL6WYtDKS4zINtXwkbUbt9UV4sZt0RA0KCqTgrn0qmXHWMQrgXmGQd7uYueYgClYYUaXjav8EzDCcty1xg==@deliverymoment-cosmos-mongo-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@deliverymoment-cosmos-mongo-db@");
                IMongoDatabase db = dbClient.GetDatabase("deliverymoment-db");


                //READ  
                var deliveryChannelsCollection = db.GetCollection<BsonDocument>("delivery-channel");
              
                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("storeNumber", storeId);
           
               //var filter = builder.And(builder.Eq("storeNumber", 7002), builder.Eq("deliveryStream", 1));

                var deliveryChannels = deliveryChannelsCollection.Find(filter).ToList();
               
                return deliveryChannels;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
