using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryMomentReferenceRepository
{
   public class DeliveryStreamRepository
    {
        public List<BsonDocument> GetDeliveryStreams()
        {
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://deliverymoment-cosmos-mongo-db:0LTdfL6WYtDKS4zINtXwkbUbt9UV4sZt0RA0KCqTgrn0qmXHWMQrgXmGQd7uYueYgClYYUaXjav8EzDCcty1xg==@deliverymoment-cosmos-mongo-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@deliverymoment-cosmos-mongo-db@");
                IMongoDatabase db = dbClient.GetDatabase("deliverymoment-db");


                //READ  
                var deliveryStreamCollection = db.GetCollection<BsonDocument>("delivery-stream");
                var deliveryStreamDocuments = deliveryStreamCollection.Find(new BsonDocument()).ToList();
                return deliveryStreamDocuments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
