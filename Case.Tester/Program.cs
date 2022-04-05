// See https://aka.ms/new-console-template for more information

using Case.CoreFunctionality.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

var _client = new HttpClient();

string payload = @"{  ""operationName"": ""Dataset"",  ""variables"": {},  ""query"": ""  query Dataset { elspotprices (order_by:{HourUTC:desc},limit:500,offset:0)  { HourUTC,HourDK,PriceArea,SpotPriceDKK,SpotPriceEUR } }""   }";
var response = await _client.PostAsync("https://data-api.energidataservice.dk/v1/graphql", new StringContent(payload, Encoding.UTF8, "application/json"));
var result = await response.Content.ReadFromJsonAsync<ElectricityPriceDataDTO>();
var lollern = result.Data.ElspotPrices.OrderByDescending(price => price.HourDK);
var expiration = result.Data.ElspotPrices.Max(price => price.HourDK);

Console.WriteLine("Hello, World!");
