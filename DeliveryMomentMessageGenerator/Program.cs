using System;
using System.Collections.Generic;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.MomentMessageGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var deliveryMomentBatchDates = GetDeliveryMomentBatchDatesToRun();
            StoreRepository storeRepository = new StoreRepository();
           var storeDocuments = storeRepository.GetStores();

            DeliveryScheduleService deliveryScheduleService = new DeliveryScheduleService();
            foreach (var storeDocument in storeDocuments)
            {
                string storeId = storeDocument.GetValue("storeId").ToString();
               
                var deliveryScheduleResult = deliveryScheduleService.GetDeliverySchedulesAsync(storeId);

                var deliveryChannelRepository = new DeliveryChannelRepository();
                var deliveryChannels = deliveryChannelRepository.GetDeliveryChannelsByStore(Convert.ToInt32(storeId));

                foreach (var deliveryChannel in deliveryChannels)
                {
                    Console.WriteLine(deliveryChannel.ToString());
                }
                var deliveryScheduleResponse = deliveryScheduleResult.Result;

                if (deliveryScheduleResponse != null && deliveryScheduleResponse.ResponseCode == "200" && deliveryScheduleResponse.Response != null && deliveryScheduleResponse.Response.Count > 0)
                {
                    var deliveryShedulesFinal = new List<DeliverySchedule>();
                    var deliverySchedules = deliveryScheduleResponse.Response;

                    foreach (var deliverySchedule in deliverySchedules)
                    {
                        var endDate = DateTime.Now.Date;
                        var startDate = Convert.ToDateTime(deliverySchedule.StartDate).Date;
                        if (deliverySchedule.EndDate == null || deliverySchedule.EndDate == "null")
                        {
                            endDate = endDate.AddDays(42);
                        }
                        else
                        {
                           endDate = Convert.ToDateTime(deliverySchedule.EndDate).Date;
                        }

                        foreach (var date in deliveryMomentBatchDates)
                        {
                            if ((date >= startDate) && (date <= endDate))
                            {
                               // Pass the store, date and stream to DeliveryChannel collection. Repeat for every store, date and stream
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

        public static void GetDeliverInfo()
        {
            
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
