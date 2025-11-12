namespace Contract.Services.Event;

public interface IEventQueue
{
    void Enqueue(object evt);
    IAsyncEnumerable<object> ReadAllAsync();
}