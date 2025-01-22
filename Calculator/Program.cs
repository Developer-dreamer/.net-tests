using Calculator.Processors;

namespace Calculator;

public class Calculator
{
    public static void Main()
    {
        Console.Write("Enter your expression: ");
        string ?expression = Console.ReadLine();

        List<string> tokens = Tokenizator.Tokenize(ref expression);
        Console.WriteLine($"Tokens: {string.Join(", ", tokens)}");

        var algorithm = new ShuntingYard();
        Queue<string> translatedNotation = algorithm.Convert(ref tokens);
        Console.WriteLine($"Reversed polish notation: {string.Join(", ", translatedNotation)}");

        decimal result = Computer.Evaluate(ref translatedNotation);
        Console.WriteLine($"Result: {result}");
    }
}   