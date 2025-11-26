using Contract.Dtos;

namespace Application.Abstractions;

public interface IMetricReporter
{
    void PrintMetrics(long currTime, List<PhilosopherMetricsDto> philosophersMetrics);
    void PrintFinalMetrics(long currTime, List<PhilosopherMetricsDto> philosophersMetrics);
}