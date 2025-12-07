using Contract.Enums;

namespace PhilosopherService.Application.Abstractions;

public interface IStrategy
{
    public PhilosopherAction SelectAction(ForkState leftFork, ForkState rightFork);
}