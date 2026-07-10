using System;
using System.Threading;

namespace ProcessSimulator;

public delegate void ProgressReporter(string stepName, int percent);

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

        ProgressReporter progressReporter = DrawProgressBar;
        progressReporter += ShowHalfwayWarning;

        var runner = new ProcessRunner(steps);
        runner.Run(progressReporter);

        Console.WriteLine("All process steps completed.");
        Console.CursorVisible = true;
    }

    private static void DrawProgressBar(string stepName, int percent)
    {
        if (percent == 0)
        {
            Console.WriteLine($"Starting: {stepName}");
        }

        const int width = 30;
        const char filledChar = '█';
        const char emptyChar = '░';
        const char barStartChar = '⟦';
        const char barEndChar = '⟧';

        int filled = percent * width / 100;

        string bar = new string(filledChar, filled) + new string(emptyChar, width - filled);
        Console.Write($"\r{stepName,-22} {barStartChar}{bar}{barEndChar} {percent,3}%");

        if (percent == 100)
        {
            Console.WriteLine();
            Console.WriteLine($"Completed: {stepName}");
            Console.WriteLine();
        }
    }

    private static void ShowHalfwayWarning(string stepName, int percent)
    {
        if (percent != 50)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Warning: {stepName} is only halfway done.");
    }
}

internal sealed class ProcessRunner
{
    private readonly string[] _steps;

    public ProcessRunner(string[] steps)
    {
        _steps = steps;
    }

    public void Run(ProgressReporter progressReporter)
    {
        foreach (string step in _steps)
        {
            for (int percent = 0; percent <= 100; percent += 5)
            {
                progressReporter(step, percent);

                Thread.Sleep(80);
            }
        }
    }
}
