using Contract.Enums;
using PhilosopherService.Domain.Enums;

namespace PhilosopherService.Application.Abstractions;

public interface IStrategy
{
    public PhilosopherAction SelectAction(ForkState leftFork, ForkState rightFork);
}