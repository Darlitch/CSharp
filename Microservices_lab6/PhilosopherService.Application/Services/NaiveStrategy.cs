using Contract.Enums;
using PhilosopherService.Application.Abstractions;
using PhilosopherService.Domain.Enums;

namespace PhilosopherService.Application.Services;

public class NaiveStrategy : IStrategy
{
    public PhilosopherAction SelectAction(ForkState leftFork, ForkState rightFork)
    {
        return leftFork switch
        {
            ForkState.NotInHand => PhilosopherAction.TakeLeftFork,
            ForkState.InHand when rightFork == ForkState.NotInHand => PhilosopherAction.TakeRightFork,
            _ => PhilosopherAction.None
        };
    }
}