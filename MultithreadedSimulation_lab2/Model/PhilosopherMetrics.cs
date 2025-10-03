namespace Model;

public class PhilosopherMetrics
{
    public int Eaten { get; private set; }
    public long WaitingTime { get; set; }
    
    public void IncrementEaten() {Eaten++;}
}