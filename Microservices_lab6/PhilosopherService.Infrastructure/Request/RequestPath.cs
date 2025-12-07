namespace PhilosopherService.Infrastructure.Request;

public static class RequestPath
{
    private const string Base = "/table";
    
    public const string Register = Base;
    
    public static string TakeLeftFork(int philosopherId) =>
        $"{Base}/{philosopherId}/take-left";
    
    public static string TakeRightFork(int philosopherId) =>
        $"{Base}/{philosopherId}/take-right";
    
    public static string ReleaseForks(int philosopherId) =>
        $"{Base}/{philosopherId}/release-all";
    
    public static string UpdateMetrics(int philosopherId) =>
        $"{Base}/{philosopherId}/metrics";
    
    public static string Finish(int philosopherId) =>
        $"{Base}/{philosopherId}/finish";
}