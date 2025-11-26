using Contract.Dtos;

namespace TableService.Domain;

public class PhilosopherEntry
{
    public int Id { get; }
    public string Name { get; }
    public int LeftForkId { get; }
    public int RightForkId { get; }
    public PhilosopherMetricsDto? Metrics { get; set; }
    public bool IsFinished { get; set; }

    public PhilosopherEntry(int id, string name, int leftForkId, int rightForkId)
    {
        Id = id;
        Name = name;
        LeftForkId = leftForkId;
        RightForkId = rightForkId;
        Metrics = null;
        IsFinished = false;
    }
}