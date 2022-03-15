namespace Case.CoreFunctionality.Interfaces;

public interface ISolarPanelEfficiencyService
{
    Task<Dictionary<TimeOnly, double>> GetEfficiencyForTodayAsync(CancellationToken token = default);
}