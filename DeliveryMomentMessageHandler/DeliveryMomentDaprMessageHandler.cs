using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler
{
   public class DeliveryMomentDaprMessageHandler : IDeliveryMomentMessageHandler
    {
        public void PublishDeliveryMomentMessageAsync(string message)
        {
            Console.WriteLine("Start DeliveryMomentDaprMessageHandler.PublishDeliveryMomentMessage()");

            HttpClient client = new HttpClient();
            try
            { 
                  string url = "http://localhost:3500/v1.0/publish/pubsub/deliverymomentmessage";
               
                var response = client.PostAsync(url, new StringContent(message)).Result;

                Console.WriteLine($"Message published with status {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeliveryMomentDaprMessageHandler.PublishDeliveryMomentMessage() Error: " + ex.Message);
            }
            Console.WriteLine("End DeliveryMomentDaprMessageHandler.PublishDeliveryMomentMessage()");
        }
    }
}
