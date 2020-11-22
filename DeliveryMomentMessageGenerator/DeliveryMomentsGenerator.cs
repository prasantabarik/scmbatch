using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DeliveryMomentsGenerator
    {
        public static void GenerateDeliveryMomentsByStore(BsonDocument storeDocument, List<DateTime> deliveryMomentBatchDates,
           List<BsonDocument> logisticChannels)
        {
            try
            {
                Console.WriteLine($"Start GenerateDeliveryMomentsByStore for Store Number : {storeDocument.ToString()}");
                DeliveryScheduleService deliveryScheduleService = new DeliveryScheduleService();
                string storeId = storeDocument.GetValue("storeId").ToString();
                var deliveryScheduleResult = deliveryScheduleService.GetDeliverySchedulesAsync(storeId);
                var deliveryChannels = DeliveryMomentServiceHelper.GetDeliveryChannels(storeId);
                var deliveryScheduleResponse = deliveryScheduleResult.Result;
                if (deliveryScheduleResponse != null && deliveryScheduleResponse.ResponseCode == "200" && deliveryScheduleResponse.Response != null && deliveryScheduleResponse.Response.Count > 0)
                {
                    var deliveryShedulesFinal = new List<DeliverySchedule>();
                    var deliverySchedules = deliveryScheduleResponse.Response;

                    GenerateDeliveryMomentsFromDeliverySchedules(storeId, deliveryMomentBatchDates, logisticChannels, deliverySchedules, deliveryChannels);
                }
                Console.WriteLine($"End GenerateDeliveryMomentsByStore for Store Number : {storeDocument.ToString()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GenerateDeliveryMomentsByStore for Store : {storeDocument.ToString()} and exception ex: {ex.Message}");
            }
        }

        private static void GenerateDeliveryMomentsFromDeliverySchedules(string storeId, List<DateTime> deliveryMomentBatchDates,
           List<BsonDocument> logisticChannels, List<DeliverySchedule> deliverySchedules, List<BsonDocument> deliveryChannels)
        {
            try
            {
                Console.WriteLine($"Start GenerateDeliveryMomentsFromDeliverySchedules for Store Number : {storeId}");
                foreach (var deliverySchedule in deliverySchedules)
                {
                    if (deliverySchedule.TimeTable != null && deliverySchedule.TimeTable.Count > 0)
                    {
                        GenerateDeliveryMomentsFromDeliverySchedule(storeId, deliveryMomentBatchDates, logisticChannels, deliverySchedule, deliveryChannels);
                    }
                    else
                    {
                        Console.WriteLine($"GenerateDeliveryMomentsFromDeliverySchedules with no time table  for Store Number : {storeId}");
                    }
                }
                Console.WriteLine($"End GenerateDeliveryMomentsFromDeliverySchedules for Store Number : {storeId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GenerateDeliveryMomentsFromDeliverySchedules for Store : {storeId} and exception ex: {ex.Message}");
            }
        }

        private static void GenerateDeliveryMomentsFromDeliverySchedule(string storeId, List<DateTime> deliveryMomentBatchDates,
         List<BsonDocument> logisticChannels, DeliverySchedule deliverySchedule, List<BsonDocument> deliveryChannels)
        {
            try
            {
                var startDate = Convert.ToDateTime(deliverySchedule.StartDate).Date;
                var endDate = DateTimeUtilities.GetEndDate(deliverySchedule.EndDate);

                Console.WriteLine($"Start GenerateDeliveryMomentsFromDeliverySchedule for Store Number : {storeId} with startdate {deliverySchedule.StartDate} and enddate {deliverySchedule.EndDate}");             

                foreach (var date in deliveryMomentBatchDates)
                {
                    if ((date >= startDate) && (date <= endDate))
                    {
                        GenerateDeliveryMomentsByDeliveryScheduleDates(storeId, logisticChannels, startDate, endDate, date, deliverySchedule, deliveryChannels);
                    }
                }
                Console.WriteLine($"End GenerateDeliveryMomentsFromDeliverySchedule for Store Number : {storeId} with startdate {deliverySchedule.StartDate} and enddate {deliverySchedule.EndDate}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GenerateDeliveryMomentsFromDeliverySchedule for Store Number : {storeId} with startdate {deliverySchedule.StartDate} and enddate {deliverySchedule.EndDate} with exception {ex.Message}");
            }
        }

        private static void GenerateDeliveryMomentsByDeliveryScheduleDates(string storeId, List<BsonDocument> logisticChannels, DateTime startDate, 
            DateTime endDate, DateTime date,  DeliverySchedule deliverySchedule, List<BsonDocument> deliveryChannels)
        {
            try
            {
                Console.WriteLine($"Start GenerateDeliveryMomentsByDeliveryScheduleDates for Store Number : {storeId} with startdate {startDate} and enddate {endDate}");
                // Pass the store, date and stream to DeliveryChannel collection. Repeat for every store, date and stream
                var delivererNumber = DeliveryMomentServiceHelper.GetDelivererNumber(storeId, deliveryChannels, deliverySchedule.DeliveryStreamNumber,
                    deliverySchedule.DeliverySchemaType, date, startDate, endDate);
              
               
                if (!string.IsNullOrEmpty(delivererNumber))
                {
                    //For the record, identify the warehouse number to prepare StoreOrder section of DeliveryMoment document. 
                    GenerateDeliveryMomentsByDeliverer(storeId, logisticChannels, date, deliverySchedule, delivererNumber);
                }

                Console.WriteLine($"End GenerateDeliveryMomentsByDeliveryScheduleDates for Store Number : {storeId} with startdate {startDate} and enddate {endDate}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GenerateDeliveryMomentsByDeliveryScheduleDates for Store Number : {storeId} with startdate {startDate} and enddate {endDate}. Exception is {ex.Message}");
            }
        }

        private static void GenerateDeliveryMomentsByDeliverer(string storeId, List<BsonDocument> logisticChannels,
             DateTime date, DeliverySchedule deliverySchedule, string delivererNumber)
        {

            try
            {
                Console.WriteLine($"Start GenerateDeliveryMomentsByDeliverer for Store Number : {storeId} with date {date} and delivererNumber {delivererNumber}");
                //For the record, identify the warehouse number to prepare StoreOrder section of DeliveryMoment document. 
                var wharehouseNumber = DeliveryMomentServiceHelper.GetWharehouseGroupNumber(storeId, logisticChannels, deliverySchedule.DeliveryStreamNumber, date);

                if (!string.IsNullOrEmpty(wharehouseNumber) && wharehouseNumber != "null")
                {
                    var deliveryMoments = DeliveryMomentsCreator.PrepareDeliveryMoments(storeId, deliverySchedule, date, delivererNumber, wharehouseNumber);
                    DeliveryMomentsMessageHandler.PublishMessages(deliveryMoments);
                }
                Console.WriteLine($"End GenerateDeliveryMomentsByDeliverer for Store Number : {storeId} with date {date} and delivererNumber {delivererNumber}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GenerateDeliveryMomentsByDeliverer for Store Number : {storeId} with date {date} and delivererNumber {delivererNumber}. Exception as {ex.Message}");
            }
        }
    }
}