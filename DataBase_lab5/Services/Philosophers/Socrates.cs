using Contract.Services.PhilosopherMain;
using Microsoft.Extensions.Options;
using Services.PhilosopherMain;
using Services.Simulation;

namespace Services.Philosophers;

public class Socrates(
    IOptions<SimulationOptions> options,
    IPhilosopherServiceBundle services,
    int ind) : PhilosopherHostedService(options, services, ind, "Сократ");