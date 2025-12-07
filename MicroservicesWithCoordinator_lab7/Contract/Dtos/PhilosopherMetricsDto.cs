using Contract.Enums;

namespace Contract.Dtos;

public record PhilosopherMetricsDto(
    PhilosopherState State,
    PhilosopherAction Action,
    int CurrentActionDuration,
    int Eaten,
    long WaitingTime);