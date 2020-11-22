using System;
using System.Data;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandlerr;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace DeliveryMomentMessageProcesser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {                      
                Console.WriteLine("DeliveryMomentMessageProcesser Main start");
                DeliveryMomentConfluentMessageListner.StartSubscribe();
                Console.WriteLine("DeliveryMomentMessageProcesser Main End");
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
            }
        }

        #region Test
        private static void TestDeliveryMoment()
        {
            DeliveryMomentService deliveryMomentService = new DeliveryMomentService();
           
            deliveryMomentService.SaveDeliveryMomentAsync(GetSampleModel());

            Console.ReadLine();
        }
        private static DeliveryMomentModel GetSampleModel()
        {
            DeliveryMomentModel deliveryMoment = new DeliveryMomentModel();
            deliveryMoment.BoxSzie = 1;
            deliveryMoment.CreatedBy = "Nag 123";
            deliveryMoment.CreationDateTime = DateTime.Now.ToString();
            deliveryMoment.DelivererNumber = 1;
            deliveryMoment.DeliveryDateTime = DateTime.Now;
            deliveryMoment.DeliveryScheamaType = 1;
            deliveryMoment.DeliveryStreamName = "1";
            deliveryMoment.DeliveryStreamName = "Stream";
            deliveryMoment.FillDateTime = DateTime.Now;
            deliveryMoment.Id = "test123";
            deliveryMoment.InitialPromotionFlag = "N";
            deliveryMoment.MainDeliveryFlag = "N";
            deliveryMoment.OrderDateTime = DateTime.Now;
            deliveryMoment.OrderStatus = "null";
            deliveryMoment.SchemaName = "Schema 123";
            deliveryMoment.StartFillDateTime = DateTime.Now;
            deliveryMoment.StoreAdviseFlag = "N";
            deliveryMoment.StoreNumber = 1;
            deliveryMoment.StreamNumber = 1;
            deliveryMoment.TotalInitialOrderQuantity = "null";
            deliveryMoment.TotalOrderQuantity = "null";
            deliveryMoment.UpdateDateTime = DateTime.Now.ToString();
            deliveryMoment.UpdatedBy = "test";
            return deliveryMoment;
        }

        #endregion
    }
}
