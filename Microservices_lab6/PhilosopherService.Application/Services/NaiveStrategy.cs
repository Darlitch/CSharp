using Contract.Enums;
using PhilosopherService.Application.Abstractions;
using PhilosopherService.Domain.Enums;

namespace PhilosopherService.Application.Services;

public class NaiveStrategy : IStrategy
{
    public PhilosopherAction SelectAction(ForkState leftFork, ForkState rightFork)
    {
        if ((leftFork == ForkState.NotInHand))
        {
            return PhilosopherAction.TakeLeftFork;
        }
        if ((leftFork == ForkState.InHand) && rightFork == ForkState.NotInHand)
        {
            return PhilosopherAction.TakeRightFork;
        }
        return PhilosopherAction.None;
    }
}