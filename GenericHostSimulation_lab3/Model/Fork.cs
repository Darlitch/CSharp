using System.Diagnostics;
using Model.Enums;

namespace Model;

public class Fork
{
    public string? Owner { get; private set; }
    public ForkState State { get; private set; } = ForkState.Available;
    public long FreeTime { get; set; }
    public long BlockTime { get; set; }
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private readonly Lock _lock = new Lock();

    public void TryTakeFork(string owner)
    {
        using (_lock.EnterScope())
        {
            if (State != ForkState.Available) return;
            _stopwatch.Stop();
            FreeTime += _stopwatch.ElapsedMilliseconds;
            Owner = owner;
            State = ForkState.InUse;
            _stopwatch.Restart();
        }
    }

    public void ReleaseFork()
    {
        using (_lock.EnterScope())
        {
            _stopwatch.Stop();
            BlockTime += _stopwatch.ElapsedMilliseconds;
            Owner = null;
            State = ForkState.Available;
            _stopwatch.Restart();
        }
    }

    public bool IsAvailable()
    {
        using (_lock.EnterScope())
        {
            return State == ForkState.Available;
        }
    }

    public bool IsOwner(string owner)
    {
        using (_lock.EnterScope())
        {
            return Owner == owner;
        }
    }
}