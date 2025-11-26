using Data;
using Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace UnitTests.SQLite_Tests;

public class SimulationRunRepositoryTests
{
    private readonly SqliteConnection _connection;
    private readonly SimulationRunRepository _repository;
    private readonly TestDbContext _context;

    public SimulationRunRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging(true)
            .Options;
        _connection.Open();
        _context = new TestDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new SimulationRunRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
    
    [Fact]
    public async Task AddAsync_SaveSimulationRunAndReturnRunId()
    {
        var runId = await _repository.AddAsync(new SimulationRun(1000, 5));
        Assert.Equal(1, runId);
        var sim = await _context.Simulations.AsNoTracking().FirstOrDefaultAsync(s => s.RunId == runId);
        Assert.NotNull(sim);
        Assert.Equal(1000, sim.DurationMs);
        Assert.Equal(5, sim.PhilosophersCount);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesDurationMs()
    {
        var runId = await _repository.AddAsync(new SimulationRun(1000, 5));
        await _repository.UpdateAsync(runId, 255);
        var sim = await _context.Simulations.AsNoTracking().FirstOrDefaultAsync(s => s.RunId == runId);
        Assert.NotNull(sim);
        Assert.Equal(255, sim.DurationMs);
    }

    [Fact]
    public async Task GetAsync_ReturnsSimulationRun()
    {
        var runId = await _repository.AddAsync(new SimulationRun(1000, 5));
        var sim = await _repository.GetAsync(runId);
        Assert.NotNull(sim);
        Assert.Equal(1000, sim.DurationMs);
        Assert.Equal(5, sim.PhilosophersCount);
        sim = await _repository.GetAsync(runId+1);
        Assert.Null(sim);
    }
}