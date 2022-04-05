using Confluent.Kafka;

namespace Case.ElectricityTask.ScheduledJobs;

public class ElectricityPriceJob : IJob
{
    private readonly IElectricityPriceService _priceService;
    private readonly IProducer<string, string> producer;

    public ElectricityPriceJob(IElectricityPriceService priceService, IConfiguration configuration)
    {
        _priceService = priceService;
        producer = new ProducerBuilder<string, string>(configuration.AsEnumerable()).Build();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Log.Information("Starting: {job}", nameof(ElectricityPriceJob));
    }
}
