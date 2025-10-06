using Model;
using StrategyInterface;

namespace PhilosopherService.Philosophers;

public class Platoo(IPhilosopherStrategy strategy, Fork leftFork, Fork rightFork)
    : PhilosopherHostedService(strategy, leftFork, rightFork, "Платон");