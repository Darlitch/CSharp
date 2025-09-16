using Model;
using StrategyInterface;

namespace Strategy;

public class Coordinator: ICoordinator
{
    private readonly List<Philosopher> _philosophers;
    private readonly List<Fork> _forks;
    private List<int> _waiting;

    public Coordinator(List<Philosopher> philosophers, List<Fork> forks)
    {
        _philosophers = philosophers;
        _forks = forks;
        _waiting = new List<int>();
        foreach (var philosopher in _philosophers)
        {
            philosopher.OnHungry += HandleOnHungry;
        }
    }

    private void HandleOnHungry(Philosopher philosopher)
    {
        if (!_waiting.Contains(_philosophers.IndexOf(philosopher)))
        {
            _waiting.Add(_philosophers.IndexOf(philosopher));
        }
    }
}