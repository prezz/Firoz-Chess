using System;


namespace Chess.Model.Engine
{
  /// <summary>
  /// Part of the partial class BoardEvaluator.
  /// This part is responsible for evaluating some specific positions that is best avoided..
  /// </summary>
  partial class BoardEvaluator
  {
    /// <summary>
    /// Gives a penalty if king has moved left/right on the back rank with rook still positioned
    /// to the left/right.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void EvaluateKingRookPosition()
    {
      //King and Rook position for white.
      switch (m_board.WhiteKingLocation)
      {
        case Square.B1:
          if (m_board[Square.A1] == Piece.WhiteRook)
            m_whiteScore -= 30;
          break;

        case Square.C1:
          if (m_board[Square.A1] == Piece.WhiteRook || m_board[Square.B1] == Piece.WhiteRook)
            m_whiteScore -= 30;
          break;

        case Square.D1:
          if (m_board[Square.A1] == Piece.WhiteRook || m_board[Square.B1] == Piece.WhiteRook || m_board[Square.C1] == Piece.WhiteRook)
            m_whiteScore -= 30;
          break;

        case Square.F1:
          if (m_board[Square.H1] == Piece.WhiteRook || m_board[Square.G1] == Piece.WhiteRook)
            m_whiteScore -= 30;
          break;

        case Square.G1:
          if (m_board[Square.H1] == Piece.WhiteRook)
            m_whiteScore -= 30;
          break;
      }

      //King and Rook position for black.
      switch (m_board.BlackKingLocation)
      {
        case Square.B8:
          if (m_board[Square.A8] == Piece.BlackRook)
            m_blackScore -= 30;
          break;

        case Square.C8:
          if (m_board[Square.A8] == Piece.BlackRook || m_board[Square.B8] == Piece.BlackRook)
            m_blackScore -= 30;
          break;

        case Square.D8:
          if (m_board[Square.A8] == Piece.BlackRook || m_board[Square.B8] == Piece.BlackRook || m_board[Square.C8] == Piece.BlackRook)
            m_blackScore -= 30;
          break;

        case Square.F8:
          if (m_board[Square.H8] == Piece.BlackRook || m_board[Square.G8] == Piece.BlackRook)
            m_blackScore -= 30;
          break;

        case Square.G8:
          if (m_board[Square.H8] == Piece.BlackRook)
            m_blackScore -= 30;
          break;
      }
    }

    /// <summary>
    /// Gives a penalty if knight or bishop is blocking movement for the two center pawns if they are
    /// still in their initial position.
    /// </summary>
    private void EvaluateCenterPawnsBlocked()
    {
      if (m_board[Square.D2] == Piece.WhitePawn)
        if (m_board[Square.D3] == Piece.WhiteKnight || m_board[Square.D3] == Piece.WhiteBishop)
          m_whiteScore -= 30;

      if (m_board[Square.E2] == Piece.WhitePawn)
        if (m_board[Square.E3] == Piece.WhiteKnight || m_board[Square.E3] == Piece.WhiteBishop)
          m_whiteScore -= 30;

      if (m_board[Square.D7] == Piece.BlackPawn)
        if (m_board[Square.D6] == Piece.BlackKnight || m_board[Square.D6] == Piece.BlackBishop)
          m_blackScore -= 30;

      if (m_board[Square.E7] == Piece.BlackPawn)
        if (m_board[Square.E6] == Piece.BlackKnight || m_board[Square.E6] == Piece.BlackBishop)
          m_blackScore -= 30;
    }

    /// <summary>
    /// Evaluates specefic position charactaristics.
    /// </summary>
    private void EvaluatePositions()
    {
      EvaluateKingRookPosition();
      EvaluateCenterPawnsBlocked();
    }
  }
}
