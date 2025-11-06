using Model.Enums;

namespace Model.Entity;

public class PhilosopherEvent
{
    public long Id { get; private set; }
    public int Index { get; private set; }
    public string Name { get; private set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }
    public int Eaten { get; private set; }
    public long WaitingTime { get; private set; }
    public long TimestampMs { get; private set; }
    public long RunId { get; private set; }
    
    private PhilosopherEvent() { }

    public PhilosopherEvent(int index, string name, PhilosopherState state, PhilosopherAction action,
        int eaten, long waitingTime, long timestampMs, long runId)
    {
        Index = index;
        Name = name;
        State = state;
        Action = action;
        Eaten = eaten;
        WaitingTime = waitingTime;
        RunId = runId;
    }
}