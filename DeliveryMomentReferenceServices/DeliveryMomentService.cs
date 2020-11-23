using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                deliveryMomentResponse = JsonConvert.DeserializeObject<DeliveryMomentRepsponse>(deliveryMomentStream.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return deliveryMomentResponse;
        }

        private async Task SaveDeliveryMomentAsync(DeliveryMomentModel deliveryMomentModel)
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

        public void Save(DeliveryMomentModel deliveryMomentModel)
        {
            DeliveryMomentModel dbDeliveryMomentModel = null;
            try
            {
                var deliveryMomentResponse = GetDeliveryMomentAsync(deliveryMomentModel.StoreNumber.ToString(), deliveryMomentModel.DeliveryDateTime, deliveryMomentModel.StreamNumber.ToString());

                if (deliveryMomentResponse.Result != null && deliveryMomentResponse.Result.Response != null && deliveryMomentResponse.Result.Response.Count > 0)
                {
                    dbDeliveryMomentModel = deliveryMomentResponse.Result.Response[0];
                    if (dbDeliveryMomentModel != null)
                    {
                        CompareAndSave(deliveryMomentModel, dbDeliveryMomentModel);
                    }
                }
                else
                {
                    SaveDeliveryMomentAsync(deliveryMomentModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CompareAndSave(DeliveryMomentModel deliveryMomentModel, DeliveryMomentModel dbDeliveryMomentModel)
        {
            try
            {
                bool isSame = DeliveryMomentServiceHelper.CompareDeliveryModels(deliveryMomentModel, dbDeliveryMomentModel);
                if (!isSame)
                {
                    deliveryMomentModel.CreatedBy = dbDeliveryMomentModel.CreatedBy;
                    deliveryMomentModel.CreationDateTime = dbDeliveryMomentModel.CreationDateTime;
                    SaveDeliveryMomentAsync(deliveryMomentModel);
                    Console.WriteLine($"DeliveryMoment {deliveryMomentModel.Id} is saved");
                }
                else
                {
                    Console.WriteLine($"deliveryMoment {deliveryMomentModel.Id} has not change. Need not to be updated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       
    }
}
