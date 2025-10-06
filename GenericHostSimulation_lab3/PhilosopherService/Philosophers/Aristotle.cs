using Model;
using StrategyInterface;

namespace PhilosopherService.Philosophers;

public class Aristotle(IPhilosopherStrategy strategy, Fork leftFork, Fork rightFork)
    : PhilosopherHostedService(strategy, leftFork, rightFork, "Аристотель");