namespace Case.ElectricityTask.Services.Implementations;

public class SolarPanelEfficiencyService : ISolarPanelEfficiencyService
{
    public async Task<int> GetLastHoursEfficiencyAsync(CancellationToken token = default)
    {
        return 300;
    }
}