using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    public class Exclusion
    {
        [JsonPropertyName("logisticGroupNumber")]
        public int LogisticGroupNumber { get; set; }


    }
}
