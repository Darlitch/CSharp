namespace Contract.Services;

public interface IEventQueue
{
    void Enqueue(object evt);
    IAsyncEnumerable<object> ReadAllAsync();
}