using System.Globalization;
using Calculator.Collections;

namespace Calculator.Processors;

public static class Computer
{
    private static readonly Stack<decimal> _numbers = new();

    public static decimal Evaluate(ref Queue<string> rpnTokens)
    {
        foreach (var token in rpnTokens)
        {
            if ( token.IsNumber())
            {
                _numbers.Push(decimal.Parse(token, CultureInfo.InvariantCulture));
            }
            else if (token.IsFunc())
            {
                decimal num = _numbers.Pop();
                decimal result = 0;

                switch (token)
                {
                    case "sin":
                        result = (decimal)Math.Sin((double)num);
                        break;
                    case "cos":
                        result = (decimal)Math.Cos((double)num);
                        break;
                }
                _numbers.Push(result);
            }
            else if (token.IsOperator())
            {
                var num2 = _numbers.Pop();
                var num1 = _numbers.Pop();

                decimal interimResult = 0;
                switch (token)
                {
                    case "+": 
                        interimResult = num1 + num2;
                        break;
                    case "-": 
                        interimResult = num1 - num2;
                        break;
                    case "*": 
                        interimResult = num1 * num2;
                        break;
                    case "/":
                        if (num2 == 0) throw new InvalidOperationException("Division by zero"); 
                        interimResult = num1 / num2;
                        break;
                    case "^":
                        interimResult = (decimal)Math.Pow((double)num1, (double)num2);
                        break;
                    default:
                        throw new Exception("Invalid operator");
                }
                
                _numbers.Push(interimResult);
            }
        }

        return _numbers.Pop();
    }
}