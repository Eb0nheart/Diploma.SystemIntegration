using Case.CoreFunctionality;
using Case.OfficeTemperatureTask;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddRoomTemperatureFunctionality();
    })
    .Build();

await host.RunAsync();
