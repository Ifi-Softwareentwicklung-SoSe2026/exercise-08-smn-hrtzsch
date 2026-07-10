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

        SimulateProcess(steps, progressReporter);

        Console.WriteLine("All process steps completed.");
        Console.CursorVisible = true;
    }

    private static void SimulateProcess(string[] steps, ProgressReporter progressReporter)
    {
        foreach (string step in steps)
        {
            for (int percent = 0; percent <= 100; percent += 5)
            {
                progressReporter(step, percent);

                Thread.Sleep(80);
            }
        }
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
