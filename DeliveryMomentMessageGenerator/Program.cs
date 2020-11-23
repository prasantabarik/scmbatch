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
            Console.WriteLine("Main() Start");
            GenerateDeliveryMoments();
            Console.WriteLine("Main() End");
        }

        private static void GenerateDeliveryMoments()
        {
            Console.WriteLine("GenerateDeliveryMoments() Start");
            try
            {
                //Generate delivery schdules for datetine.mow + 2 to next 42 days.
                var deliveryMomentBatchDates = DateTimeUtilities.GetDeliveryMomentBatchDatesToRun();
                //Get All the stores to process
                var storeDocuments = DeliveryMomentServiceHelper.GetAllStores();

                //Get All the stores to process
                var deliveryStreamDocuments = DeliveryMomentServiceHelper.GetAllDeliveryStreams();

                //Generate DeliveryMoments for each store
                foreach (var storeDocument in storeDocuments)
                {
                    try
                    {
                        Console.WriteLine($"Start GenerateDeliveryMoments for Store Number : {storeDocument.ToString()}");
                        string storeId = storeDocument.GetValue("storeId").ToString();
                        //Todo: Need to change
                        if (storeId == "7010")
                        {
                            //Get logicstic channels for each store.
                            var logisticChannels = DeliveryMomentServiceHelper.GetLogisticChannels(storeId);
                            //Generate deliverymoments for each store
                            DeliveryMomentsGenerator.GenerateDeliveryMomentsByStore(storeDocument, deliveryMomentBatchDates, logisticChannels, deliveryStreamDocuments);
                            Console.WriteLine($"End GenerateDeliveryMoments for Store Number : {storeDocument.ToString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"GenerateDeliveryMoments for Store : {storeDocument.ToString()} and exception ex: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error for GenerateDeliveryMoments: {ex.Message} ");
            }
            Console.WriteLine("GenerateDeliveryMoments() End");
        }
    }
}

