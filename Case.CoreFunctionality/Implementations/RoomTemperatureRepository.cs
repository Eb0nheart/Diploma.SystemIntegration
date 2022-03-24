using Dapper;
using System.Data.SqlClient;

namespace Case.CoreFunctionality.Implementations;

public class RoomTemperatureRepository : IRepository<RoomTemperature>
{
    private string SQL = "select Time = CAST(dato AS DATETIME) + CAST(tidspunkt AS DATETIME), grader as Temperature from Temperatur";
    private SqlConnection _connection;

    public RoomTemperatureRepository()
    {
        _connection = new SqlConnection("Server=tcp:indeklima.database.windows.net,1433;Initial Catalog=indeklima;Persist Security Info=False;User ID=systemintegration;Password=mnadsp9gu32rklag3289#2knda;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

    public async Task<IEnumerable<RoomTemperature>> GetLast24HoursAsync(CancellationToken cancellationToken = default)
    {
        var data = await _connection.QueryAsync<RoomTemperature>(SQL, cancellationToken);
        var yesterday = DateTime.Now.AddDays(-1);
        var todaysTemps = data.Where(temperature => temperature.Time > yesterday);
        return todaysTemps;
    }
}