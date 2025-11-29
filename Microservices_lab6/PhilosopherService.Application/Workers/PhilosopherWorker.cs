using System.Diagnostics;
using Contract.Dtos;
using Contract.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PhilosopherService.Application.Abstractions;
using PhilosopherService.Application.Configurations;
using PhilosopherService.Application.Extensions;
using PhilosopherService.Domain;
using PhilosopherService.Domain.Enums;

namespace PhilosopherService.Application.Workers;

public class PhilosopherWorker(IStrategy strategy, IOptions<PhilosopherOptions> options,
    ITableServiceClient tableServiceClient) : BackgroundService
{
    private ForkState LeftFork { get; set; } = ForkState.NotInHand;
    private ForkState RightFork { get; set; } = ForkState.NotInHand;
    private readonly PhilosopherMetrics _metrics = new();
    private readonly PhilosopherOptions _options = options.Value;
    
    private readonly Stopwatch _simulationTime = new();
    private readonly Stopwatch _currActionTime = new();
    private readonly Stopwatch _waitingTime = new();
    private readonly Random _random = new();
    
    private void SetState(PhilosopherState state, int duration)
    {
        _metrics.State = state;
        _metrics.CurrentActionDuration = duration;
        _metrics.Action = PhilosopherAction.None;
        _currActionTime.Restart();
    }
    
    private void StartThinking()
    {
        SetState(PhilosopherState.Thinking, _random.Next(_options.ThinkingTimeMin, _options.ThinkingTimeMax));
        // SetState(PhilosopherState.Thinking, 100);
    }
    
    private void SetHungry()
    {
        SetState(PhilosopherState.Hungry, 0);
        _waitingTime.Restart();
    }
    
    private void StartEating()
    {
        _waitingTime.Stop(); 
        _metrics.WaitingTime += _waitingTime.ElapsedMilliseconds;
        SetState(PhilosopherState.Eating, _random.Next(_options.EatingTimeMin, _options.EatingTimeMax));
        _metrics.IncrementEaten();
    }
    
    private async Task TakeLeftFork()
    {
        _metrics.Action = PhilosopherAction.TakeLeftFork;
        var success = await tableServiceClient.TakeLeftFork(_options.Id);
        if (success.IsSuccess == false)
        {
            return;
        }
        LeftFork = ForkState.InHand;
        _metrics.CurrentActionDuration = _options.ForkAcquisitionTime;
        _currActionTime.Restart();
    }
    
    private async Task TakeRightFork()
    {
        _metrics.Action = PhilosopherAction.TakeRightFork;
        var success = await tableServiceClient.TakeRightFork(_options.Id);
        if (success.IsSuccess == false)
        {
            return;
        }
        RightFork = ForkState.InHand;
        _metrics.CurrentActionDuration = _options.ForkAcquisitionTime;
        _currActionTime.Restart();
    }
    
    private async Task ReleaseForks()
    {
        await tableServiceClient.ReleaseForks(_options.Id);
        LeftFork = ForkState.NotInHand;
        RightFork = ForkState.NotInHand;
    }
    
    private async Task Update()
    {
        if (_currActionTime.ElapsedMilliseconds < _metrics.CurrentActionDuration) return;
        switch (_metrics.State)
        {
            case PhilosopherState.Thinking:
                SetHungry();
                break;
            case PhilosopherState.Eating:
                StartThinking();
                await ReleaseForks();
                break;
            case PhilosopherState.Hungry:
                if (LeftFork == ForkState.InHand && RightFork == ForkState.InHand)
                {
                    StartEating();
                }
                break;
            default:
                throw new InvalidOperationException($"Неизвестное состояние философа: {_metrics.State}");
        }
        if (_metrics.Action is PhilosopherAction.TakeLeftFork or PhilosopherAction.TakeRightFork)
        {
            _metrics.Action = PhilosopherAction.None;
        }
    }
    
    private async Task HandleAction(PhilosopherAction action)
    {
        switch (action)
        {
            case PhilosopherAction.TakeLeftFork:
                await TakeLeftFork();
                break;
            case PhilosopherAction.TakeRightFork:
                await TakeRightFork();
                break;
            case PhilosopherAction.None:
                break;
            default:
                throw new InvalidOperationException($"Неизвестное действие философа: {action}");
        }
    }
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await tableServiceClient.Register(_options.ToRegisterPhilosopherDto(), ct);
        _simulationTime.Restart();
        StartThinking();
        var lastTime = _simulationTime.ElapsedMilliseconds;
        while (_simulationTime.ElapsedMilliseconds < _options.DurationMinutes * 60 * 1000)
        {
            Console.WriteLine(_options.DurationMinutes);
            await Update();
            if (_metrics is { State: PhilosopherState.Hungry, Action: PhilosopherAction.None })
            {
                await HandleAction(strategy.SelectAction(LeftFork, RightFork));
            }
            if (_simulationTime.ElapsedMilliseconds - lastTime >= 50)
            {
                lastTime = _simulationTime.ElapsedMilliseconds;
                await tableServiceClient.UpdateMetrics(_options.Id, _metrics.ToPhilosopherMetricsDto(), ct);
            }
            await Task.Delay(10, ct);
        }
        await tableServiceClient.Finish(_options.Id, ct);
    }
}