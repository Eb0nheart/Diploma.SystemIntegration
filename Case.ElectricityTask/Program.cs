using Case.CoreFunctionality;
using Case.ElectricityTask.ScheduledJobs;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddElectricityFunctionality();

        services.AddQuartz(configurator =>
        {
            configurator.UseMicrosoftDependencyInjectionJobFactory();

            var now = DateTime.Now;
            configurator.ScheduleJob<SolarPanelEfficiencyJob>(trigger =>
            {
                var startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour+1, 2, 0);
                trigger
                    .StartAt(startTime)
                    .WithSimpleSchedule(schedule =>
                    {
                        schedule.WithIntervalInHours(1);
                        schedule.RepeatForever();
                    });
            });

            configurator.ScheduleJob<ElectricityPriceJob>(trigger =>
            {
                var startTime = new DateTime(now.Year, now.Month, now.Day, 17, 2, 0);
                trigger
                    .StartAt(startTime)
                    .WithSimpleSchedule(schedule =>
                    {
                        schedule.WithIntervalInHours(1);
                        schedule.RepeatForever();
                    });
            });

            configurator.ScheduleJob<TestJob>(trigger => trigger.StartNow());

            configurator.InterruptJobsOnShutdown = false;
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    })
    .UseSerilog((_, configuration) =>
    {
        configuration.WriteTo.Console(Serilog.Events.LogEventLevel.Debug);
    })
    .Build();

await host.RunAsync();

class TestJob : IJob
{
    private readonly IElectricityPriceService _priceService;
    private readonly ISolarPanelEfficiencyService _efficiencyService;

    public TestJob(IElectricityPriceService priceService, ISolarPanelEfficiencyService efficiencyService)
    {
        _priceService = priceService;
        _efficiencyService = efficiencyService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var prices = await _priceService.GetNextDaysPricesAsync(context.CancellationToken);
        var efficiency = await _efficiencyService.GetEfficiencyForTodayAsync(context.CancellationToken);

        Log.Information("Got data: {el} {@prices}", efficiency, prices);
    }
}