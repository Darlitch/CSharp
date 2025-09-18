using Model.Enums;

namespace StrategyInterface;

public interface ICoordinator
{
    public PhilosopherAction Update();
}