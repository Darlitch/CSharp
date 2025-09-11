namespace Strategy;

public interface IStrategy
{
    public void TryToStartEating(int philosopherId);
    public void ReleaseFork(int philosopherId);
}