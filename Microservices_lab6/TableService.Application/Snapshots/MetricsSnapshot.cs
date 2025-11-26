using Contract.Enums;

namespace Application.Snapshots;

public record MetricsSnapshot(
    int Id,
    string Name,
    PhilosopherState State,
    PhilosopherAction Action,
    int CurrentActionDuration,
    int Eaten,
    long WaitingTime);