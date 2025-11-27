using Contract.Enums;

namespace PhilosopherService.Domain;

public class PhilosopherMetrics
{
    public PhilosopherState State { get; set; }
    public PhilosopherAction Action { get; set; }
    public int CurrentActionDuration { get; set; }
    public int Eaten { get; private set; }
    public long WaitingTime { get; set; }
    
    public void IncrementEaten() {Eaten++;}
}