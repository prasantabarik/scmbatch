using System;
using System.Collections.Generic;
using System.Text;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DeliveryMomentsCreator
    {
        public static List<DeliveryMomentModel> PrepareDeliveryMoments(string storeNumber, DeliverySchedule deliverySchedule,
          DateTime deliveryDateTime, string delivererNumber, string wharehouseNumber)
        {
            List<DeliveryMomentModel> deliveryMoments = new List<DeliveryMomentModel>();
            var timeTables = deliverySchedule.TimeTable;
            if (timeTables != null && timeTables.Count > 0)
            {
                foreach (var timeTable in timeTables)
                {
                    var deliveryDay = DateTimeUtilities.DayOfTheWeek(timeTable.DeliveryDay);
                    if (deliveryDateTime.DayOfWeek.ToString() == deliveryDay)
                    {
                        TimeSpan onlyDeliveryTime = DateTime.Parse(timeTable.DeliveryTime).TimeOfDay;
                        TimeSpan onlyOrderTime = DateTime.Parse(timeTable.OrderTime).TimeOfDay;
                        TimeSpan onlyFillTime = DateTime.Parse(timeTable.FillTime).TimeOfDay;
                        TimeSpan onlyStartFillTime = DateTime.Parse(timeTable.StartTime).TimeOfDay;

                        var deliveryMoment = new DeliveryMomentModel();
                        deliveryMoment.Id = GenerateDeliveryMomentId(storeNumber, deliveryDateTime, deliverySchedule.DeliveryStreamNumber);
                        deliveryMoment.StoreNumber = Convert.ToInt32(storeNumber);
                        deliveryMoment.StreamNumber = deliverySchedule.DeliveryStreamNumber;

                        deliveryMoment.DeliveryDateTime = deliveryDateTime.Add(onlyDeliveryTime);

                        deliveryMoment.BoxSzie = timeTable.BoxSize;
                        deliveryMoment.DelivererNumber = Convert.ToInt32(delivererNumber);

                        var orderDateTime = deliveryDateTime;
                        orderDateTime = DateTimeUtilities.GetNextDateTimeFromDayCode(orderDateTime, timeTable.OrderDay).AddDays(-7);

                        deliveryMoment.OrderDateTime = orderDateTime.Add(onlyOrderTime);

                        var startFillDateTime = deliveryDateTime;
                        startFillDateTime = DateTimeUtilities.GetNextDateTimeFromDayCode(startFillDateTime, timeTable.StartDay).AddDays(-7);

                        deliveryMoment.StartFillDateTime = startFillDateTime.Add(onlyStartFillTime);

                        var fillDateTime = deliveryDateTime;
                        fillDateTime = DateTimeUtilities.GetNextDateTimeFromDayCode(fillDateTime, timeTable.FillDay);
                        deliveryMoment.FillDateTime = fillDateTime.Add(onlyFillTime);

                        deliveryMoment.InitialPromotionFlag = timeTable.InitialDeliveryFlag;
                        deliveryMoment.OrderStatus = null;
                        deliveryMoment.TotalInitialOrderQuantity = null;
                        deliveryMoment.TotalOrderQuantity = null;
                        deliveryMoment.MainDeliveryFlag = timeTable.MainDeliveryFlag;

                        deliveryMoment.StoreAdviseFlag = "N";
                        deliveryMoment.DeliveryScheamaType = 1;

                        StoreOrder storeOrder = new StoreOrder();
                        storeOrder.WarehouseNumber = Convert.ToInt32(wharehouseNumber);
                        storeOrder.OrderNumber = GenerateRandomNumber();

                        deliveryMoment.StoreOrders = new List<StoreOrder>();
                        deliveryMoment.StoreOrders.Add(storeOrder);
                        deliveryMoments.Add(deliveryMoment);
                        deliveryMoment.LogisticGroupExclusion = new List<int>();
                        if (timeTable.Exclusion != null && timeTable.Exclusion.Count > 0)
                        {
                            foreach (var exclusion in timeTable.Exclusion)
                            {
                                deliveryMoment.LogisticGroupExclusion.Add(exclusion.LogisticGroupNumber);
                            }
                        }
                        deliveryMoment.CreatedBy = "DeliveryMomentBatch";
                        deliveryMoment.UpdatedBy = "DeliveryMomentBatch";
                        deliveryMoment.CreationDateTime = DateTime.Now.ToString();
                        deliveryMoment.UpdateDateTime = DateTime.Now.ToString();
                    }
                }
            }

            return deliveryMoments;

        }

        private static int GenerateRandomNumber()
        {
            Random random = new Random();
            int num = random.Next(1000, int.MaxValue);
            return num;
        }

        private static string GenerateDeliveryMomentId(string storeId, DateTime deliveryDate, int deliveryStream )
        {
            return string.Concat(storeId, deliveryDate.ToString("yyyy-MM-dd"), deliveryStream.ToString());
        }
    }
}
