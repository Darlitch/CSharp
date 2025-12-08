using PhilosopherService.Application.Abstractions;

namespace PhilosopherService.Application.Services;

public class EatingPermissionService : IEatingPermissionService
{
    private int _flag = 0;
    
    public void GrantPermission()
    {
        Interlocked.Exchange(ref _flag, 1);
    }

    public void ResetPermission()
    {
        Interlocked.Exchange(ref _flag, 0);
    }

    public bool CheckPermission() => Volatile.Read(ref _flag) == 1;
}