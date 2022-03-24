// See https://aka.ms/new-console-template for more information

using Case.CoreFunctionality;
using Case.CoreFunctionality.Interfaces;
using Case.CoreFunctionality.Models;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddAllCaseFunctionality();

var serviceProvider = services.BuildServiceProvider();

var testee = serviceProvider.GetService<IRepository<RoomTemperature>>();

var data = await testee.GetLast24HoursAsync();

Console.WriteLine("Hello, World!");
