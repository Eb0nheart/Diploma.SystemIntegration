using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case.CoreFunctionality.Implementations;

public class DateTimeSerializer : ISerializer<DateTime>, IDeserializer<DateTime>
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
