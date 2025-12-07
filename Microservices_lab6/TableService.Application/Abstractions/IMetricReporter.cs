using Contract.Dtos;

namespace Application.Abstractions;

public interface IMetricReporter
{
    void PrintMetrics(long currTime);
    void PrintFinalMetrics(long currTime);
}