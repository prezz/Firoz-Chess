using System;
using System.ComponentModel;
using Chess.Model.Logic;


namespace Chess.Model.EngineInterface
{
  /// <summary>
  /// Interface an actual search tree must implement.
  /// </summary>
  interface ISearchTree
  {
    /// <summary>
    /// Signals the engine that it should stop searching and return a null value from FindBestMove.
    /// </summary>
    void SignalStopSearch();

    /// <summary>
    /// Carries out a search returning the best move contained in moveOrganizer.
    /// </summary>
    /// <param name="board">Board to find a move for.</param>
    /// <param name="moveOrganizer">Move organizer to find best move in.</param>
    /// <param name="aimSearchTime">The search time to aim for. Actual search time might be between this and maxSearchTime.</param>
    /// <param name="maxSearchTime">The maximum amount of seconds to seach for move.</param>
    /// <param name="maxSearchDepth">Maximum depth for full width search.</param>
    /// <returns>The best move and the one that should be played, or null if search was aborted.</returns>
    Move FindBestMove(Board board, MoveOrganizer moveOrganizer, TimeSpan aimSearchTime, TimeSpan maxSearchTime, int maxSearchDepth);
  }
}
