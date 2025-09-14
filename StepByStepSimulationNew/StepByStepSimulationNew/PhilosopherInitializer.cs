
using Model;

namespace StepByStepSimulationNew;

public static class PhilosopherInitializer
{
    private const int PhilosopherCount = 5;

    public static List<Philosopher> InitPhilosophers()
    {
        var philosophers = new List<Philosopher>();
        // var forks = Enumerable.Repeat(new Fork(), PhilosopherCount).ToList();
        var forks = new List<Fork>();
        for (int i = 0; i < PhilosopherCount; ++i)
        {
            forks.Add(new Fork());
        }
        // string file = "Resources/Philosophers.txt";
        string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Philosophers.txt");
        if (!File.Exists(file))
        {
            throw new FileNotFoundException($"Could not find {file}", file);
        }

        try
        {
            using var reader = new StreamReader(file);
            int ind = 0;
            while (reader.ReadLine() is { } line && philosophers.Count < PhilosopherCount)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    philosophers.Add(new Philosopher(line.Trim(), forks[ind], forks[(ind + 1) % PhilosopherCount]));
                    ind++;
                }
            }

            if (philosophers.Count < PhilosopherCount)
            {
                throw new InvalidDataException(
                    $"{PhilosopherCount} names of philosophers are expected, but in the file only {philosophers.Count}");
            }

            return philosophers;
        }
        catch (IOException ex)
        {
            throw new IOException($"Could not read {file}", ex);
        }
    }
}