
using System.Text.Json.Serialization;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    
    public class Store
    {
        [JsonPropertyName("storeId")]
        public int StoreId { get; set; }
    }
}
