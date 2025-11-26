using Model.Enums;

namespace Model.DTO;

public record CreatePhilosopherEventDto(int Index, string Name, PhilosopherState State, PhilosopherAction Action,
    int Eaten, long WaitingTime, long CurrentTimeMs);