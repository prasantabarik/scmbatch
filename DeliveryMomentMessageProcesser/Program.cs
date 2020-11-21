using System;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandlerr;

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
    }
}
