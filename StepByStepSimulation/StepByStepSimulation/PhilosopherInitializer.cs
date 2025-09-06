using System.Collections.Generic;
using System.IO;
using StepByStepSimulation.Models;

namespace StepByStepSimulation;

public class PhilosopherInitializer
{
    private const int _philosopherCount = 5;
    
    public static List<Philosopher> InitPhilosophers()
    {
        var philosophers = new List<Philosopher>();
        string file = "Resources/Philosophers.txt";
        if (!File.Exists(file))
        {
            throw new FileNotFoundException($"Could not find {file}", file);
        }

        try
        {
            using var reader = new StreamReader(file);
            string? line;
            while ((line = reader.ReadLine()) != null && philosophers.Count < _philosopherCount)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    philosophers.Add(new Philosopher(line.Trim()));
                }
            }

            if (philosophers.Count < _philosopherCount)
            {
                throw new InvalidDataException(
                    $"{_philosopherCount} names of philosophers are expected, but in the file only {philosophers.Count}");
            }
                
            return philosophers;
        }
        catch (IOException ex)
        {
            throw new IOException($"Could not read {file}", ex);
        }
    }
}