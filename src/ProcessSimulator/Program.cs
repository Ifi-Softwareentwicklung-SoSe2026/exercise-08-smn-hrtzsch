using System;
using System.Threading;

namespace ProcessSimulator;

public sealed class ProgressChangedEventArgs : EventArgs
{
    public ProgressChangedEventArgs(string stepName, int percent)
    {
        StepName = stepName;
        Percent = percent;
    }

    public string StepName { get; }

    public int Percent { get; }
}

internal sealed class ProcessRunner
{
    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

    public void Run(string[] steps)
    {
        foreach (string step in steps)
        {
            Console.WriteLine($"Starting: {step}");

            for (int percent = 0; percent <= 100; percent += 5)
            {
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(step, percent));

                Thread.Sleep(80);
            }

            Console.WriteLine($"Completed: {step}");
            Console.WriteLine();
        }

        Console.WriteLine("All process steps completed.");
    }
}

internal class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;
        Console.WriteLine("=== Process Simulator ===");
        Console.WriteLine();

        string[] steps =
        {
            "Downloading data",
            "Validating input",
            "Processing records",
            "Generating report",
            "Publishing results",
            "Cleaning up"
        };

        var runner = new ProcessRunner();
        runner.ProgressChanged += DrawProgressBar;
        runner.ProgressChanged += ShowHalfwayWarning;

        runner.Run(steps);

        Console.CursorVisible = true;
    }

    private static void DrawProgressBar(object? sender, ProgressChangedEventArgs e)
    {
        const int width = 30;
        const char filledChar = '█';
        const char emptyChar = '░';
        const char barStartChar = '⟦';
        const char barEndChar = '⟧';

        int filled = e.Percent * width / 100;

        string bar = new string(filledChar, filled) + new string(emptyChar, width - filled);
        Console.Write($"\r{e.StepName,-22} {barStartChar}{bar}{barEndChar} {e.Percent,3}%");

        if (e.Percent == 100)
        {
            Console.WriteLine();
        }
    }

    private static void ShowHalfwayWarning(object? sender, ProgressChangedEventArgs e)
    {
        if (e.Percent == 50)
        {
            Console.WriteLine($"  Warning: {e.StepName} is only halfway done.");
        }
    }
}
