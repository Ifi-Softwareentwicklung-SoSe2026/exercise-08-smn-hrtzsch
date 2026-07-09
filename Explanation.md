# Exercise: From Linear Code to Delegates and Events in C#

## Context

In this exercise, you will refactor a small console application that simulates a running process. The current implementation is intentionally linear: the process logic, console output, warning logic, and completion message are all mixed together.

Your task is to improve the design by introducing **delegates** and then **events**.

The example uses progress bars to make the process visible in the terminal.

## Learning goals

After completing this exercise, you should be able to:

- explain why tightly coupled linear code becomes hard to extend,
- define and use a delegate,
- subscribe methods to a delegate or event,
- raise events from a class,
- separate process logic from UI/output logic.

## Starting point

Open the project in `ProcessSimulator` and inspect `Program.cs`.

Run the program:

```bash
dotnet run
```

You should see several simulated process steps with progress bars.

## Task 1: Understand the linear implementation

Before changing the code, answer briefly:

1. Which parts of the code belong to the process logic?
2. Which parts belong to console visualization?
3. Which parts would be hard to reuse in a GUI or web application?
4. What would you need to change if you wanted to add logging?

## Task 2: Introduce a delegate

Create a delegate that can be used to report progress.

Suggested signature:

```csharp
public delegate void ProgressReporter(string stepName, int percent);
```

Then change the process simulation so that it no longer prints the progress directly. Instead, it should call a delegate whenever progress changes.

## Task 3: Add multiple subscribers

Use the delegate to call more than one method:

- one method should draw the progress bar,
- another method should print a warning when progress reaches 50%,
- another optional method may log progress messages.

## Task 4: Refactor to events

Create a `ProcessRunner` class that exposes events such as:

```csharp
public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;
public event EventHandler<ProcessStepEventArgs>? StepStarted;
public event EventHandler<ProcessStepEventArgs>? StepCompleted;
public event EventHandler? ProcessCompleted;
```

The `ProcessRunner` should not know anything about the console UI.

## Task 5: Extend the system

Add at least two new event subscribers, for example:

- a logger,
- a sound/beep notification,
- a statistics collector,
- a warning system for long-running steps,
- a second progress visualization.

## Expected result

At the end, your program should still simulate the same process, but the responsibilities should be separated:

- `ProcessRunner` controls the simulated process,
- event argument classes describe what happened,
- UI/output classes react to events,
- new behavior can be added without changing the process logic.

## Reflection questions

1. Why are events better than direct method calls in this example?
2. What is the difference between a delegate and an event?
3. Why should the process runner not directly write to the console?
4. Where else in software development are event-based designs useful?
