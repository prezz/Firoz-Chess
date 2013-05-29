using System;


namespace Chess.Model.Logic
{
  #region MoveType

  /// <summary>
  /// Enumeration of the different move types a move can be.
  /// </summary>
  enum MoveType
  {
    StandardMove,
    CastlingMove,
    PawnPromotionMove,
    EnPassantCaptureMove,
  };

  #endregion


  /// <summary>
  /// Represents an basic undoable chess move.
  /// </summary>
  class Move
  {
    /// <summary>
    /// The type of move.
    /// </summary>
    private MoveType m_moveType;

    /// <summary>
    /// Location move is done from.
    /// </summary>
    private Square m_from;

    /// <summary>
    /// Location move is done to.
    /// </summary>
    private Square m_to;

    /// <summary>
    /// The piece being moved.
    /// </summary>
    private Piece m_piece;

    /// <summary>
    /// The piece captured (if any) as a result of the move.
    /// </summary>
    private Piece m_capture;

    /// <summary>
    /// The piece to promote a pawn to.
    /// </summary>
    private Piece m_promoteTo;

    /// <summary>
    /// Location "Rook" is moved from.
    /// </summary>
    private Square m_rookFrom;

    /// <summary>
    /// Location "Rook" is moved to.
    /// </summary>
    private Square m_rookTo;

