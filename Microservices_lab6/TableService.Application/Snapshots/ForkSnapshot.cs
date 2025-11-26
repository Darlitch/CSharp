using TableService.Domain.Enums;

namespace Application.Snapshots;

public record ForkSnapshot(
    int Index,
    ForkState State,
    string? Owner,
    long FreeTime,
    long BlockTime);