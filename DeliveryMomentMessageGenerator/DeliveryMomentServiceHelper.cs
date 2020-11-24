using MongoDB.Bson;
using MongoDB.Driver.Core.Operations;
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
            try
            {
                Console.WriteLine($"Start GetWharehouseGroupNumber for Store Number : {storeId} and delivererNumber {deliveryStream}");
                foreach (var logisticChannel in logisticChannels)
                {
                    if (deliveryStream.ToString() == logisticChannel.GetValue("deliveryStream").ToString())
                    {
                        try
                        {
                            var startDate = DateTimeUtilities.GetDate(logisticChannel.GetValue("startDate").ToString()).Date;
                            var endDate = DateTimeUtilities.GetEndDateForLogisticChannel(logisticChannel.GetValue("endDate").ToString());
                            if ((deliveryScheduleDate >= startDate) && (deliveryScheduleDate <= endDate))
                            {
                                wharehouseNumber = logisticChannel.GetValue("warehouseNumber").ToString();
                                return wharehouseNumber;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"GetWharehouseGroupNumber for Store Number : {storeId} and logisticChannel {logisticChannel.ToString()} with exception as {ex.Message}");
                        }
                    }
                }
                Console.WriteLine($"End GetWharehouseGroupNumber for Store Number : {storeId} and delivererNumber {deliveryStream}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetWharehouseGroupNumber for Store Number : {storeId} and delivererNumber {deliveryStream} with exception as {ex.Message}");
            }
            return wharehouseNumber;
        }
        public static List<BsonDocument> GetAllStores()
        {
            StoreRepository storeRepository = new StoreRepository();
            var storeDocuments = storeRepository.GetStores();
            return storeDocuments;
        }

        public static List<BsonDocument> GetAllDeliveryStreams()
        {
            DeliveryStreamRepository deliveryStreamRepository = new DeliveryStreamRepository();
            var deliveryStreams = deliveryStreamRepository.GetDeliveryStreams();
            return deliveryStreams;
        }

        public static string GetDeliveryStreamName(List<BsonDocument> deliveryStreams, int deliveryStreamNumber)
        {
            string deliveryStreamName = "HOUDBAAR";
            try
            {
                foreach (var deliveryStream in deliveryStreams)
                {
                    var deliveryStreamNo = Convert.ToInt32(deliveryStream.GetValue("deliveryStreamNumber", 1));
                    if (deliveryStreamNo == deliveryStreamNumber)
                    {
                        deliveryStreamName = deliveryStream.GetValue("deliveryStreamName", "HOUDBAAR").ToString();
                        return deliveryStreamName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                return deliveryStreamName;
        }

        public static string GetDelivererNumber(string storeId, List<BsonDocument> deliveryChannels, int deliveryStreamNumber, DateTime currentDate)
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
                        Console.WriteLine($"GetDelivererNumber exception for store {storeId} , and deliveryStreamNumber {deliveryStreamNumber.ToString()} with ex: {ex.Message} ");
                    }
                }

            }

            return string.Empty;
        }
    }
}
