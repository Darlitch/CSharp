using System.Linq.Expressions;
using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy;

public class Coordinator: ICoordinator
{
    private readonly List<Philosopher> _philosophers;
    private readonly List<Fork> _forks;
    private readonly Queue<int> _waiting;
    public event Action<Philosopher, PhilosopherAction> OnAction;

    public Coordinator(List<Philosopher> philosophers, List<Fork> forks)
    {
        _philosophers = philosophers;
        _forks = forks;
        _waiting = new Queue<int>();
        foreach (var philosopher in _philosophers)
        {
            philosopher.OnHungry += HandleOnHungry;
            OnAction += philosopher.HandleOnAction;
        }
    }

    private void HandleOnHungry(Philosopher philosopher)
    {
        var index = _philosophers.IndexOf(philosopher);
        if (index >= 0 && !_waiting.Contains(index))
        {
            _waiting.Enqueue(index);
        }
    }

    public void TakeLeftFork(string name)
    {
        var philosopher = _philosophers.FirstOrDefault(p => p.Name == name);
        if (philosopher != null)
        {
            OnAction.Invoke(philosopher, PhilosopherAction.TakeLeftFork);
        }
    }

    public void Update()
    {
        // if (IsDeadLock())
        // {
        //     OnAction.Invoke(_philosophers[_waiting.Last()], PhilosopherAction.ReleaseForks);
        // }
        
        int count = _waiting.Count;
        
        for (int i = 0; i < count && _waiting.TryDequeue(out int index); ++i)
        {
            if (_forks[index].State == ForkState.InUse && _forks[index].Owner == _philosophers[index].Name && _philosophers[index].Action == PhilosopherAction.None)
            {
                switch (_philosophers[(index+1) % _philosophers.Count].State)
                {
                    case PhilosopherState.Thinking:
                        OnAction.Invoke(_philosophers[index], PhilosopherAction.TakeRightFork);
                        return;
                    case PhilosopherState.Hungry:
                        if (_philosophers[(index + 1) % _philosophers.Count].Action != PhilosopherAction.TakeRightFork)
                        {
                            OnAction.Invoke(_philosophers[(index+1) % _philosophers.Count], PhilosopherAction.ReleaseLeftFork);
                            OnAction.Invoke(_philosophers[index], PhilosopherAction.TakeRightFork);
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
            _waiting.Enqueue(index);
        }
    }

    // private bool IsDeadLock()
    // {
    //     return _philosophers.All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) &&
    //            _forks.All(f => f.State == ForkState.InUse);
    // }

    // public PhilosopherAction Update()
    // {
    //     foreach (var index in _waiting.Where(ind => _forks[ind].State == ForkState.InUse && _forks[ind].Owner == _philosophers[ind].Name))
    //     {
    //         switch (_philosophers[(index+1) % _philosophers.Count].State)
    //         {
    //             case PhilosopherState.Thinking:
    //                 _waiting.Remove(index);
    //                 return PhilosopherAction.TakeRightFork;
    //             case PhilosopherState.Hungry:
    //                 _philosophers[(index+1) % _philosophers.Count].ReleaseForks();
    //                 _waiting.Remove(index);
    //                 return PhilosopherAction.TakeRightFork;
    //             default:
    //                 break;
    //         }
    //     }
    //
    //     return PhilosopherAction.None;
    // }
}