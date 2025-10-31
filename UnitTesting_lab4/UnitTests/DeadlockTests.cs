using System.Security.Cryptography;
using Model;
using Model.Enums;
using Services;
using Strategy;

namespace UnitTests;

public class DeadlockTests
{
    private readonly Fork[] _forks;
    private readonly (string Name, Fork Left, Fork Right)[] _philosophers;
    private readonly NaivePhilosopherStrategy _strategy;
    public DeadlockTests()
    {
        _forks = new Fork[5];
        for (var i = 0; i < _forks.Length; ++i)
        {
            _forks[i] = new Fork();
        }
        _philosophers = new (string Name, Fork Left, Fork Right)[5];
        for (var i = 0; i < _philosophers.Length; ++i)
        {
            _philosophers[i] = ($"P{i+1}", _forks[i], _forks[(i+1) % _forks.Length]);
        }
        _strategy = new NaivePhilosopherStrategy();
    }
    
    
    [Fact]
    public void DeadlockTest()
    {
        foreach (var (name, left, right) in _philosophers)
        {
            var action = _strategy.SelectAction(name, left, right);
            if (action == PhilosopherAction.TakeLeftFork)
            {
                left.TryTakeFork(name);
            }
        }
        for (var i = 0; i < 5; i++)
        {
            Assert.True(_forks[i].IsOwner(_philosophers[i].Name));
        }
        
        bool anyCanEat = false;
        foreach (var (name, left, right) in _philosophers)
        {
            var action = _strategy.SelectAction(name, left, right);
            if (action == PhilosopherAction.TakeRightFork)
            {
                anyCanEat = true;
            }
        }
        Assert.False(anyCanEat);
        Assert.True(_forks.All(f => f.State == ForkState.InUse));
    }
}