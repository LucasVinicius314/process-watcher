using System.Diagnostics;

public class Program
{
  public static List<PerformanceCounter> GetGPUCounters()
  {
    var category = new PerformanceCounterCategory("GPU Engine");
    var counterNames = category.GetInstanceNames();

    var gpuCounters = counterNames
                        .Where(counterName => counterName.EndsWith("engtype_3D"))
                        .SelectMany(counterName => category.GetCounters(counterName))
                        .Where(counter => counter.CounterName.Equals("Utilization Percentage"))
                        .ToList();

    return gpuCounters;
  }

  public static float GetGPUUsage(List<PerformanceCounter> gpuCounters)
  {
    gpuCounters.ForEach(x => x.NextValue());

    var result = gpuCounters.Sum(x => x.NextValue());

    return result;
  }

  public static void Main(string[] args)
  {
    while (true)
    {
      try
      {
        var gpuCounters = GetGPUCounters();
        var gpuUsage = GetGPUUsage(gpuCounters);

        Console.WriteLine(gpuUsage);
      }
      catch (Exception e)
      {
        Console.Error.Write(e);
      }

      Thread.Sleep(1000);
    }
  }
}