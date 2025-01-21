using System.Text;
using Calculator.Collections;

namespace Calculator.Processors;

public static class Tokenizator
{
    private static readonly List<string> Tokens = [];

    public static List<string> Tokenize(ref string? expression)
    {
        var currentToken = new StringBuilder();

        if (expression == null)
            throw new Exception("Expression must not be empty");
        
        if (expression[0] == '-' || expression[0] == '+')
        {
            expression = "0" + expression;
        }
        
        foreach (var token in expression)
        {
            switch (token) {
                case >= '0' and <= '9' or '.' or ',':
                    currentToken.Append(token);
                    break;
                case '+' or '-' when currentToken.Length == 0 && Tokens is [] or [.., "(" or "+" or "-" or "*" or "/"]:
                    // Unary minus.
                    currentToken.Append(token); 
                    break;
                case var _ when token.IsOperator():
                    // Operator
                    AppendNumber(ref currentToken);
                    Tokens.Add(token.ToString());
                    break;
                case var _ when char.IsLetter(token):
                    // Part of operator (sin, cos, etc.)
                    currentToken.Append(token);
                    break;
                case ' ':
                    AppendNumber(ref currentToken);
                    break;
                default:
                    throw new Exception("Unknown character");
            }
        }
        AppendNumber(ref currentToken);

        return Tokens;
    }

    private static void AppendNumber(ref StringBuilder currentToken)
    {
        if (currentToken.Length <= 0) return;
        
        Tokens.Add(currentToken.ToString());
        currentToken.Clear();
    }
}