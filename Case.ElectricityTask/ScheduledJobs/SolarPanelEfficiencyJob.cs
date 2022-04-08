namespace Case.ElectricityService.ScheduledJobs;

public class SolarPanelEfficiencyJob : IJob
{
    private readonly ISolarPanelEfficiencyService _efficiencyService;

    public SolarPanelEfficiencyJob(ISolarPanelEfficiencyService efficiencyService)
    {
        _efficiencyService = efficiencyService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Log.Information("Starting: {job}", nameof(SolarPanelEfficiencyJob));
    }
}
