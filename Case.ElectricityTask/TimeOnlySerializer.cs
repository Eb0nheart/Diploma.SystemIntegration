using Confluent.Kafka;

namespace Case.ElectricityService;

public class TimeOnlySerializer : ISerializer<TimeOnly>
{
    public byte[] Serialize(TimeOnly data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(data.ToString());
    }
}
