namespace Model;

public class PhilosopherMetrics
{
    public int Eaten { get; private set; }
    public int WaitingTime { get; set; }
    
    public void IncrementEaten() {Eaten++;}
}