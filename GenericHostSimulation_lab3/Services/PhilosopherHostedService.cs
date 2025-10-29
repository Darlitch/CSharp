using System.Diagnostics;
using IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Model;
using Model.Enums;
using StrategyInterface;

namespace Services;

public abstract class PhilosopherHostedService : BackgroundService, IPhilosopher
{
    public int Index { get; }
    public string Name { get; }
    public PhilosopherMetrics Metrics { get; }
    public int CurrentActionDuration { get; private set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }
    private Fork LeftFork { get; }
    private Fork RightFork { get; }
    private readonly Stopwatch _stopwatchWait;
    private readonly Stopwatch _stopwatch;
    private readonly IPhilosopherStrategy _strategy;
    private readonly IOptions<SimulationOptions> _options;

    protected PhilosopherHostedService(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options, int ind, string name)
    {
        Index = ind;
        Name = name;
        Metrics = new PhilosopherMetrics();
        LeftFork = tableManager.GetFork(ind);
        RightFork = tableManager.GetFork(ind + 1);
        _strategy = strategy;
        _options = options;
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
        SetState(PhilosopherState.Thinking, new Random().Next(_options.Value.ThinkingTimeMin, _options.Value.ThinkingTimeMax));
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
        SetState(PhilosopherState.Eating, new Random().Next(_options.Value.EatingTimeMin,_options.Value.EatingTimeMax));
        Metrics.IncrementEaten();
    }

    private void TakeLeftFork()
    {
        Action = PhilosopherAction.TakeLeftFork;
        LeftFork.TryTakeFork(Name);
        CurrentActionDuration = _options.Value.ForkAcquisitionTime;
        _stopwatch.Restart();
    }
    
    private void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        RightFork.TryTakeFork(Name);
        CurrentActionDuration = _options.Value.ForkAcquisitionTime;
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

    private void Update()
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
    
    private void HandleAction(PhilosopherAction action)
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
        while (!stoppingToken.IsCancellationRequested)
        {
            Update();
            if (IsHungry && Action == PhilosopherAction.None)
            {
                HandleAction(_strategy.SelectAction(Name, LeftFork, RightFork));
            }
            await Task.Delay(10, stoppingToken);
        }
    }
}