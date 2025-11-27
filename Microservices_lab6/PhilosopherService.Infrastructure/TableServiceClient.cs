using System.Net.Http.Json;
using Contract.Dtos;
using PhilosopherService.Application.Abstractions;
using PhilosopherService.Infrastructure.Request;

namespace PhilosopherService.Infrastructure;

public class TableServiceClient(HttpClient client, RequestFactory requestFactory) : ITableServiceClient
{

    public async Task Register(RegisterPhilosopherDto dto, CancellationToken ct = default)
    {
        using var request = requestFactory.Register(dto);
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TakeForkResult> TakeLeftFork(int philosopherId, CancellationToken ct = default)
    {
        using var request = requestFactory.TakeLeftFork(philosopherId);
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TakeForkResult>(ct)
               ?? throw new InvalidOperationException("TableService returned empty body for TakeLeftFork");
    }
    
    public async Task<TakeForkResult> TakeRightFork(int philosopherId, CancellationToken ct = default)
    {
        using var request = requestFactory.TakeRightFork(philosopherId);
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TakeForkResult>(ct)
               ?? throw new InvalidOperationException("TableService returned empty body for TakeRightFork");
    }
    
    public async Task ReleaseForks(int philosopherId, CancellationToken ct = default)
    {
        using var request = requestFactory.ReleaseForks(philosopherId);
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateMetrics(int philosopherId, PhilosopherMetricsDto dto, CancellationToken ct = default)
    {
        using var request = requestFactory.UpdateMetrics(philosopherId, dto);
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task Finish(int philosopherId, CancellationToken ct = default)
    {
        using var request = requestFactory.Finish(philosopherId);
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }

}