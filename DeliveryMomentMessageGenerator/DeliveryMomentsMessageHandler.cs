using System;
using System.Collections.Generic;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler;
using System.Threading.Tasks;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.CommonUtilities;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DeliveryMomentsMessageHandler
    {
        public static void PublishMessages(List<DeliveryMomentModel> deliveryMoments)
        {
            if (deliveryMoments != null && deliveryMoments.Count > 0)
            {
                foreach (var deliveryMoment in deliveryMoments)
                {
                    try
                    {
                        var deliveryMomentAsString = DeliveryMomentSerializer.Serialize(deliveryMoment);
                        IDeliveryMomentMessageHandler deliveryMomentMessageHandler = new DeliveryMomentDaprMessageHandler();
                        LogModel logModel = new LogModel(deliveryMoment.Id, "insert", deliveryMomentAsString, deliveryMoment.StoreNumber.ToString());
                        DeliveryMomentLogger.AddToLog(logModel);

                        deliveryMomentMessageHandler.PublishDeliveryMomentMessageAsync(deliveryMomentAsString);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error DeliveryMomentsMessageHandler for deliveryMoment : {deliveryMoment.Id} and exception ex: {ex.Message}");
                    }

                }
            }

        }

       
    }
}
