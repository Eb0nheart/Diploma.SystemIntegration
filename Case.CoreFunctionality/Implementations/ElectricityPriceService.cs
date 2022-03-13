using System.Net;

namespace Case.CoreFunctionality.Implementations;
public class ElectricityPriceService : IElectricityPriceService
{
    private const string baseUri = "ftp://inverter.westeurope.cloudapp.azure.com";

    public async Task<Dictionary<TimeOnly, double>> GetNextDaysPricesAsync(CancellationToken token = default)
    {
        var files = await GetFilesToDownload();
        var usages = new List<double>();
        files.ForEach(async file => usages.Add(await GetUsage($"{baseUri}/{file}")));

        var data = new Dictionary<TimeOnly, double>();
        var hour = 1;
        usages.ForEach(usage =>
        {
            data.Add(new TimeOnly(hour, 0), usage);
            hour++;
        });

        return data;
    }

    private async Task<double> GetUsage(string uri)
    {
        using var reader = GetReader(uri, WebRequestMethods.Ftp.DownloadFile);
        var lineNumber = 1;
        var lines = new List<string>();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (lineNumber <= 6)
            {
                lineNumber++;
                continue;
            }

            if(line != "[wr_ende]")
            {
                lines.Add(line);
            }
        }

        var firstLineColumns = lines.First().Split(';');
        var lastLineColumns = lines.Last().Split(';');

        var first = firstLineColumns.ElementAt(37);
        var last = lastLineColumns.ElementAt(37);

        var firstUsage = double.Parse(first);
        var lastUsage = double.Parse(last);

        return lastUsage - firstUsage;
    }

    private async Task<List<string?>> GetFilesToDownload()
    {
        using var reader = GetReader(baseUri, WebRequestMethods.Ftp.ListDirectory);
        var hoursOfToday = DateTime.Now.Hour;
        var filesToDownload = new List<string?>();

        for (int i = 0; i <= hoursOfToday; i++)
        {
            filesToDownload.Add(await reader.ReadLineAsync());
        }

        return filesToDownload;
    }

    private StreamReader GetReader(string uri, string requestMethod)
    {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
        var request = WebRequest.Create(uri);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

        request.Method = requestMethod;
        request.Credentials = new NetworkCredential("studerende", "kmdp4gslmg46jhs");

        var response = request.GetResponse();
        return new StreamReader(response.GetResponseStream());
    }
}
