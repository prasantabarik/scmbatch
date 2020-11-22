using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    public class DeliveryMomentModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

		[JsonProperty("storeNumber")]
		public int StoreNumber { get; set; }

		[JsonProperty("streamNumber")]
		public int StreamNumber { get; set; }

		[JsonProperty("deliveryStreamName")]
		public string DeliveryStreamName { get; set; }

		[JsonProperty("schemaName")]
		public string SchemaName { get; set; }

		[JsonProperty("deliveryDateTime")]
		public DateTime DeliveryDateTime { get; set; }

		[JsonProperty("orderDateTime")]
		public DateTime OrderDateTime { get; set; }

		[JsonProperty("initialPromotionFlag")]
		public string InitialPromotionFlag { get; set; }

		[JsonProperty("orderStatus")]
		public string OrderStatus { get; set; }

		[JsonProperty("totalInitialOrderQuantity")]
		public string TotalInitialOrderQuantity { get; set; }

		[JsonProperty("totalOrderQuantity")]
		public string TotalOrderQuantity { get; set; }

		[JsonProperty("boxSize")]
		public int BoxSzie { get; set; }

		[JsonProperty("fillDateTime")]
		public DateTime FillDateTime { get; set; }

		[JsonProperty("mainDeliveryFlag")]
		public string MainDeliveryFlag { get; set; }

		[JsonProperty("storeAdviseFlag")]
		public string StoreAdviseFlag { get; set; }

		[JsonProperty("deliverySchemaType")]
		public int DeliveryScheamaType { get; set; }

		[JsonProperty("delivererNumber")]
		public int DelivererNumber { get; set; }

		[JsonProperty("startFillTime")]
		public DateTime StartFillDateTime { get; set; }

		[JsonProperty("logisticGroupExclusion")]
		public List<int> LogisticGroupExclusion { get; set; }

		[JsonProperty("storeOrder")]
		public List<StoreOrder> StoreOrders { get; set; }

		[JsonProperty("createdBy")]
		public string CreatedBy { get; set; }

		[JsonProperty("creationDateTime")]
		public string CreationDateTime { get; set; }

		[JsonProperty("updatedBy")]
		public string UpdatedBy { get; set; }

		[JsonProperty("updateDateTime")]
		public string UpdateDateTime { get; set; }
	}

    public class StoreOrder
    {
		[JsonProperty("orderNumber")]
		public int OrderNumber { get; set; }

		[JsonProperty("warehouseNumber")]
		public int WarehouseNumber { get; set; }
    }
}
