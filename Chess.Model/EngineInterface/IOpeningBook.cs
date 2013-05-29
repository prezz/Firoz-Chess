using System;
using Chess.Model.Logic;


namespace Chess.Model.EngineInterface
{
  /// <summary>
  /// Interface an actual opening book must implement.
  /// </summary>
  interface IOpeningBook
  {
    /// <summary>
    /// Adds a position to the opening book.
    /// </summary>
    /// <param name="board">Board with the position to add.</param>
    /// <param name="adjustValue">value to adjust the boards value with.</param>
    void AddPosition(Board board, int adjustValue);

    /// <summary>
    /// Returns a move from the moveOrganizer if a move added to the book exists in it.
    /// </summary>
    /// <param name="board">Board to query best move for.</param>
    /// <param name="moveOrganizer">The move organizer to find a move in.</param>
    /// <returns>Move existing in opening book and contained in move organizer.</returns>
    Move Query(Board board, MoveOrganizer moveOrganizer);

    /// <summary>
    /// Saves the book.
    /// </summary>
    void Save();

    /// <summary>
    /// Loads the book.
    /// </summary>
    void Load();
  }
}
