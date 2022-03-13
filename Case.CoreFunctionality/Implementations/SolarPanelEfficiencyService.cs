
namespace Case.CoreFunctionality.Implementations;

public class SolarPanelEfficiencyService : ISolarPanelEfficiencyService
{
    public async Task<int> GetLastHoursEfficiencyAsync(CancellationToken token = default)
    {
        return 300;
    }
}