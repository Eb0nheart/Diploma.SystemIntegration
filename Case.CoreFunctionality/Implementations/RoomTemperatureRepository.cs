using Dapper;
using System.Data.SqlClient;

namespace Case.CoreFunctionality.Implementations;

public class RoomTemperatureRepository : IRepository<RoomTemperature>
{
    private string SQL = "select * from Temperatur";
    private SqlConnection _connection;

    public RoomTemperatureRepository()
    {
        _connection = new SqlConnection("Server=tcp:indeklima.database.windows.net,1433;Initial Catalog=indeklima;Persist Security Info=False;User ID=systemintegration;Password=mnadsp9gu32rklag3289#2knda;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

    public async Task<IEnumerable<RoomTemperature>> SelectAllAsync(CancellationToken cancellationToken = default)
    {
        var data = await _connection.QueryAsync<RoomTemperature>(SQL, cancellationToken);
        var todaysTemps = data.Where(temperature => DateOnly.FromDateTime(temperature.Date) == DateOnly.FromDateTime(DateTime.Now));
        return todaysTemps;
    }
}