using System;


namespace Chess.Model.EngineInterface
{
  /// <summary>
  /// Interface an actual time control must implement.
  /// The time control class is responsible for calculating the amount of
  /// time the engine must search based on the clocks remaining time.
  /// </summary>
  interface ITimeControl
  {
    /// <summary>
    /// Calculate the aimed search time allowed by the engine based on the current clock time.
    /// </summary>
    /// <param name="clockTime">The current remaining time of the clock.</param>
    /// <returns>The amount of time the engine is allowed to search.</returns>
    TimeSpan CalculateAimedSearchTime(TimeSpan clockTime);

    /// <summary>
    /// Calculate the maximum search time allowed by the engine based on the current clock time.
    /// </summary>
    /// <param name="clockTime">The current remaining time of the clock.</param>
    /// <returns>The amount of time the engine is allowed to search.</returns>
    TimeSpan CalculateMaxSearchTime(TimeSpan clockTime);
  }
}
