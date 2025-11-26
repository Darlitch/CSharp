namespace Contract.Dtos;

public record RegisterPhilosopherDto(
    int Id,
    string Name,
    int LeftForkId,
    int RightForkId);