using System.Diagnostics;
using System.Globalization;
using NUnit.Framework;

namespace CalculatorTests;

[TestFixture]
public class Tests
{
    private static readonly string AppPath = Path.Combine(
        Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
        "Calculator/bin/Debug/net9.0/Calculator"
    );

    private static IEnumerable<object[]> _tokenizationTestCases = new List<object[]>
    {
        new object[] { "3", "3" },
        new object[] { "3+5", "3, +, 5" },
        new object[] { "3 +    5 ", "3, +, 5" },
        new object[] { "3 + 5 + 9", "3, +, 5, +, 9" },
        new object[] { "7 * 8 + 3 * 6", "7, *, 8, +, 3, *, 6" },
        new object[] { "7*8+3*6", "7, *, 8, +, 3, *, 6" },
        new object[] { "3+8/2-9*3*3*3", "3, +, 8, /, 2, -, 9, *, 3, *, 3, *, 3" },
        new object[] { "7*(8+3)*6", "7, *, (, 8, +, 3, ), *, 6" },
        new object[] { " 7 *(8+ 3)  *6", "7, *, (, 8, +, 3, ), *, 6" },
        new object[] { " sin(7)", "sin, (, 7, )" },
        new object[] { "4 + sin(7)^9", "4, +, sin, (, 7, ), ^, 9" },
        new object[] { "pow(4; 3)", "pow, (, 4, ;, 3, )" }
    };
    
    private static IEnumerable<object[]> _rpnTestCases = new List<object[]>
    {
        new object[] { "3", "3" },
        new object[] { "3+5", "3, 5, +" },
        new object[] { "3 +    5 ", "3, 5, +" },
        new object[] { "3 + 5 + 9", "3, 5, +, 9, +" },
        new object[] { "7 * 8 + 3 * 6", "7, 8, *, 3, 6, *, +" },
        new object[] { "7*8+3*6", "7, 8, *, 3, 6, *, +" },
        new object[] { "3+8/2-9*3*3*3", "3, 8, 2, /, +, 9, 3, *, 3, *, 3, *, -" },
        new object[] { "7*(8+3)*6", "7, 8, 3, +, *, 6, *" },
        new object[] { " 7 *(8+ 3)  *6", "7, 8, 3, +, *, 6, *" },
        new object[] { " sin(7)", "7, sin" },
        new object[] { "4 + sin(7)^9", "4, 7, sin, 9, ^, +" },
        new object[] { "pow(4; 3)", "4, 3, pow" }
    };
    
    private static IEnumerable<object[]> _evaluationTestCases = new List<object[]>
    {
        new object[] { "3", "3" },
        new object[] { "3+5", "8" },
        new object[] { "3 +    5 ", "8" },
        new object[] { "3 + 5 + 9", "17" },
        new object[] { "7 * 8 + 3 * 6", "74" },
        new object[] { "7*8+3*6", "74" },
        new object[] { "3+8/2-9*3*3*3", "-236" },
        new object[] { "7*(8+3)*6", "462" },
        new object[] { " 7 *(8+ 3)  *6", "462" },
        new object[] { " sin(7)", "0.656986598718789" },
        new object[] { "4 + sin(7)^9", "4.0228038721672474" },
        new object[] { "pow(4; 3)", "64" }
    };
    
    private async Task<string> RunCalculatorAsync(string input)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = AppPath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.Start();

            // Provide input to the application
            await process.StandardInput.WriteLineAsync(input);
            process.StandardInput.Close(); // Important to signal input completion

            // Read output
            string output = await process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();

            return output;
        }
    }

    [Test, TestCaseSource(nameof(_tokenizationTestCases))]
    [Category("Atomic")]
    public async Task HasTokenization(string input, string expectedOutput)
    {
        string output = await RunCalculatorAsync(input);
            
        Assert.That(output, Does.Contain(expectedOutput));
    }
    
    [Test, TestCaseSource(nameof(_rpnTestCases))]
    [Category("Atomic")]
    public async Task HasRpn(string input, string expectedOutput)
    {
        string output = await RunCalculatorAsync(input);
        
        Assert.That(output, Does.Contain(expectedOutput));
    }
    
    [Test, TestCaseSource(nameof(_evaluationTestCases))]
    [Category("Atomic")]
    public async Task HasEvaluation(string input, string expectedOutput)
    {
        string output = await RunCalculatorAsync(input);
        
        Assert.That(output, Does.Contain(expectedOutput));
    }
}
