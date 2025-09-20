using Model.Enums;

namespace StrategyInterface;

public interface ICoordinator
{
    public void TakeLeftFork(string name);
    public void Update();
}