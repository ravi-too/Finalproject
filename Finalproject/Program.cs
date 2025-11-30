using System;

namespace ConsoleCalculator
{

    public class CalculatorService
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("nulze gagopa daushvebelia!");

            return a / b;
        }
    }

   
    public static class InputValidator
    {
        public static bool TryReadNumber(string input, out double number)
        {
            return double.TryParse(input, out number);
        }

        public static bool IsValidOperation(string op)
        {
            return op == "+" || op == "-" || op == "*" || op == "/";
        }
    }

   
    class Program
    {
        static void Main(string[] args)
        {
            CalculatorService calculator = new CalculatorService();

            while (true)
            {
                Console.WriteLine("\n--- konsoluri kalkulatori ---");

               
                Console.Write("sheiyvanet pirveli ricxvi: ");
                string firstInput = Console.ReadLine();
                if (!InputValidator.TryReadNumber(firstInput, out double num1))
                {
                    Console.WriteLine("shecdoma! gtqovt sheiyvanot swori ricxvi.");
                    continue;
                }

              
                Console.Write("sheiyvanet meore ricxvi: ");
                string secondInput = Console.ReadLine();
                if (!InputValidator.TryReadNumber(secondInput, out double num2))
                {
                    Console.WriteLine("shecdoma! gtqovt sheiyvanot swori ricxvi.");
                    continue;
                }

                Console.Write("airchiet operacia (+, -, *, /): ");
                string op = Console.ReadLine();

                if (!InputValidator.IsValidOperation(op))
                {
                    Console.WriteLine("arswori operacia!");
                    continue;
                }

                try
                {
                    double result = 0;

                    switch (op)
                    {
                        case "+": result = calculator.Add(num1, num2); break;
                        case "-": result = calculator.Subtract(num1, num2); break;
                        case "*": result = calculator.Multiply(num1, num2); break;
                        case "/": result = calculator.Divide(num1, num2); break;
                    }

                    Console.WriteLine($"shedegi: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"shecdoma: {ex.Message}");
                }

                Console.WriteLine("gsurt gagrdzeleba? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }
        }
    }
}
