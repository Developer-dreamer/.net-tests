using System.Globalization;

namespace Calculator.Processors;

public static class Helpers
{
    private static readonly Dictionary<char, int> Operators = new()
    {
        {'(', 0},
        {')', 0},
        {'+', 1},
        {'-', 1},
        {'*', 2},
        {'/', 2},
        {'^', 3}
    };

    private static readonly Dictionary<string, int> Functions = new()
    {
        {"sin", 3},
        {"cos", 3}
    };

    public static bool IsOperator(this char @char) => Operators.ContainsKey(@char);
    public static bool IsOperator(this string token) => token.Length is 1 && Operators.ContainsKey(token[0]);
    public static bool IsFunc(this string token) => token is "sin" or "cos";
    
    public static bool IsNumber(this string token)
    {
        var culture = new CultureInfo("en-US")
        {
            NumberFormat =
            {
                NumberDecimalSeparator = ",",
                NumberGroupSeparator = "."
            }
        };

        return decimal.TryParse(token, NumberStyles.Number, culture, out _);
    }

    public static bool ComparePriority(string operator1,string token)
    {
        if (operator1.IsFunc() != token.IsFunc())
        {
            if (operator1.IsFunc())
                return Functions[operator1] >= Operators[char.Parse(token)];
            return Operators[char.Parse(operator1)] >= Functions[token];
        }
    
        return operator1.IsFunc() ? Functions[operator1] >= Functions[token] : Operators[char.Parse(operator1)] >= Operators[char.Parse(token)];

    }
}