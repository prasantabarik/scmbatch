using System;
using System.Collections.Generic;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;
using MongoDB.Bson;
using System.Threading.Tasks.Sources;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.MomentMessageGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var deliveryMomentBatchDates = GetDeliveryMomentBatchDatesToRun();
          
           var storeDocuments = GetAllStores();

           DeliveryScheduleService deliveryScheduleService = new DeliveryScheduleService();

            foreach (var storeDocument in storeDocuments)
            {
                string storeId = storeDocument.GetValue("storeId").ToString();
               
                var deliveryScheduleResult = deliveryScheduleService.GetDeliverySchedulesAsync(storeId);

                var deliveryChannels = GetDeliveryChannels(storeId);

                var deliveryScheduleResponse = deliveryScheduleResult.Result;

                if (deliveryScheduleResponse != null && deliveryScheduleResponse.ResponseCode == "200" && deliveryScheduleResponse.Response != null && deliveryScheduleResponse.Response.Count > 0)
                {
                    var deliveryShedulesFinal = new List<DeliverySchedule>();
                    var deliverySchedules = deliveryScheduleResponse.Response;

                    foreach (var deliverySchedule in deliverySchedules)
                    {
                        
                        var startDate = Convert.ToDateTime(deliverySchedule.StartDate).Date;
                        var endDate = GetEndDate(deliverySchedule.EndDate);

                        foreach (var date in deliveryMomentBatchDates)
                        {
                            if ((date >= startDate) && (date <= endDate))
                            {
                                // Pass the store, date and stream to DeliveryChannel collection. Repeat for every store, date and stream
                                var delivererNumber = GetDelivererNumber(storeId, deliveryChannels, deliverySchedule.DeliveryStreamNumber,
                                    deliverySchedule.DeliverySchemaType, date, startDate, endDate);
                              
                            }
                        }
                    }

                }
                        //foreach (var date in deliveryMomentBatchDates)
                //{

                //    Console.WriteLine("Date is: " + date.ToString());
                //}
            }

            Console.ReadLine();
        }

        private static List<BsonDocument> GetDeliveryChannels(string storeId)
        {
            var deliveryChannelRepository = new DeliveryChannelRepository();
            var deliveryChannels = deliveryChannelRepository.GetDeliveryChannelsByStore(Convert.ToInt32(storeId));
            return deliveryChannels;
        }

        private static List<BsonDocument> GetAllStores()
        {
            StoreRepository storeRepository = new StoreRepository();
            var storeDocuments = storeRepository.GetStores();
            return storeDocuments;
        }

        private static string GetDelivererNumber(string storeId, List<BsonDocument> deliveryChannels, int deliveryStreamNumber, 
            int deliverySchemaType, DateTime currentDate, DateTime startDate, DateTime endDate)
        {
            foreach (var deliveryChannel in deliveryChannels)
            {
                Console.WriteLine($"DeliveryChannel :  {deliveryChannel.ToString()}");
                string delivererNumber = string.Empty;
                var newDeliveryStreamNumber =  Convert.ToInt32(deliveryChannel.GetValue("deliveryStream", 0));

                if (newDeliveryStreamNumber != 0 && newDeliveryStreamNumber == deliveryStreamNumber)
                {
                    string newStartDate = deliveryChannel.GetValue("startDate").ToString();
                    string newEndDate = deliveryChannel.GetValue("endDate").ToString();
                    DateTime newStartDateTime = Convert.ToDateTime(newStartDate);
                    DateTime newEndDateTime = GetEndDate(newEndDate);
                    if ((currentDate >= newStartDateTime) && (currentDate <= newEndDateTime))
                    {
                        delivererNumber = deliveryChannel.GetValue("delivererNumber").ToString();
                        return delivererNumber;
                    }
                }

            }

            return string.Empty;
        }

        private static DateTime GetEndDate(string endDatetime)
        {
            DateTime newEndDate = DateTime.Now;
            if (endDatetime == null || endDatetime == "null" || endDatetime == "BsonNull")
            {
                newEndDate = DateTime.Now.AddDays(42).Date;
            }
            else
            {
                newEndDate = Convert.ToDateTime(endDatetime).Date;
            }
            return newEndDate;
        }
        private static List<DateTime> GetDeliveryMomentBatchDatesToRun()
        {
            var deliveryMomentBatchDatesList = new List<DateTime>();
            for (int iIndex = 2; iIndex <= 42; iIndex++)
            {
                var deliveryMomentDate = DateTime.Now.AddDays(iIndex).Date;
                deliveryMomentBatchDatesList.Add(deliveryMomentDate);
            }
          
            return deliveryMomentBatchDatesList;
        }
    }
}
