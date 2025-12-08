using CoordinatorService.Application.Abstractions;
using CoordinatorService.Application.Configurations;
using Microsoft.Extensions.Options;

namespace CoordinatorService.Application.Services;

public class CoordinatorManager(IOptions<CoordinatorOptions> options) : ICoordinatorManager
{
    private readonly Queue<int> _waiting = new();
    private readonly HashSet<int> _activeEaters = new();
    private readonly int _maxEating = options.Value.PhilosopherCount - 1;
    private readonly Lock _lock = new();

    public void EnqueueHungry(int philosopherId)
    {
        using var _ = _lock.EnterScope();
        if (!_activeEaters.Contains(philosopherId) && !_waiting.Contains(philosopherId))
        {
            _waiting.Enqueue(philosopherId);
        }
    }
    
    public void EndEating(int philosopherId)
    {
        using var _ = _lock.EnterScope();
        _activeEaters.Remove(philosopherId);
    }

    public bool TryGrantPermissionToEat(out int philosopherId)
    {
        using var _ = _lock.EnterScope();
        philosopherId = default;
        if (_activeEaters.Count >= _maxEating || _waiting.Count == 0)
        {
            return false;
        }
        philosopherId = _waiting.Dequeue();
        _activeEaters.Add(philosopherId);
        return true;
    }
}