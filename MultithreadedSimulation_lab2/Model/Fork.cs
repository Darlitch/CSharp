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

    public void TakeFork(string owner)
    {
        _stopwatch.Stop();
        FreeTime += _stopwatch.ElapsedMilliseconds;
        Owner = owner;
        State = ForkState.InUse;
        _stopwatch.Restart();
    }

    public void ReleaseFork()
    {
        _stopwatch.Stop();
        BlockTime += _stopwatch.ElapsedMilliseconds;
        Owner = null;
        State = ForkState.Available;
        _stopwatch.Restart();
    }
}