using Confluent.Kafka;

namespace Case.ElectricityService;

public class SolarPanelEfficiencyProducer : BackgroundService
{
    private const string TOPIC = "solar_panel_production";
    private static readonly int TIMER_DUETIME = (int)TimeSpan.FromHours(1).TotalMilliseconds;
    private static readonly Dictionary<string, string> configuration = new()
    {
        { "bootstrap.servers", "localhost:9092" }
    };
    private readonly ISolarPanelEfficiencyService _service;
    private readonly ISerializer<TimeOnly> _serializer;
    private IProducer<TimeOnly, double> _producer;
    private readonly ProducerBuilder<TimeOnly, double> _builder;
    private readonly Timer _timer;

    public SolarPanelEfficiencyProducer(ISolarPanelEfficiencyService service, ISerializer<TimeOnly> serializer)
    {
        _builder = new ProducerBuilder<TimeOnly, double>(configuration);
        _timer = new Timer(async (_) => await GetLatestProduction());
        _service = service;
        _serializer = serializer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Initialize();
        // After analyzing how this runs, you could set the delay time to make sure it runs at intervals/times closest to the newest data. 
        _timer.Change(0, TIMER_DUETIME);
        return Task.CompletedTask;
    }

    private void Initialize()
    {
        _builder.SetKeySerializer(_serializer);
        _producer = _builder.Build();
    }

    private async Task GetLatestProduction()
    {
        var todaysProduction = await _service.GetEfficiencyForTodayAsync();
        var now = DateTime.Now;

        KeyValuePair<TimeOnly, double> lastHour;
        try
        {
            lastHour = todaysProduction.SingleOrDefault(production => production.Key.Hour == now.Hour);
        }
        catch (InvalidOperationException)
        {
            return;
        }

        var message = new Message<TimeOnly, double>
        {
            Key = lastHour.Key,
            Value = lastHour.Value
        };
        await _producer.ProduceAsync(TOPIC, message);
    }
}
