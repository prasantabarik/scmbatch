using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices
{
    public class DeliveryMomentService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<DeliveryMomentRepsponse> GetDeliveryMomentAsync(string storeId, string deriveryDate, string streamNumber)
        {
            DeliveryMomentRepsponse deliveryMomentResponse = null;
            try
            {         
                string url = $"http://deliverymomentcrud-edppublic-deliverymomentcrud-dev.59ae6b648ca3437aae3a.westeurope.aksapp.io/api/v1/deliveryMoment-Crud-service/model?storeNumber={storeId}&streamNumber={streamNumber}&deliveryDateTime={deriveryDate}";
                
                 var deliveryMomentStream = await client.GetStringAsync(url);
                deliveryMomentResponse = JsonConvert.DeserializeObject<DeliveryMomentRepsponse>(deliveryMomentStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return deliveryMomentResponse;
        }

        public async Task SaveDeliveryMomentAsync(DeliveryMomentModel deliveryMomentModel)
        {
            try
            {
                string url = $"http://deliverymomentcrud-edppublic-deliverymomentcrud-dev.59ae6b648ca3437aae3a.westeurope.aksapp.io/api/v1/deliveryMoment-Crud-service/model";
                          

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    var json = JsonConvert.SerializeObject(deliveryMomentModel);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
