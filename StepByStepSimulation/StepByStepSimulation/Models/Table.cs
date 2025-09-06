using System.Collections.Generic;
using System.IO;
using System.Linq;
using StepByStepSimulation.Enums;

namespace StepByStepSimulation.Models
{
    public class Table
    {   
        private const int _philosopherCount = 5;
        public List<ForkState> Forks { get; set; }
        public List<Philosopher> Philosophers { get; set; }

        public Table()
        {
            Forks = Enumerable.Repeat(ForkState.Available, 5).ToList();
            Philosophers = PhilosopherInitializer.InitPhilosophers();
        }
    }
}