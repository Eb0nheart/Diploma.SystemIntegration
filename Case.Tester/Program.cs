// See https://aka.ms/new-console-template for more information

using Case.CoreFunctionality;
using Case.CoreFunctionality.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddAllCaseFunctionality();

var serviceProvider = services.BuildServiceProvider();

var testee = serviceProvider.GetService<IWeatherFilterService>();

var data = await testee.GetWeatherDataForKoldingAsync();

Console.WriteLine("Hello, World!");
