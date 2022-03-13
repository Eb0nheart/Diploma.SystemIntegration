namespace Case.CoreFunctionality.Interfaces;

public interface ISolarPanelEfficiencyService
{
    Task<int> GetLastHoursEfficiencyAsync(CancellationToken token = default);
}