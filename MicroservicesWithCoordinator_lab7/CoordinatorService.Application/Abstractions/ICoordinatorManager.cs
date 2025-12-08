namespace CoordinatorService.Application.Abstractions;

public interface ICoordinatorManager
{
    void EnqueueHungry(int philosopoherId);
    void EndEating(int philosopoherId);
    bool TryGrantPermissionToEat(out int philosopoherId);
}