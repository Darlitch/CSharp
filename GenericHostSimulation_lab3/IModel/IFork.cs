namespace IModel;

public interface IFork
{
    public void TakeFork(string owner);
    public void ReleaseFork();
}