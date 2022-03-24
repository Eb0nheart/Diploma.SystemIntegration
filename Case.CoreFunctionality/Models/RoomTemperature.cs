using System.ComponentModel.DataAnnotations.Schema;

namespace Case.CoreFunctionality.Models;

public class RoomTemperature
{
    public DateTime Time { get; set; }

    public double Temperature { get; set; }
}
