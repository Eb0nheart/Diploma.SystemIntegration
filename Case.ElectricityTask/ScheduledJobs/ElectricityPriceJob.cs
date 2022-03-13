namespace Case.ElectricityTask.ScheduledJobs;

public class ElectricityPriceJob : IJob
{
    private readonly IElectricityPriceService _priceService;

    public ElectricityPriceJob(IElectricityPriceService priceService)
    {
        _priceService = priceService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Log.Information("Starting: {job}", nameof(ElectricityPriceJob));
    }
}
