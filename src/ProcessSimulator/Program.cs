using System;
using System.Threading;

namespace ProcessSimulator;

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

        var runner = new ProcessRunner(steps);
        runner.StepStarted += AnnounceStepStarted;
        runner.ProgressChanged += DrawProgressBar;
        runner.ProgressChanged += ShowHalfwayWarning;
        runner.StepCompleted += AnnounceStepCompleted;
        runner.ProcessCompleted += AnnounceProcessCompleted;

        runner.Run();

        Console.CursorVisible = true;
    }

    private static void AnnounceStepStarted(object? sender, ProcessStepEventArgs e)
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

    private static void ShowHalfwayWarning(object? sender, ProgressChangedEventArgs e)
    {
        if (e.Percent != 50)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Warning: {e.StepName} is only halfway done.");
    }

    private static void AnnounceStepCompleted(object? sender, ProcessStepEventArgs e)
    {
        Console.WriteLine($"Completed: {e.StepName}");
        Console.WriteLine();
    }

    private static void AnnounceProcessCompleted(object? sender, EventArgs e)
    {
        Console.WriteLine("All process steps completed.");
    }
}

internal sealed class ProcessRunner
{
    private readonly string[] _steps;

    public ProcessRunner(string[] steps)
    {
        _steps = steps;
    }

    public event EventHandler<ProcessStepEventArgs>? StepStarted;

    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

    public event EventHandler<ProcessStepEventArgs>? StepCompleted;

    public event EventHandler? ProcessCompleted;

    public void Run()
    {
        foreach (string step in _steps)
        {
            StepStarted?.Invoke(this, new ProcessStepEventArgs(step));

            for (int percent = 0; percent <= 100; percent += 5)
            {
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(step, percent));

                Thread.Sleep(80);
            }

            StepCompleted?.Invoke(this, new ProcessStepEventArgs(step));
        }

        ProcessCompleted?.Invoke(this, EventArgs.Empty);
    }
}

internal sealed class ProcessStepEventArgs : EventArgs
{
    public ProcessStepEventArgs(string stepName)
    {
        StepName = stepName;
    }

    public string StepName { get; }
}

internal sealed class ProgressChangedEventArgs : EventArgs
{
    public ProgressChangedEventArgs(string stepName, int percent)
    {
        StepName = stepName;
        Percent = percent;
    }

    public string StepName { get; }

    public int Percent { get; }
}
