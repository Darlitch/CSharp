using IServices;
using Microsoft.Extensions.Options;
using Model.Enums;
using Moq;
using Services;
using Strategy;

namespace UnitTests;

public class PhilosopherStateTests
{
    private readonly PhilosopherHostedService _philosopher;
    public PhilosopherStateTests()
    {
        var strategy = new NaivePhilosopherStrategy();
        var optionsMock = new Mock<IOptions<SimulationOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new SimulationOptions
        {
            ThinkingTimeMin = 0,
            ThinkingTimeMax = 0,
            EatingTimeMin = 0,
            EatingTimeMax = 0,
            ForkAcquisitionTime = 0,
            PhilosophersCount = 5
        });
        var options = optionsMock.Object;
        var tableManager = new TableManager(options);
        _philosopher = new PhilosopherHostedService(strategy, tableManager, options, 0, "Test");
    }

    [Fact]
    public void Philosopher_DefaultState()
    {
        Assert.Equal(PhilosopherState.Thinking, _philosopher.State);
    }

    [Fact]
    public void Philosopher_SetHungry()
    {
        _philosopher.SetHungry();
        Assert.Equal(PhilosopherState.Hungry, _philosopher.State);
    }

    [Fact]
    public void Philosopher_StartEating()
    {
        _philosopher.SetHungry();
        _philosopher.HandleAction(PhilosopherAction.TakeLeftFork);
        _philosopher.HandleAction(PhilosopherAction.TakeRightFork);
        Assert.True(_philosopher.LeftFork.IsOwner("Test"));
        Assert.True(_philosopher.RightFork.IsOwner("Test"));
        _philosopher.Update();
        Assert.Equal(PhilosopherState.Eating, _philosopher.State);
    }

    [Fact]
    public void Philosopher_StartThinking()
    {
        _philosopher.StartEating();
        _philosopher.Update();
        Assert.Equal(PhilosopherState.Thinking, _philosopher.State);
        Assert.True(_philosopher.LeftFork.IsAvailable());
        Assert.True(_philosopher.RightFork.IsAvailable());
    }
}