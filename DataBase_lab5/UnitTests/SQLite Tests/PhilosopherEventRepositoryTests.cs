using Data;
using Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Entity;
using Model.Enums;

namespace UnitTests.SQLite_Tests;

public class PhilosopherEventRepositoryTests
{
    private readonly SqliteConnection _connection;
    private readonly TestDbContext _context;
    private readonly PhilosopherEventRepository _repository;

    public PhilosopherEventRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging(true)
            .Options;
        _context = new TestDbContext(options);
        _connection.Open();
        _context.Database.EnsureCreated();
        _repository = new PhilosopherEventRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task AddAsync_SavePhilosopherEvent()
    {
        await _repository.AddAsync(new PhilosopherEvent(1, "Test", PhilosopherState.Hungry,
            PhilosopherAction.None, 25, 200, 1000, 1));
        var evt = await _context.PhilosopherEvents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 1);
        Assert.NotNull(evt);
        Assert.Equal(1, evt.Index);
        Assert.Equal("Test", evt.Name);
        Assert.Equal(PhilosopherState.Hungry, evt.State);
        Assert.Equal(PhilosopherAction.None, evt.Action);
        Assert.Equal(25, evt.Eaten);
        Assert.Equal(200, evt.WaitingTime);
        Assert.Equal(1000, evt.TimestampMs);
        Assert.Equal(1, evt.RunId);
    }

    [Fact]
    public async Task GetAsync_ReturnsLatestStateBeforeTimeStamp()
    {
        await _repository.AddAsync(new PhilosopherEvent(1, "Test", PhilosopherState.Hungry,
            PhilosopherAction.None, 25, 200, 1000, 1));
        var evt = await _repository.GetAsync(1, 1200, 1);
        Assert.NotNull(evt);
        Assert.Equal(1, evt.Index);
        Assert.Equal("Test", evt.Name);
        Assert.Equal(PhilosopherState.Hungry, evt.State);
        Assert.Equal(PhilosopherAction.None, evt.Action);
        Assert.Equal(25, evt.Eaten);
        Assert.Equal(200, evt.WaitingTime);
        Assert.Equal(1000, evt.TimestampMs);
        Assert.Equal(1, evt.RunId);
        evt = await _repository.GetAsync(1, 1000, 1);
        Assert.NotNull(evt);
        evt = await _repository.GetAsync(1, 999, 1);
        Assert.Null(evt);
    }
}