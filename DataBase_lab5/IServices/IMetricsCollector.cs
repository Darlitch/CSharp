namespace IServices;

public interface IMetricsCollector
{
    void PrintMetrics(long currTime);
    void PrintFinalMetrics(long currTime);
}