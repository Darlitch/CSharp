using Application.Abstractions;
using Contract.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace TableService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TableController(ITableManager tableManager) : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RegisterPhilosopherDto dto)
    {
        tableManager.RegisterPhilosopher(dto);
        return Ok();
    }

    [HttpPost("{philosopherId:int}/take-left")]
    public IActionResult TakeLeftFork([FromRoute] int philosopherId)
    {
        var resultDto = tableManager.TryTakeLeftFork(philosopherId);
        return Ok(resultDto);
    }
    
    [HttpPost("{philosopherId:int}/take-right")]
    public IActionResult TakeRightFork([FromRoute] int philosopherId)
    {
        var resultDto = tableManager.TryTakeRightFork(philosopherId);
        return Ok(resultDto);
    }

    [HttpPost("{philosopherId:int}/release-all")]
    public IActionResult ReleaseAll([FromRoute] int philosopherId)
    {
        tableManager.ReleaseForks(philosopherId);
        return NoContent();
    }

    [HttpPut("{philosopherId:int}/metrics")]
    public IActionResult UpdateMetrics([FromRoute] int philosopherId, [FromBody] PhilosopherMetricsDto dto)
    {
        tableManager.UpdatePhilosopherMetrics(philosopherId, dto);
        return Ok();
    }

    [HttpPatch("{philosopherId:int}/finish")]
    public IActionResult Finish([FromRoute] int philosopherId)
    {
        tableManager.FinishPhilosopher(philosopherId);
        return Ok();
    }
    
}