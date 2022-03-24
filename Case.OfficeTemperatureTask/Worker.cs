using Case.CoreFunctionality.Interfaces;
using Case.CoreFunctionality.Models;

namespace Case.OfficeTemperatureTask
{
    public class Worker : BackgroundService
    {
        private readonly Timer _timer;
        private readonly IRepository<RoomTemperature> _repository;

        public Worker(IRepository<RoomTemperature> repository)
        {
            _timer = new Timer(async _ => await GetLatestTemperature());
            this._repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var intervalMs = TimeSpan.FromMinutes(30).TotalMilliseconds;
            _timer.Change(0, (int)intervalMs);
        }

        private async Task GetLatestTemperature()
        {
            var temperatures = await _repository.GetLast24HoursAsync();
            var todaysTemperatures = temperatures.Where(temperature => temperature.Time > DateTime.Now.AddDays(-1));

            // TODO: Do something with temps.... put on queue, cache, etc?? 
        }
    }
}