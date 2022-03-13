namespace Case.ElectricityTask.Services;

public interface ISolarPanelEfficiencyService
{
    Task<int> GetLastHoursEfficiencyAsync(CancellationToken token = default);
}