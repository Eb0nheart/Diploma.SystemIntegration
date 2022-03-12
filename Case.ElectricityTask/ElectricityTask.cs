namespace Case.ElectricityTask;

public class ElectricityTask : BackgroundService
{
    private readonly ILogger<ElectricityTask> _logger;
    private readonly ElectricityTaskOptions _options;
    private readonly Timer _timer;

    public ElectricityTask(ILogger<ElectricityTask> logger, IOptions<ElectricityTaskOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _timer = new(async _ => await TimerCallback());
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[STARTING] {serviceName}", nameof(ElectricityTask));
        _timer.Change(0, 1000);
        return Task.CompletedTask;
    }

    private async Task TimerCallback()
    {
        // TODO: ElectricityService.GetNextDayPrices();
    }
}

public class ElectricityTaskOptions
{
    public string Key => nameof(ElectricityTaskOptions);

    public int ExecutionTime { get; set; }
}