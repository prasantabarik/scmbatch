using System;
using System.Collections.Generic;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models
{
    public class DeliveryMomentModel
    {
        public int StoreNumber { get; set; }
        public int StreamNumber { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string InitialPromotionFlag { get; set; }
        public string OrderStatus { get; set; }
        public string TotalInitialOrderQuantity { get; set; }
        public string TotalOrderQuantity { get; set; }
        public int BoxSzie { get; set; }
        public DateTime FillDateTime { get; set; }
        public string MainDeliveryFlag { get; set; }
        public string StoreAdviseFlag { get; set; }
        public int DeliveryScheamaType { get; set; }
        public int DelivererNumber { get; set; }
        public DateTime StartFillDateTime { get; set; }
        public List<int> LogisticGroupExclusion { get; set; }
        public List<StoreOrder> StoreOrders { get; set; }
    }

    public class StoreOrder
    {
        public int OrderNumber { get; set; }
        public int WarehouseNumber { get; set; }
    }
}
