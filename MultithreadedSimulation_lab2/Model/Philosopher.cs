using System.Diagnostics;
using Model.Enums;

namespace Model;

public class Philosopher
{
    public string Name { get; }
    public PhilosopherMetrics Metrics { get; }
    public int CurrentActionDuration { get; private set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }
    public Fork LeftFork { get; }
    public Fork RightFork { get; }
    private readonly Stopwatch _stopwatchWait;
    private readonly Stopwatch _stopwatch;

    public Philosopher(string name, Fork leftFork, Fork rightFork)
    {
        Name = name;
        Metrics = new PhilosopherMetrics();
        LeftFork = leftFork;
        RightFork = rightFork;
        _stopwatchWait = new Stopwatch();
        _stopwatch = Stopwatch.StartNew();
        StartThinking();
    }

    private void SetState(PhilosopherState state, int duration)
    {
        State = state;
        CurrentActionDuration = duration;
        Action = PhilosopherAction.None;
        _stopwatch.Restart();
    }

    private void StartThinking()
    {
        SetState(PhilosopherState.Thinking, new Random().Next(30, 100));
        // SetState(PhilosopherState.Thinking, 100);
    }

    private void SetHungry()
    {
        SetState(PhilosopherState.Hungry, 0);
        _stopwatchWait.Restart();
    }

    private void StartEating()
    {
        _stopwatchWait.Stop();
        Metrics.WaitingTime += _stopwatchWait.ElapsedMilliseconds;
        SetState(PhilosopherState.Eating, new Random().Next(40, 50));
        Metrics.IncrementEaten();
    }

    private void TakeLeftFork()
    {
        Action = PhilosopherAction.TakeLeftFork;
        LeftFork.TakeFork(Name);
        CurrentActionDuration = 20;
        _stopwatch.Restart();
    }
    
    private void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        RightFork.TakeFork(Name);
        CurrentActionDuration = 20;
        _stopwatch.Restart();
    }

    private void ReleaseForks()
    {
        LeftFork.ReleaseFork();
        RightFork.ReleaseFork();
    }

    private void ReleaseLeftFork()
    {
        LeftFork.ReleaseFork();
    }

    public void Update()
    {
        if (_stopwatch.ElapsedMilliseconds < CurrentActionDuration) return;
        switch (State)
        {
            case PhilosopherState.Thinking:
                SetHungry();
                break;
            case PhilosopherState.Eating:
                StartThinking();
                ReleaseForks();
                break;
            case PhilosopherState.Hungry:
                if (LeftFork.Owner == Name && RightFork.Owner == Name)
                {
                    StartEating();
                }
                break;
            default:
                throw new InvalidOperationException($"Неизвестное состояние философа: {State}");
        }

        if (Action is PhilosopherAction.TakeLeftFork or PhilosopherAction.TakeRightFork)
        {
            Action = PhilosopherAction.None;
        }
    }
    
    public void HandleAction(PhilosopherAction action)
    {
        switch (action)
        {
            case PhilosopherAction.TakeLeftFork:
                TakeLeftFork();
                break;
            case PhilosopherAction.TakeRightFork:
                TakeRightFork();
                break;
            case PhilosopherAction.ReleaseLeftFork:
                ReleaseLeftFork();
                Action = PhilosopherAction.None;
                CurrentActionDuration = 0;
                break;
            case PhilosopherAction.None:
                break;
            default:
                throw new InvalidOperationException($"Неизвестное действие философа: {action}");
        }
    }
    
    public bool IsHungry => State == PhilosopherState.Hungry;
}