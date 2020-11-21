using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DeliveryMomentSerializer
    {
        public static string Serialize(DeliveryMomentModel deliveryMoment)
        {
            string deliveryMomentSerializeData = JsonConvert.SerializeObject(deliveryMoment);
            return deliveryMomentSerializeData;
        }
    }
}
