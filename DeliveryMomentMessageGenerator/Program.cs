using System;
using System.Collections.Generic;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;
using MongoDB.Bson;
using System.Threading.Tasks.Sources;
using MongoDB.Driver.Core.Operations;
using System.Linq.Expressions;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var deliveryMomentBatchDates = GetDeliveryMomentBatchDatesToRun();

            var storeDocuments = GetAllStores();
            foreach (var storeDocument in storeDocuments)
            {
                Console.WriteLine($"DeliveryMomentMessageGenerator for Store Number : {storeDocument.ToString()}");
                string storeId = storeDocument.GetValue("storeId").ToString();
                var logisticChannels = GetLogisticChannels(storeId);
                GenerateDeliverySchedulesByStore(storeDocument, deliveryMomentBatchDates, logisticChannels);
            }

            Console.ReadLine();
        }

        private static void GenerateDeliverySchedulesByStore(BsonDocument storeDocument, List<DateTime> deliveryMomentBatchDates, 
            List<BsonDocument> logisticChannels)
        {
            DeliveryScheduleService deliveryScheduleService = new DeliveryScheduleService();

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

                            if (!string.IsNullOrEmpty(delivererNumber))
                            {
                                //For the record, identify the warehouse number to prepare StoreOrder section of DeliveryMoment document. 
                                var wharehouseNumber = GetWharehouseGroupNumber(storeId, logisticChannels, deliverySchedule.DeliveryStreamNumber, date);
                                if (string.IsNullOrEmpty(wharehouseNumber) && wharehouseNumber == "null")
                                {
                                  var deliveryMoments =  PrepareDeliveryMoments(storeId, deliverySchedule, date, delivererNumber, wharehouseNumber);
                                }
                            }
                            }
                        }
                    }

                }
            }
        
        private static List<BsonDocument> GetDeliveryChannels(string storeId)
        {
            var deliveryChannelRepository = new DeliveryChannelRepository();
            var deliveryChannels = deliveryChannelRepository.GetDeliveryChannelsByStore(Convert.ToInt32(storeId));
            return deliveryChannels;
        }

        private static List<BsonDocument> GetLogisticChannels(string storeId)
        {
            var logicsticChannelRepository = new LogisticChannelRepository();
            var logicsticChannels = logicsticChannelRepository.GetLogisticChannelsByStore(Convert.ToInt32(storeId));
            return logicsticChannels;
        }

        private static string GetWharehouseGroupNumber(string storeId, List<BsonDocument> logisticChannels, int deliveryStream, 
            DateTime deliveryScheduleDate) 
        {
            string wharehouseNumber = string.Empty;
            foreach (var logisticChannel in logisticChannels)
            {
                if (deliveryStream.ToString() == logisticChannel.GetValue("deliveryStreamNumber").ToString())
                {
                    var startDate = Convert.ToDateTime(logisticChannel.GetValue("startDate").ToString()).Date;
                    var endDate = GetEndDate(logisticChannel.GetValue("endDate").ToString());
                    if ((deliveryScheduleDate >= startDate) && (deliveryScheduleDate <= endDate))
                    {
                        wharehouseNumber = logisticChannel.GetValue("warehouseNumber").ToString();
                        return wharehouseNumber;
                    }
                }
            }
            return wharehouseNumber;
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
                var newDeliveryStreamNumber = Convert.ToInt32(deliveryChannel.GetValue("deliveryStream", 0));

                if (newDeliveryStreamNumber != 0 && newDeliveryStreamNumber == deliveryStreamNumber)
                {
                    try
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
                    catch (Exception ex)
                    {

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

        private static List<DeliveryMomentModel> PrepareDeliveryMoments(string storeNumber, DeliverySchedule deliverySchedule, 
            DateTime deliveryDateTime, string delivererNumber, string wharehouseNumber)
        {
            List<DeliveryMomentModel> deliveryMoments = new List<DeliveryMomentModel>();
            var timeTables = deliverySchedule.TimeTable;
            if (timeTables != null && timeTables.Count > 0)
            {
                foreach (var timeTable in timeTables)
                {
                    var deliveryDay = DayOfTheWeek(timeTable.DeliveryDay);
                    if (deliveryDateTime.DayOfWeek.ToString() == deliveryDay)
                    {
                        TimeSpan onlyDeliveryTime = DateTime.Parse(timeTable.DeliveryTime).TimeOfDay;
                        TimeSpan onlyOrderTime = DateTime.Parse(timeTable.OrderTime).TimeOfDay;
                        TimeSpan onlyFillTime = DateTime.Parse(timeTable.FillTime).TimeOfDay;
                        TimeSpan onlyStartFillTime = DateTime.Parse(timeTable.StartTime).TimeOfDay;

                        var deliveryMoment = new DeliveryMomentModel();
                        deliveryMoment.StoreNumber = Convert.ToInt32(storeNumber);
                        deliveryMoment.StreamNumber = deliverySchedule.DeliveryStreamNumber;

                        deliveryMoment.DeliveryDateTime = deliveryDateTime.Add(onlyDeliveryTime);
                        
                        deliveryMoment.BoxSzie = timeTable.BoxSize;
                        deliveryMoment.DelivererNumber = Convert.ToInt32(delivererNumber);

                        var orderDateTime = deliveryDateTime;
                        orderDateTime = GetNextDateTimeFromDayCode(orderDateTime, timeTable.OrderDay).AddDays(-7);                               
                        
                        deliveryMoment.OrderDateTime = orderDateTime.Add(onlyOrderTime);
                       
                        var startFillDateTime = deliveryDateTime;
                        startFillDateTime = GetNextDateTimeFromDayCode(startFillDateTime, timeTable.StartDay).AddDays(-7);
                        
                        deliveryMoment.StartFillDateTime = startFillDateTime.Add(onlyStartFillTime);

                        var fillDateTime = deliveryDateTime;
                        fillDateTime = GetNextDateTimeFromDayCode(fillDateTime, timeTable.FillDay);
                        deliveryMoment.FillDateTime = fillDateTime.Add(onlyFillTime);

                        deliveryMoment.InitialPromotionFlag = timeTable.InitialDeliveryFlag;
                        deliveryMoment.OrderStatus = null;
                        deliveryMoment.TotalInitialOrderQuantity = null;
                        deliveryMoment.TotalOrderQuantity = null;
                        deliveryMoment.MainDeliveryFlag = timeTable.MainDeliveryFlag;

                        //Todo: Not present in timetable. Check with Vanchi
                        deliveryMoment.StoreAdviseFlag = timeTable.StoreAdviseFlag;
                        deliveryMoment.DeliveryScheamaType = 1;

                        //Todo: Check with Vanchi on StoreOrders multiple of array or single. If array how do we get it?
                        StoreOrder storeOrder = new StoreOrder();
                        storeOrder.WarehouseNumber = Convert.ToInt32(wharehouseNumber);
                        storeOrder.OrderNumber = GenerateRandomNumber();

                        deliveryMoment.StoreOrders = new List<StoreOrder>();
                        deliveryMoment.StoreOrders.Add(storeOrder);
                        deliveryMoments.Add(deliveryMoment);

                        if (timeTable.Exclusion != null && timeTable.Exclusion.Count > 0)
                        {
                            foreach (var exclusion in timeTable.Exclusion)
                            {
                                deliveryMoment.LogisticGroupExclusion.Add(exclusion.LogisticGroupNumber);
                            }
                        }
                    }
                }
            }

            return deliveryMoments;

        }

        private static DateTime GetNextDateTimeFromDayCode(DateTime dateTime, string weekCode)
        {
            DayOfWeek dayName = DayOfTheWeekName(weekCode);

            int daysToAdd = ((int) dayName - (int)dateTime.DayOfWeek + 7) % 7;
            return dateTime.AddDays(daysToAdd);
        }
        private static string DayOfTheWeek(string weekCode)
        {
            string weekName = string.Empty;
            switch (weekCode)
            {
                case "MA":
                        weekName = "Monday";
                        break;
                case "DI":
                    weekName = "Tuesday";
                    break;
                case "WO":
                    weekName = "Wednesday";
                    break;
                case "DO":
                    weekName = "Thursday";
                    break;
                case "VR":
                    weekName = "Friday";
                    break;
                case "ZA":
                    weekName = "Saturday";
                    break;
                case "ZO":
                    weekName = "Sunday";
                    break;
                default:
                    break;
            }
            return weekName;
        }

        private static DayOfWeek DayOfTheWeekName(string weekCode)
        {
            DayOfWeek weekName = DayOfWeek.Sunday;
            switch (weekCode)
            {
                case "MA":
                    weekName = DayOfWeek.Monday;
                    break;
                case "DI":
                    weekName = DayOfWeek.Tuesday;
                    break;
                case "WO":
                    weekName = DayOfWeek.Wednesday;
                    break;
                case "DO":
                    weekName = DayOfWeek.Thursday;
                    break;
                case "VR":
                    weekName = DayOfWeek.Friday;
                    break;
                case "ZA":
                    weekName = DayOfWeek.Saturday;
                    break;
                case "ZO":
                    weekName = DayOfWeek.Sunday;
                    break;
                default:
                    break;
            }
            return weekName;
        }

        private static int GenerateRandomNumber()
        {
            Random random = new Random();
            int num = random.Next(1000, int.MaxValue);
            return num;
        }
    }
}

