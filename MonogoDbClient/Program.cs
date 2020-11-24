using System;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository;

namespace MonogoDbClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAll();
            DeleteDeliveryMomentBatchDocuments();
            GetAll();
        }

        static void GetAll()
        {
            DeliveryMomentRepository deliveryMomentRepository = new DeliveryMomentRepository();
            var list = deliveryMomentRepository.GetAllBatchDeliveryMoments();
        }

       static void DeleteDeliveryMomentBatchDocuments()
        {
            DeliveryMomentRepository deliveryMomentRepository = new DeliveryMomentRepository();
            deliveryMomentRepository.DeleteAllBatchDeliveryMoments();
        }
    }
}
