namespace Model.Entity;

public class SimulationRun
{
    public long RunId { get; private set; }
    public long Duration { get; private set; }
    
    public int PhilosophersCount { get; private set; }

    private SimulationRun() {}

    public SimulationRun(long duration, int philosophersCount)
    {
        Duration = duration;
        PhilosophersCount = philosophersCount;
    }
}