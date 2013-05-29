using System;
using Chess.Model.Logic;


namespace Chess.Model.EngineInterface
{
  /// <summary>
  /// Interface an actual board avaluator class must implement.
  /// </summary>
  interface IBoardEvaluator
  {
    /// <summary>
    /// The alpha value used in search. Must be negative.
    /// </summary>
    int AlphaValue { get; }

    /// <summary>
    /// Value used when a player is mated. Must be a negative value greater then AlphaValue.
    /// </summary>
    int MateValue { get; }

    /// <summary>
    /// Returns value of a piece.
    /// </summary>
    /// <param name="piece">Piece to get value for.</param>
    /// <returns>The value.</returns>
    int PieceValue(Piece piece);

    /// <summary>
    /// Evaluates a board and returns a score. The score must always be greater
    /// then AlphaValue and MateValue and smaller then BetaValue.
    /// </summary>
    /// <param name="board">Board to evalueate.</param>
    /// <returns>The score for the board.</returns>
    int EvaluatePosition(Board board);
  }
}
