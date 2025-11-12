using Contract.Services;
using Contract.Services.Event;
using Contract.Services.PhilosopherMain;
using Contract.Services.Simulation;
using Microsoft.Extensions.Options;
using Services.PhilosopherMain;
using Services.Simulation;
using StrategyInterface;

namespace Services.Philosophers;

public class Platoo(
    IOptions<SimulationOptions> options,
    IPhilosopherServiceBundle services,
    int ind) : PhilosopherHostedService(options, services, ind, "Платон");