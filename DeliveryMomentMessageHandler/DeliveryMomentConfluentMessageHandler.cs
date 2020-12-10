﻿using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.CommonUtilities;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler
{
    public class DeliveryMomentConfluentMessageHandler : IDeliveryMomentMessageHandler
    {
        public void PublishDeliveryMomentMessageAsync(string message)
        {
            Console.WriteLine("Start DeliveryMomentConfluentMessageHandler.PublishDeliveryMomentMessage()");

            try
            {
                string brokerList = "pkc-lq8gm.westeurope.azure.confluent.cloud:9092";
                string topicName = "DeliveryMomentMessageProcesser";
                var config = new ProducerConfig { BootstrapServers = brokerList };
                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SaslUsername = "GHEF4JHYMYPE2EQV";
                config.SaslPassword = "w9Z7vlY3RcWB3rD1Q1bgj/NmpDiopNHegNIFna6CQYamK6oOCRG/+yAXthdSNRJV";
                using (var deliveryMomentProducer = new ProducerBuilder<string, string>(config).Build())
                {
                    try
                    {
                        Guid guid = Guid.NewGuid();

                        var deliveryReport = deliveryMomentProducer.ProduceAsync(
                            topicName, new Message<string, string> { Key = guid.ToString(), Value = message }).Result;

                        

                        Console.WriteLine($" mesage: {guid} is delivered to: {deliveryReport.TopicPartitionOffset}");
                    }
                    catch (ProduceException<string, string> e)
                    {
                        Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeliveryMomentConfluentMessageHandler.PublishDeliveryMomentMessage() Error: " + ex.Message);
            }
            Console.WriteLine("End DeliveryMomentConfluentMessageHandler.PublishDeliveryMomentMessage()");
        }
    }
}