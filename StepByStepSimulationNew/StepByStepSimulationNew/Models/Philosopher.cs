using StepByStepSimulationNew.Enums;

namespace StepByStepSimulationNew.Models;

public class Philosopher
{
    public string Name { get; }
    public int Eaten { get; private set; }
    public int CurrentActionDuration { get; set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }
    public ForkState LeftForkState { get; private set; }
    public ForkState RightForkState { get; private set; }

    public Philosopher(string name)
    {
        Name = name;
        Eaten = 0;
        StartThinking();
        Action = PhilosopherAction.None;
    }

    public void StartThinking()
    {
        State = PhilosopherState.Thinking;
        CurrentActionDuration = new Random().Next(3, 10);
        Action = PhilosopherAction.ReleaseLeftFork;
        LeftForkState = ForkState.Available;
        RightForkState = ForkState.Available;
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
        Eaten++;
    }

    public void TakeLeftFork()
    {
        Action = PhilosopherAction.TakeLeftFork;
        LeftForkState = ForkState.InUse;
        CurrentActionDuration = 2;
    }
    
    public void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        RightForkState = ForkState.InUse;
        CurrentActionDuration = 2;
    }

    public void ReleaseFork()
    {
        Action = PhilosopherAction.None;
    }

    public void Update()
    {
        CurrentActionDuration--;
        if (CurrentActionDuration != 0) return;
        switch (State)
        {
            case PhilosopherState.Thinking:
                SetHungry();
                break;
            case PhilosopherState.Eating:
                StartThinking();
                break;
            case PhilosopherState.Hungry:
                if (LeftForkState == ForkState.InUse && RightForkState == ForkState.InUse)
                {
                    StartEating();
                }
                break;
            default:
                break;
        }

        if (Action is PhilosopherAction.TakeLeftFork or PhilosopherAction.TakeRightFork)
        {
            Action = PhilosopherAction.None;
        }
    }

    public bool IsHungry => State == PhilosopherState.Hungry;
}