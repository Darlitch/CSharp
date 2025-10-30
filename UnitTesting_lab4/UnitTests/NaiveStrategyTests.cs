using Model;
using Model.Enums;
using Strategy;

namespace UnitTests;

public class NaiveStrategyTests
{
    private readonly Fork _leftFork = new();
    private readonly Fork _rightFork = new();
    private readonly NaivePhilosopherStrategy _strategy = new();

    [Fact]
    public void SelectAction_WhenAllForkAvailable()
    {
        var result = _strategy.SelectAction("tester1", _leftFork, _rightFork);
        Assert.Equal(PhilosopherAction.TakeLeftFork, result);
    }
    
    [Fact]
    public void SelectAction_WhenLeftForkInUseByTester()
    {
        _leftFork.TryTakeFork("tester1");
        var result = _strategy.SelectAction("tester1", _leftFork, _rightFork);
        Assert.Equal(PhilosopherAction.TakeRightFork, result);
    }
    
    [Fact]
    public void SelectAction_WhenLeftForkInUseByOtherPhilosopher()
    {
        _leftFork.TryTakeFork("tester1");
        var result = _strategy.SelectAction("tester2", _leftFork, _rightFork);
        Assert.Equal(PhilosopherAction.None, result);
    }
    
    [Fact]
    public void SelectAction_WhenAllInUse()
    {
        _leftFork.TryTakeFork("tester1");
        _rightFork.TryTakeFork("tester2");
        var result = _strategy.SelectAction("tester3", _leftFork, _rightFork);
        Assert.Equal(PhilosopherAction.None, result);
    }
    
    [Fact]
    public void SelectAction_WhenLeftForkInUseByTesterAndRightForkInUseByOtherPhilosopher()
    {
        _leftFork.TryTakeFork("tester1");
        _rightFork.TryTakeFork("tester2");
        var result = _strategy.SelectAction("tester1", _leftFork, _rightFork);
        Assert.Equal(PhilosopherAction.None, result);
    }
}