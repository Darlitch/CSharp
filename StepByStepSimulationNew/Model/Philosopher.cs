using Model.Enums;

namespace Model;

public class Philosopher
{
    public string Name { get; }
    public int Eaten { get; private set; }
    public int WaitingTime { get; private set; }
    public int CurrentActionDuration { get; private set; }
    public PhilosopherState State { get; private set; }
    public PhilosopherAction Action { get; private set; }
    public Fork LeftFork { get; }
    public Fork RightFork { get; }

    public Philosopher(string name, Fork leftFork, Fork rightFork)
    {
        Name = name;
        Eaten = 0;
        LeftFork = leftFork;
        RightFork = rightFork;
        StartThinking();
        Action = PhilosopherAction.None;
    }

    private void StartThinking()
    {
        State = PhilosopherState.Thinking;
        CurrentActionDuration = new Random().Next(3, 10);
        // CurrentActionDuration = 5;
        // Action = PhilosopherAction.ReleaseForks;
        Action = PhilosopherAction.None;
    }

    private void SetHungry()
    {
        State = PhilosopherState.Hungry;
        CurrentActionDuration = 0;
    }

    private void StartEating()
    {
        State = PhilosopherState.Eating;
        CurrentActionDuration = new Random().Next(4, 5);
        Action = PhilosopherAction.None;
        Eaten++;
    }

    public void TakeLeftFork()
    {
        Action = PhilosopherAction.TakeLeftFork;
        LeftFork.TakeFork(Name);
        CurrentActionDuration = 2;
    }
    
    public void TakeRightFork()
    {
        Action = PhilosopherAction.TakeRightFork;
        RightFork.TakeFork(Name);
        CurrentActionDuration = 2;
    }

    public void ReleaseForks()
    {
        LeftFork.ReleaseFork();
        RightFork.ReleaseFork();
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
                ReleaseForks();
                break;
            case PhilosopherState.Hungry:
                if (LeftFork.Owner == Name && RightFork.Owner == Name)
                {
                    StartEating();
                }

                WaitingTime++;
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