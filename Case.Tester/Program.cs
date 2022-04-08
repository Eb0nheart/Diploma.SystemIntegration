// See https://aka.ms/new-console-template for more information

using Case.CoreFunctionality.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

var config = new List<KeyValuePair<string, string>>
{
    new KeyValuePair<string, string>("auto.offset.reset", "latest"),
    new KeyValuePair<string, string>("group.id", "price-receivers"),
    new KeyValuePair<string, string>("bootstrap.servers", "localhost:9092")
};

var pconfig = new ProducerConfig()
{
    BootstrapServers = "localhost:9092"
};
var cconfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = "price-receivers",
    AutoOffsetReset = AutoOffsetReset.Latest
};

var pb = new ProducerBuilder<Null, DateTime>(pconfig);
var serializer = new DateTimeSerializer();
pb.SetValueSerializer(serializer);
var cb = new ConsumerBuilder<Null, DateTime>(cconfig);
cb.SetValueDeserializer(serializer);

var consumer = cb.Build();
var producer = pb.Build();

consumer.Subscribe("get-price");
await producer.ProduceAsync("get-price", new Message<Null, DateTime>() { Value = DateTime.Now });
var lollern = consumer.Consume();
Console.WriteLine(lollern.Message.Value);

Console.WriteLine("Hello, World!");

class DateTimeSerializer : ISerializer<DateTime>, IDeserializer<DateTime>
{
    public DateTime Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return DateTime.Parse(Encoding.UTF8.GetString(data));
    }

    public byte[] Serialize(DateTime data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(data.ToString());
    }
}