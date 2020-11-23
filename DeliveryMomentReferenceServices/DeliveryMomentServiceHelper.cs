using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices
{
    class DeliveryMomentServiceHelper
    {
        public static bool JsonCompareLogisticGroupExclusion(DeliveryMomentModel fromMessage, DeliveryMomentModel fromDB)
        {
            if ((fromMessage == null) || (fromDB == null)) return false;

            var objJson = JsonConvert.SerializeObject(fromMessage.LogisticGroupExclusion);
            var anotherJson = JsonConvert.SerializeObject(fromDB.LogisticGroupExclusion);

            return objJson == anotherJson;
        }

        public static bool CompareDeliveryModels(DeliveryMomentModel fromMessage, DeliveryMomentModel fromDB)
        {
            bool isSame = true;
            if ((fromMessage == null) || (fromDB == null))
            {
                isSame = false;
                return isSame;
            }

            if (fromMessage.BoxSzie != fromDB.BoxSzie || fromMessage.DelivererNumber != fromDB.DelivererNumber ||
                fromMessage.DeliveryDateTime != fromDB.DeliveryDateTime || fromMessage.FillDateTime != fromDB.FillDateTime ||
                fromMessage.InitialPromotionFlag != fromDB.InitialPromotionFlag || fromMessage.MainDeliveryFlag != fromDB.MainDeliveryFlag ||
                 fromMessage.OrderDateTime != fromDB.OrderDateTime || fromMessage.StartFillDateTime != fromDB.StartFillDateTime ||
                 fromMessage.StoreAdviseFlag != fromDB.StoreAdviseFlag || fromMessage.StoreNumber != fromDB.StoreNumber ||
                 fromMessage.StreamNumber != fromDB.StreamNumber)
            {
                isSame = false;
                return isSame;
            }

            if (CompareStoreOrders(fromMessage, fromDB) == false || CompareLogisticGroupExclusion(fromMessage, fromDB) == false)
            {
                isSame = false;
                return isSame;
            }

            return isSame;
        }

        private static bool CompareStoreOrders(DeliveryMomentModel fromMessage, DeliveryMomentModel fromDB)
        {
            bool isSame = true;
            if (GetStoreOrdersCount(fromMessage) != GetStoreOrdersCount(fromDB))
                isSame = false;
            else
            {
                if (fromMessage.StoreOrders != null && fromDB.StoreOrders != null &&
                    fromMessage.StoreOrders.Count > 0 && fromDB.StoreOrders.Count > 0)
                {
                    foreach (var storeOrder in fromMessage.StoreOrders)
                    {
                        var orders = from order in fromDB.StoreOrders
                                     where order.WarehouseNumber == storeOrder.WarehouseNumber
                                     select order;
                        if (orders == null && orders.ToList().Count < 0)
                        {
                            isSame = false;
                            break;
                        }
                    }
                }
            }
            return isSame;
        }

        private static bool CompareLogisticGroupExclusion(DeliveryMomentModel fromMessage, DeliveryMomentModel fromDB)
        {
            bool isSame = true;
            if (GetLogisticGroupExclusionCount(fromMessage) != GetLogisticGroupExclusionCount(fromDB))
                isSame = false;
            else
            {
                isSame = JsonCompareLogisticGroupExclusion(fromMessage, fromDB);
            }
            return isSame;
        }
        private static int GetStoreOrdersCount(DeliveryMomentModel deliveryMomentModel)
        {
            int count = 0;
            if (deliveryMomentModel.StoreOrders != null && deliveryMomentModel.StoreOrders.Count > 0)
            {
                count = deliveryMomentModel.StoreOrders.Count;
            }

            return count;
        }

        private static int GetLogisticGroupExclusionCount(DeliveryMomentModel deliveryMomentModel)
        {
            int count = 0;
            if (deliveryMomentModel.LogisticGroupExclusion != null && deliveryMomentModel.LogisticGroupExclusion.Count > 0)
            {
                count = deliveryMomentModel.LogisticGroupExclusion.Count;
            }

            return count;
        }
    }
}
