using System.Diagnostics;

namespace ProcessWatcher.Metrics
{
  class Watcher
  {
    List<PerformanceCounter> counters = Helpers.GetCounters();

    Queue<float> usageQueue = new Queue<float>(10);

    public Watcher()
    {
      while (true)
      {
        Cycle();

        Thread.Sleep(1000);
      }
    }

    Task Cycle() => Task.Run(() =>
    {
      try
      {
        var gpuUsage = Helpers.ComputeUsage(counters);

        if (usageQueue.Count >= 10)
        {
          usageQueue.Dequeue();
        }

        usageQueue.Enqueue(gpuUsage);

        var average = usageQueue.Average();

        Console.WriteLine($"Current: {gpuUsage}");
        Console.WriteLine($"Average: {average}");
        Console.WriteLine("");
      }
      catch (Exception e)
      {
        Console.Error.Write(e);
      }
    });
  }
}
