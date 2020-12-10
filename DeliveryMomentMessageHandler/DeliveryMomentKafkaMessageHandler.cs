using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.CommonUtilities;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler
{
    public class DeliveryMomentKafkaMessageHandler : IDeliveryMomentMessageHandler
    {
        public void PublishDeliveryMomentMessageAsync(string message)
        {
            Console.WriteLine("Start DeliveryMomentConfluentMessageHandler.PublishDeliveryMomentMessage()");

            try
            {
                string brokerList = "localhost:9192";
                string topicName = "deliverymomentmessage";
                var config = new ProducerConfig { BootstrapServers = brokerList };
                
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