using StepByStepSimulationNew.Enums;

namespace StepByStepSimulationNew.Models;

public class Philosopher
{
    public string Name { get; }
    public int Eaten { get; private set; }
    public int CurrentActionDuration { get; private set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }

    public Philosopher(string name)
    {
        Name = name;
        State = PhilosopherState.Thinking;
        Eaten = 0;
        CurrentActionDuration = new Random().Next(3, 10);
        Action = PhilosopherAction.None;
    }

    public void StartThinking()
    {
        State = PhilosopherState.Thinking;
        CurrentActionDuration = new Random().Next(3, 10);
    }

    public void SetHungry()
    {
        State = PhilosopherState.Hungry;
        CurrentActionDuration = 0;
    }

    public void StartEating()
    {
        State = PhilosopherState.Eating;
        CurrentActionDuration = new Random().Next(4, 5);
        Action = PhilosopherAction.None;
    }

    public void TakeLeftFork()
    {
        Action = PhilosopherAction.TakeLeftFork;
        CurrentActionDuration = 2;
    }
    
    public void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        CurrentActionDuration = 2;
    }

    public void Update()
    {
        CurrentActionDuration--;
        if (CurrentActionDuration == 0 && State == PhilosopherState.Thinking)
        {
            SetHungry();
        }
        else if (CurrentActionDuration == 0 && State == PhilosopherState.Eating)
        {
            StartEating();
        }
    }

    public bool IsHungry => State == PhilosopherState.Hungry;
}