using System;
using Chess.Model.EngineInterface;


namespace Chess.Model.Engine
{
  /// <summary>
  /// Interface an actual time control must implement.
  /// The time control class is responsible for calculating the amount of
  /// time the engine must search based on the clocks remaining time.
  /// </summary>
  class TimeControl : ITimeControl
  {
    /// <summary>
    /// Calculate the aimed search time allowed by the engine based on the current clock time.
    /// </summary>
    /// <param name="clockTime">The current remaining time of the clock.</param>
    /// <returns>The amount of time the engine is allowed to search.</returns>
    public TimeSpan CalculateAimedSearchTime(TimeSpan clockTime)
    {
      return TimeSpan.FromMilliseconds(clockTime.TotalMilliseconds / 38.0);
    }

    /// <summary>
    /// Calculate the maximum search time allowed by the engine based on the current clock time.
    /// </summary>
    /// <param name="clockTime">The current remaining time of the clock.</param>
    /// <returns>The amount of time the engine is allowed to search.</returns>
    public TimeSpan CalculateMaxSearchTime(TimeSpan clockTime)
    {
      return TimeSpan.FromMilliseconds(clockTime.TotalMilliseconds / 28.0);
    }
  }
}
