using System.Diagnostics;
using Contract.Services;
using Contract.Services.Event;
using Contract.Services.PhilosopherMain;
using Contract.Services.Simulation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Model;
using Model.DTO;
using Model.Enums;
using Services.Simulation;
using StrategyInterface;

namespace Services.PhilosopherMain;

public class PhilosopherHostedService(
    IOptions<SimulationOptions> options,
    IPhilosopherServiceBundle services,
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
    internal Fork LeftFork { get; } = services.TableManager.GetFork(ind);
    internal Fork RightFork { get; } = services.TableManager.GetFork(ind + 1);
    private readonly Stopwatch _stopwatchWait = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private void SetState(PhilosopherState state, int duration)
    {
        State = state;
        CurrentActionDuration = duration;
        Action = PhilosopherAction.None;
        _stopwatch.Restart();
        RecordPhilosopherEvent(services.SimulationTime.CurrentTimeMs);
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
        if (!LeftFork.TryTakeFork(Name)) return;
        CurrentActionDuration = options.Value.ForkAcquisitionTime;
        _stopwatch.Restart();
        RecordLeftForkEvent(services.SimulationTime.CurrentTimeMs);
    }
    
    private void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        if (!RightFork.TryTakeFork(Name)) return;
        CurrentActionDuration = options.Value.ForkAcquisitionTime;
        _stopwatch.Restart();
        RecordRightForkEvent(services.SimulationTime.CurrentTimeMs);
    }

    private void ReleaseForks()
    {
        if (LeftFork.Owner == Name)
        {
            LeftFork.ReleaseFork();
            RecordLeftForkEvent(services.SimulationTime.CurrentTimeMs-1);
        }
        if (RightFork.Owner == Name)
        {
            RightFork.ReleaseFork();
            RecordRightForkEvent(services.SimulationTime.CurrentTimeMs-1);
        }
    }

    private void ReleaseLeftFork()
    {
        if (LeftFork.Owner == Name)
        {
            LeftFork.ReleaseFork();
            RecordLeftForkEvent(services.SimulationTime.CurrentTimeMs-1);
        }
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
        StartThinking();
        while (!stoppingToken.IsCancellationRequested)
        {
            Update();
            if (IsHungry && Action == PhilosopherAction.None)
            {
                HandleAction(services.Strategy.SelectAction(Name, LeftFork, RightFork));
            }
            await Task.Delay(10, stoppingToken);
        }
    }

    private void RecordPhilosopherEvent(long currentTime)
    {
        services.EventQueue.Enqueue(new CreatePhilosopherEventDto(Index, Name, State, Action, Metrics.Eaten, Metrics.WaitingTime, currentTime));
    }
    
    private void RecordLeftForkEvent(long currentTime)
    {
        services.EventQueue.Enqueue(new CreateForkEventDto(Index, LeftFork.Owner, LeftFork.State, currentTime));
    }
    
    private void RecordRightForkEvent(long currentTime)
    {
        services.EventQueue.Enqueue(new CreateForkEventDto((Index + 1) % options.Value.PhilosophersCount, RightFork.Owner, RightFork.State, currentTime));
    }
}