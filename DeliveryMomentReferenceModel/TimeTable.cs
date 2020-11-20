using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    public class TimeTable
    {
        [JsonPropertyName("orderDay")]
        public string OrderDay { get; set; }

        [JsonPropertyName("orderTime")]
        public string OrderTime { get; set; }

        [JsonPropertyName("deliveryDay")]
        public string DeliveryDay { get; set; }

        [JsonPropertyName("deliveryTime")]
        public string DeliveryTime { get; set; }

        [JsonPropertyName("fillDay")]
        public string FillDay { get; set; }

        [JsonPropertyName("fillTime")]
        public string FillTime { get; set; }

        [JsonPropertyName("boxSize")]
        public int BoxSize { get; set; }

        [JsonPropertyName("initialDeliveryFlag")]
        public string initialDeliveryFlag { get; set; }

        [JsonPropertyName("mainDeliveryFlag")]
        public string mainDeliveryFlag { get; set; }

        [JsonPropertyName("startDay")]
        public string startDay { get; set; }

        [JsonPropertyName("startTime")]
        public string startTime { get; set; }

        [JsonPropertyName("exclusion")]
        public List<Exclusion> Exclusion { get; set; }
    }
}
