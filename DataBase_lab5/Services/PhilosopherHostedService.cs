using System.Diagnostics;
using Contract.Repositories;
using Contract.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Model;
using Model.Enums;
using StrategyInterface;

namespace Services;

public class PhilosopherHostedService(
    IPhilosopherStrategy strategy,
    ITableManager tableManager,
    IOptions<SimulationOptions> options,
    IObserver observer,
    int ind,
    string name)
    : BackgroundService
{
    public int Index { get; } = ind;
    public string Name { get; } = name;
    public PhilosopherMetrics Metrics { get; } = new();
    public int CurrentActionDuration { get; private set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }
    internal Fork LeftFork { get; } = tableManager.GetFork(ind);
    internal Fork RightFork { get; } = tableManager.GetFork(ind + 1);
    private readonly Stopwatch _stopwatchWait = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private void SetState(PhilosopherState state, int duration)
    {
        State = state;
        CurrentActionDuration = duration;
        Action = PhilosopherAction.None;
        _stopwatch.Restart();
    }

    private void StartThinking()
    {
        SetState(PhilosopherState.Thinking, new Random().Next(options.Value.ThinkingTimeMin, options.Value.ThinkingTimeMax));
        // SetState(PhilosopherState.Thinking, 100);
    }

    internal void SetHungry()
    {
        SetState(PhilosopherState.Hungry, 0);
        _stopwatchWait.Restart();
    }

    internal void StartEating()
    {
        _stopwatchWait.Stop();
        Metrics.WaitingTime += _stopwatchWait.ElapsedMilliseconds;
        SetState(PhilosopherState.Eating, new Random().Next(options.Value.EatingTimeMin,options.Value.EatingTimeMax));
        Metrics.IncrementEaten();
    }

    private void TakeLeftFork()
    {
        Action = PhilosopherAction.TakeLeftFork;
        LeftFork.TryTakeFork(Name);
        CurrentActionDuration = options.Value.ForkAcquisitionTime;
        _stopwatch.Restart();
    }
    
    private void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        RightFork.TryTakeFork(Name);
        CurrentActionDuration = options.Value.ForkAcquisitionTime;
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

    internal void Update()
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
    
    internal void HandleAction(PhilosopherAction action)
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!observer.ReadyToStart)
        {
            await Task.Delay(10, stoppingToken);
        }
        StartThinking();
        while (!stoppingToken.IsCancellationRequested)
        {
            Update();
            if (IsHungry && Action == PhilosopherAction.None)
            {
                HandleAction(strategy.SelectAction(Name, LeftFork, RightFork));
            }
            await Task.Delay(10, stoppingToken);
        }
    }
}