using System;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Struct used to identify a move using least possible data.
  /// </summary>
  struct MoveIdentifier
  {
    /// <summary>
    /// Square the move is performed from.
    /// </summary>
    public Square From;

    /// <summary>
    /// Square move is performed to.
    /// </summary>
    public Square To;

    /// <summary>
    /// The piece type a pawn is promoted to in case the move we are trying to find is a promotion move.
    /// The value of this is irrelevant if the move we want to get is not a pawn promotion move.
    /// </summary>
    public Piece PromotionPiece;


    /// <summary>
    /// Initializes a new move identifier.
    /// </summary>
    /// <param name="from">Square move is done from.</param>
    /// <param name="to">Square move is done to.</param>
    /// <param name="promotionPiece">Piece a pawn will be promoted to.</param>
    public MoveIdentifier(Square from, Square to, Piece promotionPiece)
    {
      From = from;
      To = to;
      PromotionPiece = promotionPiece;
    }
  }
}
