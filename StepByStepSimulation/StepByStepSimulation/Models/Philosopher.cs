using StepByStepSimulation.Enums;

namespace StepByStepSimulation.Models
{
    public class Philosopher
    {
        public string Name { get; }
        public int Eaten { get; private set; }
        public int Steps { get; private set; }
        public PhilosopherState State { get; private set; }

        public Philosopher(string name)
        {
            Name = name;
            State = PhilosopherState.Thinking;
            Eaten = 0;
            Steps = 0;
        }

        public void StartThinking()
        {
            State = PhilosopherState.Thinking;
            Steps = 0;
        }

        public void SetHungry()
        {
            State = PhilosopherState.Hungry;
            Steps = 0;
        }

        public void StartEating()
        {
            State = PhilosopherState.Eating;
            Steps = 0;
        }
    }
}