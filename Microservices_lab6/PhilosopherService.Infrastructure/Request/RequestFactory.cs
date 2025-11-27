using System.Text;
using System.Text.Json;
using Contract.Dtos;
using Microsoft.Extensions.Options;

namespace PhilosopherService.Infrastructure.Request;

public class RequestFactory(IOptions<RequestDomains> options)
{
    private readonly RequestDomains _domains = options.Value;
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public HttpRequestMessage Register(RegisterPhilosopherDto body)
    {
        var url = $"{_domains.TableServiceDomain}{RequestPath.Register}";
        var json = JsonSerializer.Serialize(body, JsonOptions);
        return new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    public HttpRequestMessage TakeLeftFork(int philosopherId)
    {
        var url = $"{_domains.TableServiceDomain}{RequestPath.TakeLeftFork(philosopherId)}";
        return new HttpRequestMessage(HttpMethod.Post, url);
    }

    public HttpRequestMessage TakeRightFork(int philosopherId)
    {
        var url = $"{_domains.TableServiceDomain}{RequestPath.TakeRightFork(philosopherId)}";
        return new HttpRequestMessage(HttpMethod.Post, url);
    }

    public HttpRequestMessage ReleaseForks(int philosopherId)
    {
        var url = $"{_domains.TableServiceDomain}{RequestPath.ReleaseForks(philosopherId)}";
        return new HttpRequestMessage(HttpMethod.Post, url);
    }
    
    public HttpRequestMessage UpdateMetrics(int philosopherId, PhilosopherMetricsDto body)
    {
        var url = $"{_domains.TableServiceDomain}{RequestPath.UpdateMetrics(philosopherId)}";
        var json = JsonSerializer.Serialize(body, JsonOptions);
        return new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }
    
    public HttpRequestMessage Finish(int philosopherId)
    {
        var url = $"{_domains.TableServiceDomain}{RequestPath.Finish(philosopherId)}";
        return new HttpRequestMessage(HttpMethod.Patch, url);
    }
}