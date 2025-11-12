using Model.Enums;

namespace Model.DTO;

public record PhilosopherEventDto(int Index, string Name, PhilosopherState State, PhilosopherAction Action,
    int Eaten, long WaitingTime);