# Solution Sketch: Event-Based Version

This file is intentionally only a sketch. Students should first refactor the linear implementation themselves.

## Delegate stage

```csharp
public delegate void ProgressReporter(string stepName, int percent);
```

The process simulation can accept a `ProgressReporter` parameter and call it instead of writing directly to the console.

```csharp
private static void RunProcessStep(string stepName, ProgressReporter reportProgress)
{
    for (int percent = 0; percent <= 100; percent += 5)
    {
        reportProgress(stepName, percent);
        Thread.Sleep(80);
    }
}
```

Multiple methods can be attached:

```csharp
ProgressReporter reporter = DrawProgressBar;
reporter += WarnAtHalfway;
reporter += LogProgress;
```

## Event-based structure

Possible classes:

- `ProcessRunner`
- `ProgressChangedEventArgs`
- `ProcessStepEventArgs`
- `ConsoleProgressView`
- `ProcessLogger`
- `ProcessWarningSystem`

## Event argument examples

```csharp
public class ProgressChangedEventArgs : EventArgs
{
    public string StepName { get; }
    public int Percent { get; }

    public ProgressChangedEventArgs(string stepName, int percent)
    {
        StepName = stepName;
        Percent = percent;
    }
}
```

```csharp
public class ProcessStepEventArgs : EventArgs
{
    public string StepName { get; }

    public ProcessStepEventArgs(string stepName)
    {
        StepName = stepName;
    }
}
```

## ProcessRunner example

```csharp
public class ProcessRunner
{
    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;
    public event EventHandler<ProcessStepEventArgs>? StepStarted;
    public event EventHandler<ProcessStepEventArgs>? StepCompleted;
    public event EventHandler? ProcessCompleted;

    public void Run(string[] steps)
    {
        foreach (string step in steps)
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
```
