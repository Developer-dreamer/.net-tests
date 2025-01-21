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
    public async Task Sum()
    {
        string output = await RunCalculatorAsync("3+5");

        // Validate output
        Assert.That(output, Does.Contain("Tokens: 3, +, 5"));
        Assert.That(output, Does.Contain("Reversed polish notation: 3, 5, +"));
        Assert.That(output, Does.Contain("Result: 8"));
    }
    
    [Test]
    public async Task Subtraction()
    {
        string output = await RunCalculatorAsync("3-5");

        // Validate output
        Assert.That(output, Does.Contain("Tokens: 3, -, 5"));
        Assert.That(output, Does.Contain("Reversed polish notation: 3, 5, -"));
        Assert.That(output, Does.Contain("Result: -2"));
    }
    
    [Test]
    public async Task Multiplication()
    {
        string output = await RunCalculatorAsync("3*5");

        // Validate output
        Assert.That(output, Does.Contain("Tokens: 3, *, 5"));
        Assert.That(output, Does.Contain("Reversed polish notation: 3, 5, *"));
        Assert.That(output, Does.Contain("Result: 15"));
    }

    [Test]
    public async Task Division()
    {
        string output = await RunCalculatorAsync("6/2");

        // Validate output
        Assert.That(output, Does.Contain("Tokens: 6, /, 2"));
        Assert.That(output, Does.Contain("Reversed polish notation: 6, 2, /"));
        Assert.That(output, Does.Contain("Result: 3"));
    }
    
}
