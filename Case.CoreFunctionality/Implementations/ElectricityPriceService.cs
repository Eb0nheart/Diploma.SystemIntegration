
using Renci.SshNet;

namespace Case.CoreFunctionality.Implementations;
public class ElectricityPriceService : IElectricityPriceService
{
    public async Task<Dictionary<TimeOnly, double>> GetNextDaysPricesAsync(CancellationToken token = default)
    {
        using(var client = new SftpClient("inverter.westeurope.cloudapp.azure.com", 21, "studerende", "kmdp4gslmg46jhs"))
        {
            client.Connect();
            var list = client.ListDirectory("");
        }

        return new()
        {
            { TimeOnly.FromDateTime(DateTime.Now), 23.64 }
        };
    }


}
