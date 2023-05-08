using Chippy8.Core;
using Chippy8.GUI;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddSingleton<IWindow, Window>();
services.AddChippy8Dependencies();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<Chip8>();

app.Boot();

Console.ReadLine();