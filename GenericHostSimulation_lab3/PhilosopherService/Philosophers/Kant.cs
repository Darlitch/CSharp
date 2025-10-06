using Model;
using StrategyInterface;

namespace PhilosopherService.Philosophers;

public class Kant(IPhilosopherStrategy strategy, Fork leftFork, Fork rightFork)
    : PhilosopherHostedService(strategy, leftFork, rightFork, "Кант");