using TableService.Domain.Enums;

namespace Application.Snapshots;

public record ForkSnapshot(
    ForkState State,
    string? Owner,
    long FreeTime,
    long BlockTime);