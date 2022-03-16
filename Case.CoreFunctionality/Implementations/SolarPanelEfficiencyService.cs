using System.Net;

namespace Case.CoreFunctionality.Implementations;

public class SolarPanelEfficiencyService : ISolarPanelEfficiencyService
{
    private const string baseUri = "ftp://inverter.westeurope.cloudapp.azure.com";
    private const int headerRows = 6;
    private const int fileEndRows = 2;
    private const int energyColumn = 37;

    private readonly WebClient client;

    public SolarPanelEfficiencyService(WebClient client)
    {
        this.client = client;
    }

    public async Task<Dictionary<TimeOnly, double>> GetEfficiencyForTodayAsync(CancellationToken token = default)
    {
        var files = await GetFilesToDownload();
        var data = new Dictionary<TimeOnly, double>();

        var now = DateTime.Now;
        foreach(var file in files)
        {
            var timestamp = file.Split("-").Last();

            var month = timestamp[2..4];
            var day = timestamp[4..6];
            if (month != now.Month.ToString("00") || day != now.Day.ToString("00"))
            {
                continue;
            }

            var usage = await GetUsage($"{baseUri}/{file}");
            var hour = int.Parse(timestamp[6..8]);
            data.Add(new TimeOnly(hour, 0), usage);
        }

        return data;
    }

    private async Task<double> GetUsage(string uri)
    {
        var response = await client.DownloadStringTaskAsync(uri);

        var rows = response.Split("\n");
        var dataSectionWithEnd = rows.Skip(headerRows);
        var dataSection = dataSectionWithEnd.SkipLast(fileEndRows);

        var firstLineColumns = dataSection.First().Split(';');
        var lastLineColumns = dataSection.Last().Split(';');

        var firstUsageString = firstLineColumns.ElementAt(energyColumn);
        var lastUsageString = lastLineColumns.ElementAt(energyColumn);

        var firstUsage = double.Parse(firstUsageString);
        var lastUsage = double.Parse(lastUsageString);

        return lastUsage - firstUsage;
    }

    private async Task<List<string>> GetFilesToDownload()
    {
        using var reader = GetReader(baseUri, WebRequestMethods.Ftp.ListDirectory);
        var everything = await reader.ReadToEndAsync();
        var allFiles = everything.Replace("\r", "").Split('\n');
        return allFiles.Where(file => !string.IsNullOrWhiteSpace(file)).ToList();
    }

    private StreamReader GetReader(string uri, string requestMethod)
    {
        var request = WebRequest.Create(uri);
        request.Method = requestMethod;
        request.Credentials = new NetworkCredential("studerende", "kmdp4gslmg46jhs");
        var response = request.GetResponse();
        return new StreamReader(response.GetResponseStream());
    }
}