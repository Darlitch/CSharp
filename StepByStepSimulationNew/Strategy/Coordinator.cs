using Model;
using StrategyInterface;

namespace Strategy;

public class Coordinator(List<Philosopher> philosophers, List<Fork> forks) : ICoordinator
{
    private List<Philosopher> _philosophers = philosophers;
    private List<Fork> _forks = forks;
    
    public Handle
}