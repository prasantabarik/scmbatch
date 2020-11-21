using System;
using System.Collections.Generic;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler;
using System.Threading.Tasks;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DeliveryMomentsMessageHandler
    {
        public static async Task PublishMessages(List<DeliveryMomentModel> deliveryMoments)
        {
            if (deliveryMoments != null && deliveryMoments.Count > 0)
            {
                foreach (var deliveryMoment in deliveryMoments)
                {
                    try
                    {
                        var deliveryMomentAsString = DeliveryMomentSerializer.Serialize(deliveryMoment);
                        DeliveryMomentConfluentMessageHandler deliveryMomentMessageHandler = new DeliveryMomentConfluentMessageHandler();
                        await deliveryMomentMessageHandler.PublishDeliveryMomentMessage(deliveryMomentAsString);
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
