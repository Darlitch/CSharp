using Model;
using StrategyInterface;

namespace PhilosopherService.Philosophers;

public class Decartes(IPhilosopherStrategy strategy, Fork leftFork, Fork rightFork)
    : PhilosopherHostedService(strategy, leftFork, rightFork, "Декарт");