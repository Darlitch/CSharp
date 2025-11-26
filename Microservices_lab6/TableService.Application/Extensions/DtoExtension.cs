using Contract.Dtos;
using TableService.Domain;

namespace Application.Extensions;

public static class DtoExtension
{
    public static PhilosopherEntry ToPhilosopherEntry(this RegisterPhilosopherDto dto)
        => new PhilosopherEntry(dto.Id, dto.Name, dto.LeftForkId, dto.RightForkId);
}