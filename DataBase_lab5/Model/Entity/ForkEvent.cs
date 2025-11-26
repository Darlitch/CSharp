using Model.Enums;

namespace Model.Entity;

public class ForkEvent
{
    public long Id { get; private set; }
    public int Index { get; private set; }
    public string? Owner { get; private set; }
    public ForkState State { get; private set; }
    public long TimestampMs { get; private set; }
    public long RunId { get; private set; }
    
    private ForkEvent() { }

    public ForkEvent(int index, string? owner, ForkState state, long timestampMs, long runId)
    {
        Index = index;
        Owner = owner;
        State = state;
        TimestampMs = timestampMs;
        RunId = runId;
    }
}