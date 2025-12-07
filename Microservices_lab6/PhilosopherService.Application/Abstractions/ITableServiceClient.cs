using Contract.Dtos;

namespace PhilosopherService.Application.Abstractions;

public interface ITableServiceClient
{
    Task Register(RegisterPhilosopherDto dto, CancellationToken ct = default);
    Task<TakeForkResult> TakeLeftFork(int philosopherId, CancellationToken ct = default);
    Task<TakeForkResult> TakeRightFork(int philosopherId, CancellationToken ct = default);
    Task ReleaseForks(int philosopherId, CancellationToken ct = default);
    Task UpdateMetrics(int philosopherId, PhilosopherMetricsDto dto, CancellationToken ct = default);
    Task Finish(int philosopherId, CancellationToken ct = default);
}