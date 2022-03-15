// See https://aka.ms/new-console-template for more information

using Case.CoreFunctionality;
using Case.CoreFunctionality.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddAllCaseFunctionality();

var serviceProvider = services.BuildServiceProvider();

var ftpstuff = serviceProvider.GetService<ISolarPanelEfficiencyService>();

var data = await ftpstuff.GetEfficiencyForTodayAsync();

Console.WriteLine("Hello, World!");
