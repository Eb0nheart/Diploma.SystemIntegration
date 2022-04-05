using Confluent.Kafka;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Case.CoreFunctionality.Implementations;
public class ElectricityPriceService : IElectricityPriceService
{
    private const string GET_PRICE_TOPIC = "get-price";
    private const string RECEIVE_PRICE_TOPIC = "receive-price";
    private const string PRICE_KEY = "west-prices";
    private readonly IMemoryCache _cache;
    private readonly HttpClient _client;
    private readonly IProducer<DateTime, double> _producer;
    private readonly IConsumer<Ignore, DateTime> _consumer;

    public ElectricityPriceService(IMemoryCache cache, IConfiguration configuration, HttpClient client)
    {
        _cache = cache;
        _client = client;
        _producer = new ProducerBuilder<DateTime, double>(configuration.AsEnumerable()).Build();
        _consumer = new ConsumerBuilder<Ignore, DateTime>(configuration.AsEnumerable()).Build();
    }

    public async Task ListenForPriceRequests(CancellationToken token)
    {
        _consumer.Subscribe(GET_PRICE_TOPIC);

        while (!token.IsCancellationRequested)
        {
            var result = _consumer.Consume(token);
            token.ThrowIfCancellationRequested(); // Vær sikker på at vi ikke fortsætter hvis en cancel event er triggered.
            var timeToGetPriceFor = result.Message.Value;
            var price = await GetPriceAsync(timeToGetPriceFor, token);
            var message = new Message<DateTime, double>()
            {
                Key = timeToGetPriceFor,
                Value = price
            };
            await _producer.ProduceAsync(RECEIVE_PRICE_TOPIC, message);
        }
    }

    private async Task<double> GetPriceAsync(DateTime timeOfPrice, CancellationToken token = default)
    {
        var newestPrices = await GetNewestWestPricesAsync();

        var price = newestPrices.Single(price => price.HourDK == timeOfPrice);

        return price.SpotPriceDKK;
    }

    private async Task<IEnumerable<ElectricityPriceDTO>> GetNewestWestPricesAsync(CancellationToken token = default)
    {
        var cacheOptions = new MemoryCacheEntryOptions();

        var getDataFromProvider = async () =>
        {
            string payload = @"{  ""operationName"": ""Dataset"",  ""variables"": {},  ""query"": ""  query Dataset { elspotprices (order_by:{HourUTC:desc},limit:500,offset:0)  { HourUTC,HourDK,PriceArea,SpotPriceDKK,SpotPriceEUR } }""   }";
            var response = await _client.PostAsync("https://data-api.energidataservice.dk/v1/graphql", new StringContent(payload, Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadFromJsonAsync<ElectricityPriceDataDTO>();
            var westPrices = result?.Data?.ElspotPrices.Where(price => price.PriceArea.ToLowerInvariant() == "dk1");
            cacheOptions.AbsoluteExpiration = westPrices.Max(price => price.HourDK);
            return westPrices;
        };

        return await _cache.GetData(PRICE_KEY, cacheOptions, getDataFromProvider);
    }
}
