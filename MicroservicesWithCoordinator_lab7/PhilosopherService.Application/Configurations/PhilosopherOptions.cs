namespace PhilosopherService.Application.Configurations;

public class PhilosopherOptions
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int LeftForkId { get; set; }
    public int RightForkId { get; set; }
    public double DurationMinutes { get; set; }
    
    public int ThinkingTimeMin { get; set; }
    public int ThinkingTimeMax { get; set; }
    public int EatingTimeMin { get; set; }
    public int EatingTimeMax { get; set; }
    public int ForkAcquisitionTime { get; set; }
}