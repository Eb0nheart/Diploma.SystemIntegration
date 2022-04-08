using Case.CoreFunctionality.Implementations;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case.ElectricityTask;

public class WorkerService : BackgroundService
{
    private IProducer<Null, DateTime> _producer;
    private IConsumer<DateTime, double> _consumer;

    public WorkerService(IConfiguration configuration, DateTimeSerializer serializer)
    {
        var config = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("auto.offset.reset", "latest"),
            new KeyValuePair<string, string>("group.id", "price-receivers"),
            new KeyValuePair<string, string>("bootstrap.servers", "localhost:9092")
        };
        var producerBuilder = new ProducerBuilder<Null, DateTime>(config);
        producerBuilder.SetValueSerializer(serializer);
        _producer = producerBuilder.Build();
        var consumerBuilder = new ConsumerBuilder<DateTime, double>(config);
        consumerBuilder.SetKeyDeserializer(serializer);
        _consumer = consumerBuilder.Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

    }
}
