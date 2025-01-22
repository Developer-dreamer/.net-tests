using System.Diagnostics;
using NUnit.Framework;

namespace CalculatorTests;

[TestFixture]
public class Tests
{
    private static readonly string AppPath = Path.Combine(
        Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
        "Calculator/bin/Debug/net9.0/Calculator"
    );
    
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

    [Test]
    public async Task HasTokenization()
    {
        string output = await RunCalculatorAsync("3 + 5 * 4 - 6 / 3");
        
        Assert.That(output, Does.Contain("Tokens: 3, +, 5, *, 4, -, 6, /, 3"));
    }
    
    [Test]
    public async Task HasRpn()
    {
        string output = await RunCalculatorAsync("3 + 5 * 4 - 6 / 3");
        
        Assert.That(output, Does.Contain("Reversed polish notation: 3, 5, 4, *, +, 6, 3, /, -"));
    }

    [Test]
    public async Task BasicExpression()
    {
        string output = await RunCalculatorAsync("3 + 5 * 4 - 6 / 3");

        Assert.That(output, Does.Contain("Tokens: 3, +, 5, *, 4, -, 6, /, 3"));
        Assert.That(output, Does.Contain("Reversed polish notation: 3, 5, 4, *, +, 6, 3, /, -"));
        Assert.That(output, Does.Contain("Result: 21"));
    }
    
    [Test]
    public async Task StartUnaryMinus()
    {
        string output = await RunCalculatorAsync("- 3 + 5 * 4 - 6 / 3");
        
        Assert.That(output, Does.Contain("Tokens: 0, -, 3, +, 5, *, 4, -, 6, /, 3"));
        Assert.That(output, Does.Contain("Reversed polish notation: 0, 3, -, 5, 4, *, +, 6, 3, /, -"));
        Assert.That(output, Does.Contain("Result: 15"));
    }

    [Test]
    public async Task ExpressionWithBraces()
    {
        string output = await RunCalculatorAsync("3 + 5 * (2 - 8 / (4 - 2))");
        
        Assert.That(output, Does.Contain("Tokens: 3, +, 5, *, (, 2, -, 8, /, (, 4, -, 2, ), )"));
        Assert.That(output, Does.Contain("Reversed polish notation: 3, 5, 2, 8, 4, 2, -, /, -, *, +"));
        Assert.That(output, Does.Contain("Result: -7"));
    }
    
}
