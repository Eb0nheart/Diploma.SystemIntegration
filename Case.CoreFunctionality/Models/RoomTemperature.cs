using System.ComponentModel.DataAnnotations.Schema;

namespace Case.CoreFunctionality.Models;

public class RoomTemperature
{
    [Column("dato")]
    public DateTime Date { get; set; }

    [Column("tidspunkt")]
    public TimeSpan Time { get; set; }

    [Column("grader")]
    public double Temperature { get; set; }
}