    /// <summary>
    /// The state of the board before move has been performed.
    /// </summary>
    private BoardState m_beforeState;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public Move(Board board, Square from, Square to)
    {
      m_moveType = MoveType.StandardMove;
      m_from = from;
      m_to = to;
      m_piece = board[from];
      m_capture = board[to];
      //m_promoteTo = Piece.None;
      //m_rookFrom = Square.None;
      //m_rookTo = Square.None;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="enPassantTarget"></param>
    public Move(Board board, Square from, Square to, Square enPassantTarget)
    {
      m_moveType = MoveType.EnPassantCaptureMove;
      m_from = from;
      m_to = to;
      m_piece = board[from];
      m_capture = board[enPassantTarget];
      //m_promoteTo = Piece.None;
      //m_rookFrom = Square.None;
      //m_rookTo = Square.None;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="promoteTo"></param>
    public Move(Board board, Square from, Square to, Piece promoteTo)
    {
      m_moveType = MoveType.PawnPromotionMove;
      m_from = from;
      m_to = to;
      m_piece = board[from];
      m_capture = board[to];
      m_promoteTo = promoteTo;
      //m_rookFrom = Square.None;
      //m_rookTo = Square.None;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="rookFrom"></param>
    /// <param name="rookTo"></param>
    public Move(Board board, Square from, Square to, Square rookFrom, Square rookTo)
    {
      m_moveType = MoveType.CastlingMove;
      m_from = from;
      m_to = to;
      m_piece = board[from];
      m_capture = Piece.None;
      //m_promoteTo = Piece.None;
      m_rookFrom = rookFrom;
      m_rookTo = rookTo;
    }

    /// <summary>
    /// Returns the type of move.
    /// </summary>
    public MoveType MoveType
    {
      get { return m_moveType; }
    }

    /// <summary>
    /// Returns location move is done from.
    /// </summary>
    public Square From
    {
      get { return m_from; }
    }

    /// <summary>
    /// Returns location move is done to.
    /// </summary>
    public Square To
    {
      get { return m_to; }
    }

    /// <summary>
    /// Returns the piece being moved.
    /// </summary>
    public Piece Piece
    {
      get { return m_piece; }
    }

    /// <summary>
    /// Returns the capture for this move.
    /// </summary>
    public Piece Capture
    {
      get { return m_capture; }
    }

    /// <summary>
    /// If the move as not yet been executed returns the piece the pawn is going to be promoted to.
    /// Otherwise it returns a pawn.
    /// </summary>
    public Piece PromoteTo
    {
      get { return (m_moveType == MoveType.PawnPromotionMove) ? m_promoteTo : Piece.None; }
    }

    /// <summary>
    /// Returns true if move is a capture move. Due to performance result
    /// of this 
    /// </summary>
    public bool IsCaptureMove
    {
      get { return m_capture != Piece.None; }
    }

    /// <summary>
    /// Performs the move. If move is only pseudo legal (putting own king in check)
    /// the move isn't executed.
    /// </summary>
    /// <returns>True if move could be carried out, false otherwise.</returns>
    public bool Execute(Board board)
    {
      switch (m_moveType)
      {
        case MoveType.StandardMove:
          return StandardExecute(board);

        case MoveType.CastlingMove:
          return CastlingExecute(board);

        case MoveType.PawnPromotionMove:
          return PawnPromotionExecute(board);

        case MoveType.EnPassantCaptureMove:
          return EnPassantExecute(board);
      }

      return false;
    }

    /// <summary>
    /// Takes back (undo) the move. Nothing is done to verify if undoing the move is actually
    /// valid for the board associated to this move.
    /// </summary>
    public void UnExecute(Board board)
    {
      switch (m_moveType)
      {
        case MoveType.StandardMove:
          StandardUnExecute(board);
          break;

        case MoveType.CastlingMove:
          CastlingUnExecute(board);
          break;

        case MoveType.PawnPromotionMove:
          PawnPromotionUnExecute(board);
          break;

        case MoveType.EnPassantCaptureMove:
          EnPassantUnExecute(board);
          break;
      }
    }

    /// <summary>
    /// Performs the move. If move is only pseudo legal (putting own king in check)
    /// the move isn't executed.
    /// </summary>
    /// <returns>True if move could be carried out, false otherwise.</returns>
    private bool StandardExecute(Board board)
    {
      BoardState afterState = m_beforeState = board.State;

      board.MovePiece(m_from, m_to);

      SetCastlingAvailabilety(ref afterState);
      SetEnPassentTarget(ref afterState);
      EndTurn(ref afterState);

      board.State = afterState;
      board.AddToHistory();

      if (board.IsCheck(m_beforeState.ColorToPlay))
      {
        StandardUnExecute(board);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Takes back (undo) the move. Nothing is done to verify if undoing the move is actually
    /// valid for the board associated to this move.
    /// </summary>
    private void StandardUnExecute(Board board)
    {
      board.RemoveFromHistory();

      board.MovePiece(m_to, m_from);
      board.PlacePiece(m_to, m_capture);

      board.State = m_beforeState;
    }

    /// <summary>
    /// Performs the move. I move is only pseudo legal (putting own king in check)
    /// the move isn't executed.
    /// </summary>
    /// <returns>True if move could be carried out, false otherwise.</returns>
    private bool CastlingExecute(Board board)
    {
      BoardState afterState = m_beforeState = board.State;

      //Kings move
      board.MovePiece(m_from, m_to);

      //Rooks move
      board.MovePiece(m_rookFrom, m_rookTo);

      SetCastlingAvailabilety(ref afterState);
      SetEnPassentTarget(ref afterState);
      EndTurn(ref afterState);

      board.State = afterState;
      board.AddToHistory();

      if (board.IsCheck(m_beforeState.ColorToPlay))
      {
        CastlingUnExecute(board);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Takes back (undo) the castling move. Nothing is done to verify if undoing the move is actually
    /// valid for the board associated to this move.
    /// </summary>
    private void CastlingUnExecute(Board board)
    {
      board.RemoveFromHistory();

      //Undoing rooks move
      board.MovePiece(m_rookTo, m_rookFrom);

      //Undoing kings move
      board.MovePiece(m_to, m_from);

      board.State = m_beforeState;
    }

    /// <summary>
    /// Performs the move. I move is only pseudo legal (putting own king in check)
    /// the move isn't executed.
    /// </summary>
    /// <returns>True if move could be carried out, false otherwise.</returns>
    private bool PawnPromotionExecute(Board board)
    {
      BoardState afterState = m_beforeState = board.State;

      board.MovePiece(m_from, m_to);
      board.PlacePiece(m_to, m_promoteTo);

      SetCastlingAvailabilety(ref afterState);
      SetEnPassentTarget(ref afterState);
      EndTurn(ref afterState);

      board.State = afterState;
      board.AddToHistory();

      if (board.IsCheck(m_beforeState.ColorToPlay))
      {
        PawnPromotionUnExecute(board);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Takes back (undo) the move. Nothing is done to verify if undoing the move is actually
    /// valid for the board associated to this move.
    /// </summary>
    private void PawnPromotionUnExecute(Board board)
    {
      board.RemoveFromHistory();

      board.PlacePiece(m_to, m_piece);
      board.MovePiece(m_to, m_from);
      board.PlacePiece(m_to, m_capture);

      board.State = m_beforeState;
    }

    /// <summary>
    /// Performs the move. I move is only pseudo legal (putting own king in check)
    /// the move isn't executed.
    /// </summary>
    /// <returns>True if move could be carried out, false otherwise.</returns>
    private bool EnPassantExecute(Board board)
    {
      BoardState afterState = m_beforeState = board.State;

      board.PlacePiece(m_beforeState.EnPassantTarget, Piece.None);
      board.MovePiece(m_from, m_to);

      SetCastlingAvailabilety(ref afterState);
      SetEnPassentTarget(ref afterState);
      EndTurn(ref afterState);

      board.State = afterState;
      board.AddToHistory();

      if (board.IsCheck(m_beforeState.ColorToPlay))
      {
        EnPassantUnExecute(board);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Takes back (undo) the En-Passant capture move. Nothing is done to verify if undoing the move is actually
    /// valid for the board associated to this move.
    /// </summary>
    private void EnPassantUnExecute(Board board)
    {
      board.RemoveFromHistory();

      board.MovePiece(m_to, m_from);
      board.PlacePiece(m_beforeState.EnPassantTarget, m_capture);

      board.State = m_beforeState;
    }

    /// <summary>
    /// Updates what types of castling is possible after the move has been performed. 
    /// </summary>
    /// <param name="afterState">The "BoardState" to update.</param>
    private void SetCastlingAvailabilety(ref BoardState afterState)
    {
      switch (From)
      {
        case Square.E1:
          afterState.WhiteCanCastleLong = false;
          afterState.WhiteCanCastleShort = false;
          break;

        case Square.E8:
          afterState.BlackCanCastleLong = false;
          afterState.BlackCanCastleShort = false;
          break;

        case Square.A1:
          afterState.WhiteCanCastleLong = false;
          break;

        case Square.A8:
          afterState.BlackCanCastleLong = false;
          break;

        case Square.H1:
          afterState.WhiteCanCastleShort = false;
          break;

        case Square.H8:
          afterState.BlackCanCastleShort = false;
          break;
      }

      switch (To)
      {
        case Square.A1:
          afterState.WhiteCanCastleLong = false;
          break;

        case Square.A8:
          afterState.BlackCanCastleLong = false;
          break;

        case Square.H1:
          afterState.WhiteCanCastleShort = false;
          break;

        case Square.H8:
          afterState.BlackCanCastleShort = false;
          break;
      }
    }

    /// <summary>
    /// Updates if the board has a pawn that can be hit En-Passant after the move has been performed.
    /// </summary>
    /// <param name="afterState">The "BoardState" to update.</param>
    private void SetEnPassentTarget(ref BoardState afterState)
    {
      if ((Board.Rank(From) == 1 && Board.Rank(To) == 3 && Piece == Piece.WhitePawn) ||
        (Board.Rank(From) == 6 && Board.Rank(To) == 4 && Piece == Piece.BlackPawn))
      {
        afterState.EnPassantTarget = To;
      }
      else
      {
        afterState.EnPassantTarget = Square.None;
      }
    }

    /// <summary>
    /// Updates the board state as a result of a move has been performed.
    /// </summary>
    /// <param name="afterState">The "BoardState" to update.</param>
    private void EndTurn(ref BoardState afterState)
    {
      if (Capture != Piece.None || Piece == Piece.WhitePawn || Piece == Piece.BlackPawn)
        afterState.NonHitAndPawnMovesPlayed = 0;
      else
        ++afterState.NonHitAndPawnMovesPlayed;

      switch (m_beforeState.ColorToPlay)
      {
        case PieceColor.White:
          afterState.ColorToPlay = PieceColor.Black;
          break;

        case PieceColor.Black:
          afterState.ColorToPlay = PieceColor.White;
          break;
      }
    }
  }
}
