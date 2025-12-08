namespace PhilosopherService.Application.Abstractions;

public interface IEatingPermissionService
{
    void GrantPermission();
    void ResetPermission();
    bool CheckPermission();
}