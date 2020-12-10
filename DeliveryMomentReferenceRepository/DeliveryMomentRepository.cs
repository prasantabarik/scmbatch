using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository
{
   public class DeliveryMomentRepository
    {
        public void DeleteAllBatchDeliveryMoments()
        {
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://deliverymoment-cosmos-mongo-db:0LTdfL6WYtDKS4zINtXwkbUbt9UV4sZt0RA0KCqTgrn0qmXHWMQrgXmGQd7uYueYgClYYUaXjav8EzDCcty1xg==@deliverymoment-cosmos-mongo-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@deliverymoment-cosmos-mongo-db@");
                IMongoDatabase db = dbClient.GetDatabase("deliverymoment-db");     
                var deleteFilter = Builders<BsonDocument>.Filter.Eq("storeNumber", 7010);
                //delete  
                var deliveryMomentCollection = db.GetCollection<BsonDocument>("delivery-moment\u200b");
                deliveryMomentCollection.DeleteMany(deleteFilter);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        public List<BsonDocument> GetAllBatchDeliveryMoments()
        {
            List<BsonDocument> list = null;
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://deliverymoment-cosmos-mongo-db:0LTdfL6WYtDKS4zINtXwkbUbt9UV4sZt0RA0KCqTgrn0qmXHWMQrgXmGQd7uYueYgClYYUaXjav8EzDCcty1xg==@deliverymoment-cosmos-mongo-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@deliverymoment-cosmos-mongo-db@");
                IMongoDatabase db = dbClient.GetDatabase("deliverymoment-db");
                var collections = db.ListCollections().ToList();
                foreach (var collection in collections)
                {
                    Console.WriteLine(collection.ToString());
                }

                //  var filter = Builders<BsonDocument>.Filter.Eq("updatedBy", "DeliveryMomentBatch");
                var filter = Builders<BsonDocument>.Filter.Eq("storeNumber", 7010);
                //Get  
                var deliveryMomentCollection = db.GetCollection<BsonDocument>("delivery-moment\u200b");
                list = deliveryMomentCollection.Find(filter).ToList<BsonDocument>();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return list;

        }
    }
}
