using System.Diagnostics;
using TableService.Domain.Enums;

namespace TableService.Domain;

public class Fork
{
    public string? Owner { get; private set; }
    public ForkState State { get; private set; } = ForkState.Available;
    public long FreeTime { get; private set; }
    public long BlockTime { get; private set; }
    // private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private long _lastTimeChange = 0;

    public bool TryTakeFork(string owner, long currTime)
    {
        if (State != ForkState.Available) return false;
        // _stopwatch.Stop();
        // FreeTime += _stopwatch.ElapsedMilliseconds;
        FreeTime += currTime - _lastTimeChange;
        _lastTimeChange = currTime;
        Owner = owner;
        State = ForkState.InUse;
        // _stopwatch.Restart();
        return true;
    }

    public void ReleaseFork(long currTime)
    {
        // _stopwatch.Stop();
        // BlockTime += _stopwatch.ElapsedMilliseconds;
        BlockTime += currTime - _lastTimeChange;
        _lastTimeChange = currTime;
        Owner = null;
        State = ForkState.Available;
        // _stopwatch.Restart();
    }

    public bool IsAvailable() => State == ForkState.Available;

    public bool IsOwner(string owner) => Owner == owner;
}