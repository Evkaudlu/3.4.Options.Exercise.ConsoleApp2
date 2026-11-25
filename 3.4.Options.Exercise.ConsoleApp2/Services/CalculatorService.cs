using Microsoft.Extensions.Options;
using OptionsLab.Options;

namespace OptionsLab.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IOptionsMonitor<CalculatorOptions> _options;

        public CalculatorService(
            IOptionsMonitor<CalculatorOptions> options)
        {
            _options = options;
        }

        public double Calculate(double a, double b,
            string? operation = null)
        {
            string op = operation ??
                _options.CurrentValue.DefaultOperation;

            return op.ToLowerInvariant() switch
            {
                "add" => a + b,

                "subtract" => a - b,

                "multiply" => a * b,

                "divide" => b == 0
                    ? throw new DivideByZeroException(
                        "Ділення на нуль неможливе!")
                    : a / b,

                _ => throw new ArgumentException(
                    $"Невідома операція: {op}")
            };
        }
    }
}