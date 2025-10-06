using Model;
using StrategyInterface;

namespace PhilosopherService.Philosophers;

public class Socrates(IPhilosopherStrategy strategy, Fork leftFork, Fork rightFork)
    : PhilosopherHostedService(strategy, leftFork, rightFork, "Сократ");