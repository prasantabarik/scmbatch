using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DeliveryMomentServiceHelper
    {
        public static List<BsonDocument> GetDeliveryChannels(string storeId)
        {
            var deliveryChannelRepository = new DeliveryChannelRepository();
            var deliveryChannels = deliveryChannelRepository.GetDeliveryChannelsByStore(Convert.ToInt32(storeId));
            return deliveryChannels;
        }

        public static List<BsonDocument> GetLogisticChannels(string storeId)
        {
            var logicsticChannelRepository = new LogisticChannelRepository();
            var logicsticChannels = logicsticChannelRepository.GetLogisticChannelsByStore(Convert.ToInt32(storeId));
            return logicsticChannels;
        }

        public static string GetWharehouseGroupNumber(string storeId, List<BsonDocument> logisticChannels, int deliveryStream,
            DateTime deliveryScheduleDate)
        {
            string wharehouseNumber = string.Empty;
            foreach (var logisticChannel in logisticChannels)
            {
                if (deliveryStream.ToString() == logisticChannel.GetValue("deliveryStreamNumber").ToString())
                {
                    var startDate = Convert.ToDateTime(logisticChannel.GetValue("startDate").ToString()).Date;
                    var endDate = DateTimeUtilities.GetEndDate(logisticChannel.GetValue("endDate").ToString());
                    if ((deliveryScheduleDate >= startDate) && (deliveryScheduleDate <= endDate))
                    {
                        wharehouseNumber = logisticChannel.GetValue("warehouseNumber").ToString();
                        return wharehouseNumber;
                    }
                }
            }
            return wharehouseNumber;
        }
        public static List<BsonDocument> GetAllStores()
        {
            StoreRepository storeRepository = new StoreRepository();
            var storeDocuments = storeRepository.GetStores();
            return storeDocuments;
        }

        public static string GetDelivererNumber(string storeId, List<BsonDocument> deliveryChannels, int deliveryStreamNumber,
            int deliverySchemaType, DateTime currentDate, DateTime startDate, DateTime endDate)
        {
            foreach (var deliveryChannel in deliveryChannels)
            {
                Console.WriteLine($"DeliveryChannel :  {deliveryChannel.ToString()}");
                string delivererNumber = string.Empty;
                var newDeliveryStreamNumber = Convert.ToInt32(deliveryChannel.GetValue("deliveryStream", 0));

                if (newDeliveryStreamNumber != 0 && newDeliveryStreamNumber == deliveryStreamNumber)
                {
                    try
                    {
                        string newStartDate = deliveryChannel.GetValue("startDate").ToString();
                        string newEndDate = deliveryChannel.GetValue("endDate").ToString();
                        DateTime newStartDateTime = Convert.ToDateTime(newStartDate);
                        DateTime newEndDateTime = DateTimeUtilities.GetEndDate(newEndDate);
                        if ((currentDate >= newStartDateTime) && (currentDate <= newEndDateTime))
                        {
                            delivererNumber = deliveryChannel.GetValue("delivererNumber").ToString();
                            return delivererNumber;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            return string.Empty;
        }
    }
}
