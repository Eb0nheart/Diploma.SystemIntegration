using System.Net;

namespace Case.CoreFunctionality.Implementations;

public class SolarPanelEfficiencyService : ISolarPanelEfficiencyService
{
    private const string baseUri = "ftp://inverter.westeurope.cloudapp.azure.com";

    public async Task<Dictionary<TimeOnly, double>> GetEfficiencyForTodayAsync(CancellationToken token = default)
    {
        var files = await GetFilesToDownload();
        var usages = new List<double>();
        var data = new Dictionary<TimeOnly, double>();

        var now = DateTime.Now;
        files.ForEach(async file =>
        {
            var timestamp = file.Split("-").Last();

            var month = timestamp[2..4];
            if (month != now.Month.ToString("00"))
            {
                return;
            }

            var day = timestamp[4..6];
            if (day != now.Day.ToString("00"))
            {
                return;
            }

            var usage = await GetUsage($"{baseUri}/{file}");
            var hour = int.Parse(timestamp[6..8]);
            data.Add(new TimeOnly(hour, 0), usage);
        });

        return data;
    }

    private async Task Savefile(string uri, string fileName)
    {
        using var reader = GetReader(uri, WebRequestMethods.Ftp.DownloadFile);
        var text = await reader.ReadToEndAsync();
        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + fileName, text);
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

            if (line != "[wr_ende]")
            {
                lines.Add(line);
            }
        }

        var firstLineColumns = lines.First().Split(';');
        var lastLineColumns = lines.Last().Split(';');

        var firstUsageString = firstLineColumns.ElementAt(37);
        var lastUsageString = lastLineColumns.ElementAt(37);

        var firstUsage = double.Parse(firstUsageString);
        var lastUsage = double.Parse(lastUsageString);

        return lastUsage - firstUsage;
    }

    private async Task<List<string?>> GetFilesToDownload()
    {
        using var reader = GetReader(baseUri, WebRequestMethods.Ftp.ListDirectory);
        var everything = await reader.ReadToEndAsync();
        var allFiles = everything.Replace("\r", "").Split('\n');
        return allFiles.Where(file => !string.IsNullOrWhiteSpace(file)).ToList();
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