using Case.ElectricityTask;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ElectricityTask>();
    })
    .Build();

await host.RunAsync();
