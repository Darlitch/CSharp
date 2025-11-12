using Model.Enums;

namespace Model.DTO;

public record CreateForkEventDto(int Index, string? Owner, ForkState State, long CurrentTimeMs);