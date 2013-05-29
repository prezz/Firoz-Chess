using System;
using Chess.Model.Logic;


namespace Chess.Model
{
  /// <summary>
  /// Class making it possible to edit the board and thereby making custom position setups.
  /// </summary>
  public class PositionEditor
  {
    /// <summary>
    /// Board holding the original layout the position editor is editing.
    /// Used to reset the position setup.
    /// </summary>
    private Board m_baseBoard;

    /// <summary>
    /// Board that is being edited.
    /// </summary>
    private Board m_editBoard;


    /// <summary>
    /// Event raised when the m_editBoard is changed.
    /// </summary>
    public event EventHandler<BoardChangedEventArgs> BoardChanged;


    /// <summary>
    /// Initializes a new instance of the position editor.
    /// </summary>
    /// <param name="board"></param>
    internal PositionEditor(Board board)
    {
      m_baseBoard = new Board(board);
      m_editBoard = new Board(board);
    }

    /// <summary>
    /// The edited board and position.
    /// </summary>
    internal Board Board
    {
      get { return m_editBoard; }
    }

    /// <summary>
    /// Gets or sets the piece on a specific square.
    /// </summary>
    /// <param name="square">Location of ppiece to get or set.</param>
    /// <returns>Square at a specefic location.</returns>
    public Piece this[Square square]
    {
      get { return m_editBoard[square]; }

      set
      {
        m_editBoard.PlacePiece(square, value);

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// Get or sets the color it is to play.
    /// </summary>
    public PieceColor ColorToPlay
    {
      get { return m_editBoard.State.ColorToPlay; }

      set
      {
        BoardState state = m_editBoard.State;
        state.ColorToPlay = value;
        m_editBoard.State = state;

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// Get or sets the en passant target.
    /// </summary>
    public Square EnpassantTarget
    {
      get { return m_editBoard.State.EnPassantTarget; }

      set
      {
        BoardState state = m_editBoard.State;
        state.EnPassantTarget = value;
        m_editBoard.State = state;

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// Get or sets whether white can castle short.
    /// </summary>
    public bool WhiteCanCastleShort
    {
      get { return m_editBoard.State.WhiteCanCastleShort; }

      set
      {
        BoardState state = m_editBoard.State;
        state.WhiteCanCastleShort = value;
        m_editBoard.State = state;

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// Get or sets whether white can castle long.
    /// </summary>
    public bool WhiteCanCastleLong
    {
      get { return m_editBoard.State.WhiteCanCastleLong; }

      set
      {
        BoardState state = m_editBoard.State;
        state.WhiteCanCastleLong = value;
        m_editBoard.State = state;

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// Get or sets whether black can castle short.
    /// </summary>
    public bool BlackCanCastleShort
    {
      get { return m_editBoard.State.BlackCanCastleShort; }

      set
      {
        BoardState state = m_editBoard.State;
        state.BlackCanCastleShort = value;
        m_editBoard.State = state;

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// Get or sets whether black can castle long.
    /// </summary>
    public bool BlackCanCastleLong
    {
      get { return m_editBoard.State.BlackCanCastleLong; }

      set
      {
        BoardState state = m_editBoard.State;
        state.BlackCanCastleLong = value;
        m_editBoard.State = state;

        if (BoardChanged != null)
          BoardChanged(this, new BoardChangedEventArgs(null));
      }
    }

    /// <summary>
    /// resets the position to the original layout.
    /// </summary>
    public void ResetPosition()
    {
      m_editBoard = new Board(m_baseBoard);

      if (BoardChanged != null)
        BoardChanged(this, new BoardChangedEventArgs(null));
    }

    /// <summary>
    /// Clears the entire board for pieces.
    /// </summary>
    public void ClearBoard()
    {
      BoardState state = m_editBoard.State;
      state.ColorToPlay = PieceColor.None;
      state.EnPassantTarget = Square.None;
      state.WhiteCanCastleShort = false;
      state.WhiteCanCastleLong = false;
      state.BlackCanCastleShort = false;
      state.BlackCanCastleLong = false;
      m_editBoard.State = state;

      foreach (Square square in m_editBoard)
        m_editBoard.PlacePiece(square, Piece.None);

      if (BoardChanged != null)
        BoardChanged(this, new BoardChangedEventArgs(null));
    }

    /// <summary>
    /// Validates if current position is actually valid.
    /// </summary>
    /// <returns>True if position is valid. False otherwise.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public bool ValidatePosition()
    {
      Board board = m_editBoard;
      BoardState state = board.State;

      //----Validating piece count----
      byte K = 0; //white kings
      byte k = 0; //black kings
      byte Q = 0; //white queens
      byte q = 0; //black queens
      byte R = 0; //white rooks
      byte r = 0; //black rooks
      byte B = 0; //white bishops
      byte b = 0; //black bishops
      byte N = 0; //white knights
      byte n = 0; //black knights
      byte P = 0; //white pawns
      byte p = 0; //black pawns
      foreach (Square square in board)
      {
        if (board[square] == Piece.WhiteKing)
          ++K;

        if (board[square] == Piece.BlackKing)
          ++k;

        if (board[square] == Piece.WhiteQueen)
          ++Q;

        if (board[square] == Piece.BlackQueen)
          ++q;

        if (board[square] == Piece.WhiteRook)
          ++R;

        if (board[square] == Piece.BlackRook)
          ++r;

        if (board[square] == Piece.WhiteBishop)
          ++B;

        if (board[square] == Piece.BlackBishop)
          ++b;

        if (board[square] == Piece.WhiteKnight)
          ++N;

        if (board[square] == Piece.BlackKnight)
          ++n;

        if (board[square] == Piece.WhitePawn)
          ++P;

        if (board[square] == Piece.BlackPawn)
          ++p;
      }

      if ((K + Q + R + B + N + P) > 16 || (k + q + r + b + n + p) > 16) //total
        return false;

      if (K != 1 || k != 1) //kings
        return false;

      if (P > 8 || p > 8) //pawns
        return false;

      if ((Q + R + B + N - 7) > (8 - P) || (q + r + b + n - 7) > (8 - p)) //ekstra due to promotion
        return false;


      //----Validate color to play----
      if (!(state.ColorToPlay == PieceColor.White || state.ColorToPlay == PieceColor.Black))
        return false;


      //----Validate pawn locations----
      for (Square square = Square.A1; square <= Square.H1; ++square)
      {
        Piece piece = board[square];
        if (piece == Piece.WhitePawn || piece == Piece.BlackPawn)
          return false;
      }

      for (Square square = Square.A8; square <= Square.H8; ++square)
      {
        Piece piece = board[square];
        if (piece == Piece.WhitePawn || piece == Piece.BlackPawn)
          return false;
      }


      //----Validating if a king can be captured----
      if (state.ColorToPlay == PieceColor.White)
        if (board.IsCheck(PieceColor.Black))
          return false;

      if (state.ColorToPlay == PieceColor.Black)
        if (board.IsCheck(PieceColor.White))
          return false;


      //----Validating En Passant----
      if (state.EnPassantTarget != Square.None)
      {
        if (!(Board.Rank(state.EnPassantTarget) == 3 || Board.Rank(state.EnPassantTarget) == 4))
          return false;

        if (state.ColorToPlay == PieceColor.White && board[state.EnPassantTarget] != Piece.BlackPawn)
          return false;

        if (state.ColorToPlay == PieceColor.Black && board[state.EnPassantTarget] != Piece.WhitePawn)
          return false;
      }


      //----Validating castling----
      if (state.WhiteCanCastleShort)
        if (board[Square.E1] != Piece.WhiteKing || board[Square.H1] != Piece.WhiteRook)
          return false;

      if (state.WhiteCanCastleLong)
        if (board[Square.E1] != Piece.WhiteKing || board[Square.A1] != Piece.WhiteRook)
          return false;

      if (state.BlackCanCastleShort)
        if (board[Square.E8] != Piece.BlackKing || board[Square.H8] != Piece.BlackRook)
          return false;

      if (state.BlackCanCastleLong)
        if (board[Square.E8] != Piece.BlackKing || board[Square.A8] != Piece.BlackRook)
          return false;

      return true;
    }
  }
}
