using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    public class Repsponse
    {
        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("responseDescription")]
        public string ResponseDescription { get; set; }

        [JsonPropertyName("response")]
        public List<DeliverySchedule> Response { get; set; }

    }

    public class DeliveryMomentRepsponse
    {
        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("responseDescription")]
        public string ResponseDescription { get; set; }

        [JsonPropertyName("response")]
        public List<DeliveryMomentModel> Response { get; set; }

    }
}
