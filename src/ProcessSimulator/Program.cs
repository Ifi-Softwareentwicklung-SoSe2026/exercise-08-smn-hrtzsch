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

public sealed class StepEventArgs : EventArgs
{
    public StepEventArgs(string stepName)
    {
        StepName = stepName;
    }

    public string StepName { get; }
}

public sealed class ProcessCompletedEventArgs : EventArgs
{
    public ProcessCompletedEventArgs(int stepCount)
    {
        StepCount = stepCount;
    }

    public int StepCount { get; }
}

internal sealed class ProcessRunner
{
    public event EventHandler<StepEventArgs>? StepStarted;

    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

    public event EventHandler<StepEventArgs>? StepCompleted;

    public event EventHandler<ProcessCompletedEventArgs>? ProcessCompleted;

    public void Run(string[] steps)
    {
        foreach (string step in steps)
        {
            StepStarted?.Invoke(this, new StepEventArgs(step));

            for (int percent = 0; percent <= 100; percent += 5)
            {
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(step, percent));

                Thread.Sleep(80);
            }

            StepCompleted?.Invoke(this, new StepEventArgs(step));
        }

        ProcessCompleted?.Invoke(this, new ProcessCompletedEventArgs(steps.Length));
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
        runner.StepStarted += ShowStepStarted;
        runner.ProgressChanged += DrawProgressBar;
        runner.ProgressChanged += ShowHalfwayWarning;
        runner.ProgressChanged += LogCompletedProgress;
        runner.StepCompleted += ShowStepCompleted;
        runner.ProcessCompleted += ShowProcessCompleted;

        runner.Run(steps);

        Console.CursorVisible = true;
    }

    private static void ShowStepStarted(object? sender, StepEventArgs e)
    {
        Console.WriteLine($"Starting: {e.StepName}");
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

    private static void ShowStepCompleted(object? sender, StepEventArgs e)
    {
        Console.WriteLine($"Completed: {e.StepName}");
        Console.WriteLine();
    }

    private static void ShowHalfwayWarning(object? sender, ProgressChangedEventArgs e)
    {
        if (e.Percent == 50)
        {
            Console.WriteLine($"  Warning: {e.StepName} is only halfway done.");
        }
    }

    private static void LogCompletedProgress(object? sender, ProgressChangedEventArgs e)
    {
        if (e.Percent == 100)
        {
            Console.WriteLine($"  Log: {e.StepName} reached 100%.");
        }
    }

    private static void ShowProcessCompleted(object? sender, ProcessCompletedEventArgs e)
    {
        Console.WriteLine($"All {e.StepCount} process steps completed.");
    }
}
