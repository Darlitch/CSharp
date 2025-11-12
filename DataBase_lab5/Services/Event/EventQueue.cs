using System.Threading.Channels;
using Contract.Services.Event;

namespace Services.Event;

public class EventQueue : IEventQueue
{
    private readonly Channel<object> _channel = Channel.CreateBounded<object>(1000);
    
    public void Enqueue(object evt) => _channel.Writer.TryWrite(evt);
    public IAsyncEnumerable<object> ReadAllAsync() => _channel.Reader.ReadAllAsync();
}