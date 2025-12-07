namespace Contract.Dtos;

public record RegisterPhilosopherDto(
    int Index,
    string Name,
    int LeftForkId,
    int RightForkId);