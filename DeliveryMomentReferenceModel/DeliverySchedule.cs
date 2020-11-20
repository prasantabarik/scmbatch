using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    
    public class DeliverySchedule
    {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("storeNumber")]
        public int StoreNumber { get; set; }

        [JsonPropertyName("deliveryStreamNumber")]
        public int DeliveryStreamNumber { get; set; }

        [JsonPropertyName("deliveryStreamName")]
        public string DeliveryStreamName { get; set; }

        [JsonPropertyName("schemaName")]
        public string SchemaName { get; set; }

        [JsonPropertyName("deliverySchemaType")]
        public int DeliverySchemaType { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("timeTable")]
        public List<TimeTable> TimeTable { get; set; }
    }

   

    
}
