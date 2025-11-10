using Model.Enums;

namespace Model.DTO;

public record ForkEventDto(int Index, string? Owner, ForkState State, long CurrentTimeMs);