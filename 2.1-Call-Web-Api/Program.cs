// See https://aka.ms/new-console-template for more information

using System.Xml;

var client = new HttpClient();

var response = await client.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

var content = await response.Content.ReadAsStringAsync();

var document = new XmlDocument();

document.LoadXml(content);

Console.WriteLine($"endpoint returned: {content}");
Console.ReadLine();
