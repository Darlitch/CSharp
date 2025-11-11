namespace Model.Entity;

public class SimulationRun
{
    public long RunId { get; private set; }
    public long DurationMs { get; private set; }
    
    public int PhilosophersCount { get; private set; }

    private SimulationRun() {}

    public SimulationRun(long durationMs, int philosophersCount)
    {
        DurationMs = durationMs;
        PhilosophersCount = philosophersCount;
    }
    
    public void UpdateDuration(long durationMs) => DurationMs = durationMs;
}