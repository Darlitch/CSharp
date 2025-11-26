namespace View.Contract;

public interface IViewStateSimulation
{
    public Task Run(long runId, double delay);
}