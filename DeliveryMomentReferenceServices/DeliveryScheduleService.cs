using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices
{
    public class DeliveryScheduleService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<Repsponse> GetDeliverySchedulesAsync(string storeId)
        {
            Repsponse deliveryScheduleResponse =null;
            try
            {
                string url = $"http://delschcrud-edppublic-delschcrud-dev.59ae6b648ca3437aae3a.westeurope.aksapp.io/api/v1/deliveryschedule-crud-service/model?storeNumber={storeId}";
                var streamDeliverySchedule = await client.GetStringAsync(url);
                deliveryScheduleResponse = JsonConvert.DeserializeObject<Repsponse>(streamDeliverySchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return deliveryScheduleResponse;
        }
    }
}
