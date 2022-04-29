using System.Diagnostics;

namespace ProcessWatcher
{
  class Helpers
  {
    public static List<PerformanceCounter> GetCounters()
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

    public static float ComputeUsage(List<PerformanceCounter> gpuCounters)
    {
      gpuCounters.ForEach(x => x.NextValue());

      var result = gpuCounters.Sum(x => x.NextValue());

      return result;
    }
  }
}