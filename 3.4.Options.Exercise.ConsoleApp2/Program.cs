using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OptionsLab.Options;
using OptionsLab.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Завантаження конфігурації
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json",
        optional: false,
        reloadOnChange: true)
    .Build();

// Реєстрація сервісів
var services = new ServiceCollection();

services.Configure<CalculatorOptions>(
    configuration.GetSection("CalculatorOptions"));

services.AddSingleton<ICalculatorService, CalculatorService>();

using var serviceProvider = services.BuildServiceProvider();

var calculator = serviceProvider
    .GetRequiredService<ICalculatorService>();

var options = serviceProvider
    .GetRequiredService<IOptionsMonitor<CalculatorOptions>>();

// Відстеження змін конфігурації
using var subscription = options.OnChange(settings =>
{
    Console.WriteLine(
        $"\nКонфігурацію змінено! Нова операція: " +
        settings.DefaultOperation);
});

// Початкові обчислення
double a = 20;
double b = 5;

Console.WriteLine("=== Лабораторна робота №4 ===");
Console.WriteLine("Виконала: Удалих Єва");
Console.WriteLine();

Console.WriteLine($"Перше число: {a}");
Console.WriteLine($"Друге число: {b}");

Console.WriteLine($"Додавання: {calculator.Calculate(a, b, "Add")}");
Console.WriteLine($"Віднімання: {calculator.Calculate(a, b, "Subtract")}");
Console.WriteLine($"Множення: {calculator.Calculate(a, b, "Multiply")}");
Console.WriteLine($"Ділення: {calculator.Calculate(a, b, "Divide")}");

// Перевірка ділення на нуль
try
{
    calculator.Calculate(a, 0, "Divide");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}

// Повторне виконання обчислень
while (true)
{
    Console.WriteLine();
    Console.WriteLine(
        $"Поточна операція: {options.CurrentValue.DefaultOperation}");

    try
    {
        Console.WriteLine(
            $"Результат: {calculator.Calculate(a, b)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("Enter - повторити обчислення");
    Console.WriteLine("exit - завершити програму");

    if (Console.ReadLine() == "exit")
        break;
}