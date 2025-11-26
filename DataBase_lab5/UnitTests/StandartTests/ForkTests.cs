using Model;
using Model.Enums;

namespace UnitTests.StandartTests;

public class ForkTests
{
    [Fact]
    public void TestTryTakeFork()
    {
        var fork = new Fork();
        Assert.Equal(ForkState.Available, fork.State);
        fork.TryTakeFork("test1");
        Assert.Equal(ForkState.InUse, fork.State);
        Assert.Equal("test1", fork.Owner);
        fork.TryTakeFork("test2");
        Assert.Equal(ForkState.InUse, fork.State);
        Assert.Equal("test1", fork.Owner);
    }

    [Fact]
    public void TestReleaseFork()
    {
        var fork = new Fork();
        fork.TryTakeFork("test1");
        fork.ReleaseFork();
        Assert.Equal(ForkState.Available, fork.State);
        Assert.Equal(null, fork.Owner);
    }
}