using Data;
using Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Entity;
using Model.Enums;

namespace UnitTests.SQLite_Tests;

public class ForkEventTests
{
    private readonly SqliteConnection _connection;
    private readonly TestDbContext _context;
    private readonly ForkEventRepository _repository;
    
    public ForkEventTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging(true)
            .Options;
        _context = new TestDbContext(options);
        _connection.Open();
        _context.Database.EnsureCreated();
        _repository = new ForkEventRepository(_context);
    }

    [Fact]
    public async Task AddAsync_SaveForkEvent()
    {
        await _repository.AddAsync(new ForkEvent(1, null, ForkState.Available, 1000, 1));
        var evt = await _context.ForkEvents.FirstOrDefaultAsync(e => e.Id == 1);
        Assert.NotNull(evt);
        Assert.Equal(1, evt.Index);
        Assert.Equal(ForkState.Available, evt.State);
        Assert.Null(evt.Owner);
        Assert.Equal(1000, evt.TimestampMs);
        Assert.Equal(1, evt.RunId);
    }

    [Fact]
    public async Task GetAsync_ReturnsLatestStateBeforeTimeStamp()
    {
        await _repository.AddAsync(new ForkEvent(1, null, ForkState.Available, 1000, 1));
        var evt = await _repository.GetAsync(1, 1200, 1);
        Assert.NotNull(evt);
        Assert.Equal(1, evt.Index);
        Assert.Equal(ForkState.Available, evt.State);
        Assert.Null(evt.Owner);
        Assert.Equal(1000, evt.TimestampMs);
        Assert.Equal(1, evt.RunId);
        evt = await _repository.GetAsync(1, 1000, 1);
        Assert.NotNull(evt);
        evt = await _repository.GetAsync(1, 999, 1);
        Assert.Null(evt);
    }
}