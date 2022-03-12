using Ductus.FluentDocker.Builders;

// TODO: Compose docker with file from Solution Items

var svc = new Builder()
    .UseContainer()
    .UseCompose()
    .FromFile("./docker-compose.yml")
    .RemoveOrphans()
    .Build().Start();

while(Console.ReadLine() != "q")
{

}

svc.Stop();