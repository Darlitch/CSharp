using System.Linq.Expressions;
using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy;

public class Coordinator: ICoordinator
{
    private readonly List<Philosopher> _philosophers;
    private readonly List<Fork> _forks;
    private readonly HashSet<int> _waiting;
    public event Action<PhilosopherAction> OnAction;

    public Coordinator(List<Philosopher> philosophers, List<Fork> forks)
    {
        _philosophers = philosophers;
        _forks = forks;
        _waiting = new HashSet<int>();
        foreach (var philosopher in _philosophers)
        {
            philosopher.OnHungry += HandleOnHungry;
        }
    }

    private void HandleOnHungry(Philosopher philosopher)
    {
        if (_philosophers.IndexOf(philosopher) >= 0)
        {
            _waiting.Add(_philosophers.IndexOf(philosopher));
        }
    }

    public PhilosopherAction Update()
    {
        foreach (var ind in _waiting.Where(ind => _forks[ind].State == ForkState.InUse && _forks[ind].Owner == _philosophers[ind].Name))
        {
            switch (_philosophers[(ind+1) % _philosophers.Count].State)
            {
                case PhilosopherState.Thinking:
                    _waiting.Remove(ind);
                    return PhilosopherAction.TakeRightFork;
                case PhilosopherState.Hungry:
                    _philosophers[(ind+1) % _philosophers.Count].ReleaseForks();
                    _waiting.Remove(ind);
                    return PhilosopherAction.TakeRightFork;
                default:
                    break;
            }
        }

        return PhilosopherAction.None;
    }
}