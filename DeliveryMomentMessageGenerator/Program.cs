using System;
using System.Collections.Generic;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.ReferenceRepository;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;
using MongoDB.Bson;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateDeliveryMoments();
            Console.ReadLine();
        }

        private static void GenerateDeliveryMoments()
        {
            //Generate delivery schdules for datetine.mow + 2 to next 42 days.
            var deliveryMomentBatchDates = DateTimeUtilities.GetDeliveryMomentBatchDatesToRun();
            //Get All the stores to process
            var storeDocuments = DeliveryMomentServiceHelper.GetAllStores();
            //Generate DeliveryMoments for each store
            foreach (var storeDocument in storeDocuments)
            {
                Console.WriteLine($"Start DeliveryMomentMessageGenerator for Store Number : {storeDocument.ToString()}");
                string storeId = storeDocument.GetValue("storeId").ToString();
                //Get logicstic channels for each store.
                var logisticChannels = DeliveryMomentServiceHelper.GetLogisticChannels(storeId);
                //Generate deliverymoments for each store
                DeliveryMomentsGenerator.GenerateDeliveryMomentsByStore(storeDocument, deliveryMomentBatchDates, logisticChannels);
                Console.WriteLine($"End DeliveryMomentMessageGenerator for Store Number : {storeDocument.ToString()}");
            }
        }
    }
}

