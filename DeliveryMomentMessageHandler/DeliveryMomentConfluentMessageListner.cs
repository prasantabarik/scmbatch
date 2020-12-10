using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentReferenceServices;
using TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.Models;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler
{
    public class DeliveryMomentConfluentMessageListner
    {
        public static void StartSubscribe()
        {

            var brokerList = "pkc-lq8gm.westeurope.azure.confluent.cloud:9092";
            var topics = new List<string>();
            //topics.Add("DeliveryMomentProcessor");
            topics.Add("DeliveryMomentMessageProcesser");

            Console.WriteLine($"Started consumer, Ctrl-C to stop consuming");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            Run_Consume(brokerList, topics, cts.Token);

        }

        private static void Run_Consume(string brokerList, List<string> topics, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = brokerList,
                GroupId = "mvp-batch-consumer-dev",
                EnableAutoCommit = false,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "GHEF4JHYMYPE2EQV",
                SaslPassword = "w9Z7vlY3RcWB3rD1Q1bgj/NmpDiopNHegNIFna6CQYamK6oOCRG/+yAXthdSNRJV",
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };
            Run(config, brokerList, topics, cancellationToken);

        }

        private static void Run(ConsumerConfig config, string brokerList, List<string> topics, CancellationToken cancellationToken)
        {
            // Note: If a key or value deserializer is not set (as is the case below), the 
            // deserializer corresponding to the appropriate type from Confluent.Kafka.Deserializers
            // will be used automatically (where available). The default deserializer for string
            // is UTF8. The default deserializer for Ignore returns null for all input data
            // (including non-null data).
            using (var consumer = new ConsumerBuilder<Ignore, string>(config)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
                    // possibly manually specify start offsets or override the partition assignment provided by
                    // the consumer group by returning a list of topic/partition/offsets to assign to, e.g.:
                    // 
                    // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .Build())
            {
                consumer.Subscribe(topics);

                Subscribe(cancellationToken, consumer);
            }
        }


        private static void Subscribe(CancellationToken cancellationToken, IConsumer<Ignore, string> consumer)
        {
            const int commitPeriod = 1;
            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        if (consumeResult.IsPartitionEOF)
                        {
                            Console.WriteLine(
                                $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                            continue;
                        }

                        Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                        SaveDeliveryMoment(consumeResult.Message.Value);
                        if (consumeResult.Offset % commitPeriod == 0)
                        {
                            // The Commit method sends a "commit offsets" request to the Kafka
                            // cluster and synchronously waits for the response. This is very
                            // slow compared to the rate at which the consumer is capable of
                            // consuming messages. A high performance application will typically
                            // commit offsets relatively infrequently and be designed handle
                            // duplicate messages in the event of failure.
                            try
                            {
                                consumer.Commit(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                Console.WriteLine($"Commit error: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Closing consumer.");
                consumer.Close();
            }
        }


        private static void SaveDeliveryMoment(string deliveryMomentMessage)
        {
            try
            {
                var deliveryMomentModel = Deserialize(deliveryMomentMessage);
                if (deliveryMomentModel != null)
                {
                    DeliveryMomentService deliveryMomentService = new DeliveryMomentService();
                    deliveryMomentService.Save(deliveryMomentModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeliveryMomentConfluentMessageListner.Deserialize() Exception {ex.Message} for {deliveryMomentMessage}");
            }
        }

        private static DeliveryMomentModel Deserialize(string messageStream)
        {
            DeliveryMomentModel deliveryMomentModel = null;
            try
            {
               deliveryMomentModel = JsonConvert.DeserializeObject<DeliveryMomentModel>(messageStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeliveryMomentConfluentMessageListner.Deserialize() Exception {ex.Message}");
            }

            return deliveryMomentModel;
        }
    }
}

