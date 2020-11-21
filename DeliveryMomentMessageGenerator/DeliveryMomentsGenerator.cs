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

            DeliveryScheduleService deliveryScheduleService = new DeliveryScheduleService();

            string storeId = storeDocument.GetValue("storeId").ToString();

            var deliveryScheduleResult = deliveryScheduleService.GetDeliverySchedulesAsync(storeId);

            var deliveryChannels = DeliveryMomentServiceHelper.GetDeliveryChannels(storeId);

            var deliveryScheduleResponse = deliveryScheduleResult.Result;

            if (deliveryScheduleResponse != null && deliveryScheduleResponse.ResponseCode == "200" && deliveryScheduleResponse.Response != null && deliveryScheduleResponse.Response.Count > 0)
            {
                var deliveryShedulesFinal = new List<DeliverySchedule>();
                var deliverySchedules = deliveryScheduleResponse.Response;

                foreach (var deliverySchedule in deliverySchedules)
                {

                    var startDate = Convert.ToDateTime(deliverySchedule.StartDate).Date;
                    var endDate = DateTimeUtilities.GetEndDate(deliverySchedule.EndDate);

                    foreach (var date in deliveryMomentBatchDates)
                    {
                        if ((date >= startDate) && (date <= endDate))
                        {
                            // Pass the store, date and stream to DeliveryChannel collection. Repeat for every store, date and stream
                            var delivererNumber = DeliveryMomentServiceHelper.GetDelivererNumber(storeId, deliveryChannels, deliverySchedule.DeliveryStreamNumber,
                                deliverySchedule.DeliverySchemaType, date, startDate, endDate);

                            if (!string.IsNullOrEmpty(delivererNumber))
                            {
                                //For the record, identify the warehouse number to prepare StoreOrder section of DeliveryMoment document. 
                                var wharehouseNumber = DeliveryMomentServiceHelper.GetWharehouseGroupNumber(storeId, logisticChannels, deliverySchedule.DeliveryStreamNumber, date);
                                if (string.IsNullOrEmpty(wharehouseNumber) && wharehouseNumber == "null")
                                {
                                    var deliveryMoments = DeliveryMomentsCreator.PrepareDeliveryMoments(storeId, deliverySchedule, date, delivererNumber, wharehouseNumber);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}
