using System.Collections.Generic;
using System.IO;
using StepByStepSimulationNew.Models;

namespace StepByStepSimulationNew;

public class PhilosopherInitializer
{
    private const int PhilosopherCount = 5;

    public static List<Philosopher> InitPhilosophers()
    {
        var philosophers = new List<Philosopher>();
        // string file = "Resources/Philosophers.txt";
        string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Philosophers.txt");
        if (!File.Exists(file))
        {
            throw new FileNotFoundException($"Could not find {file}", file);
        }

        try
        {
            using var reader = new StreamReader(file);
            string? line;
            while ((line = reader.ReadLine()) != null && philosophers.Count < PhilosopherCount)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    philosophers.Add(new Philosopher(line.Trim()));
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