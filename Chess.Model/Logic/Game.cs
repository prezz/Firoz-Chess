using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Represents a single game of chess. It is not possible to restart a game. To restart a game
  /// a new instance of this class must be created.
  /// </summary>
  class Game
  {
    /// <summary>
    /// The board on which the game is played.
    /// </summary>
    private Board m_board;

    /// <summary>
    /// Chess clock
    /// </summary>
    private IClock m_clock;

    /// <summary>
    /// Holds the currently playable moves.
    /// </summary>
    private MoveOrganizer m_possibleMoves;

    /// <summary>
    /// Holds the history of all previously (and undoable) played moves.
    /// </summary>
    private Stack<Move> m_undoMoveHistory;

    /// <summary>
    /// Holds subsequent moves that previously has been undone. This stack
    /// is cleared if a new move is played.
    /// </summary>
    private Stack<Move> m_redoMoveHistory;


    /// <summary>
    /// Event raised approximately every second when white is to play and whites clock changes value.
    /// </summary>
    public event EventHandler<ClockEventArgs> WhiteClockNotifier;

    /// <summary>
    /// Event raised approximately every second when black is to play and blacks clock changes value.
    /// </summary>
    public event EventHandler<ClockEventArgs> BlackClockNotifier;


    /// <summary>
    /// Initializes a new instance of the Game class.
    /// </summary>
    /// <param name="board">A board holding the layout this game will be initialized with. If this parameter is null a standard layout will be created.</param>
    /// <param name="clockConfiguration">The type of clock to use for this game.</param>
    public Game(Board board, ClockConfiguration clockConfiguration)
    {
      m_board = (board == null) ? new Board() : new Board(board);

      m_possibleMoves = new MoveOrganizer();
      m_undoMoveHistory = new Stack<Move>();
      m_redoMoveHistory = new Stack<Move>();

      SetClockConfiguration(clockConfiguration);

      HandleGameHasChanged();
    }

    /// <summary>
    /// The board on which the game is played.
    /// </summary>
    public Board Board
    {
      get { return m_board; }
    }

    /// <summary>
    /// Returns the currently possible moves.
    /// </summary>
    public MoveOrganizer Moves
    {
      get { return m_possibleMoves; }
    }

    /// <summary>
    /// Returns the clock instance of the game.
    /// </summary>
    public IClock Clock
    {
      get { return m_clock; }
    }

    /// <summary>
    /// Sets the clock configuration. Setting the configuration will
    /// only have effect if the game hasn't started yet.
    /// </summary>
    /// <param name="clockConfiguration">The configuration to set</param>
    public void SetClockConfiguration(ClockConfiguration clockConfiguration)
    {
      if (UndoCount == 0 && RedoCount == 0)
      {
        if (m_clock != null)
        {
          m_clock.StopClock();
          m_clock.WhiteClockNotifier -= WhiteClockEventHandler;
          m_clock.BlackClockNotifier -= BlackClockEventHandler;
        }

        switch (clockConfiguration.ClockType)
        {
          case ClockType.Conventional:
            m_clock = new ConventionalClock(clockConfiguration.ConventionalMoves, clockConfiguration.ConventionalTime);
            break;

          case ClockType.Incremental:
            m_clock = new IncrementalClock(clockConfiguration.IncrementStartTime, clockConfiguration.IncrementPlusTime);
            break;

          case ClockType.None:
            m_clock = new NoneClock();
            break;
        }

        m_clock.WhiteClockNotifier += WhiteClockEventHandler;
        m_clock.BlackClockNotifier += BlackClockEventHandler;
        m_clock.RaiseTimeNotifyEvent();
      }
    }

    /// <summary>
    /// The number of moves that can be undone.
    /// </summary>
    public int UndoCount
    {
      get { return m_undoMoveHistory.Count; }
    }

    /// <summary>
    /// The number of moves that can be redone.
    /// </summary>
    public int RedoCount
    {
      get { return m_redoMoveHistory.Count; }
    }

    /// <summary>
    /// Makes a move.
    /// </summary>
    /// <param name="move">The move to perform.</param>
    public bool MakeMove(Move move)
    {
      if (move.Execute(m_board))
      {
        switch (m_board.State.ColorToPlay)
        {
          case PieceColor.White:
            m_clock.EndBlackTurn();
            break;

          case PieceColor.Black:
            m_clock.EndWhiteTurn();
            break;
        }

        m_undoMoveHistory.Push(move);
        m_redoMoveHistory.Clear();
        HandleGameHasChanged();

        switch (m_board.State.ColorToPlay)
        {
          case PieceColor.White:
            m_clock.BeginWhiteTurn();
            break;

          case PieceColor.Black:
            m_clock.BeginBlackTurn();
            break;
        }

        return true;
      }

      return false;
    }

    /// <summary>
    /// Undo the last move played.
    /// </summary>
    /// <returns>The move that has been undone. If undo is impossible null is returned.</returns>
    public Move Undo()
    {
      if (m_undoMoveHistory.Count > 0)
      {
        m_clock.Undo();

        Move move = m_undoMoveHistory.Pop();
        move.UnExecute(m_board);
        m_redoMoveHistory.Push(move);

        HandleGameHasChanged();
        return move;
      }

      return null;
    }

    /// <summary>
    /// Redo the last move that has been undone.
    /// </summary>
    /// <returns>The move that has been redone. If redo was imposible null is returned.</returns>
    public Move Redo()
    {
      if (m_redoMoveHistory.Count > 0)
      {
        m_clock.Redo();

        Move move = m_redoMoveHistory.Pop();
        move.Execute(m_board);
        m_undoMoveHistory.Push(move);

        HandleGameHasChanged();
        return move;
      }

      return null;
    }

    /// <summary>
    /// Forces the the clock events to be raised
    /// </summary>
    public void RaiseClockNotifyEvent()
    {
      m_clock.RaiseTimeNotifyEvent();
    }

    /// <summary>
    /// Returns the current status of this game.
    /// </summary>
    /// <returns>The current "GameStatus".</returns>
    public GameStatus GetGameStatus()
    {
      if (m_possibleMoves.Count == 0)
      {
        if (m_board.ColorToPlayIsCheck())
        {
          switch (m_board.State.ColorToPlay)
          {
            case PieceColor.White:
              return GameStatus.BlackCheckmateVictory;

            case PieceColor.Black:
              return GameStatus.WhiteCheckmateVictory;
          }
        }

        return GameStatus.StalemateDraw;
      }

      if (IsDrawInsufficientPieces())
        return GameStatus.InsufficientPiecesDraw;

      if (IsDrawRepetitionOfMoves())
        return GameStatus.MoveRepetitionDraw;

      if (IsDrawFiftyMoveRule())
        return GameStatus.FiftyMovesDraw;

      if (m_clock.WhiteTimeOut)
        return GameStatus.BlackTimeVictory;

      if (m_clock.BlackTimeOut)
        return GameStatus.WhiteTimeVictory;

      if (m_board.ColorToPlayIsCheck())
      {
        switch (m_board.State.ColorToPlay)
        {
          case PieceColor.White:
            return GameStatus.WhiteIsCheck;

          case PieceColor.Black:
            return GameStatus.BlackIsCheck;
        }
      }

      return GameStatus.Normal;
    }

    /// <summary>
    /// Handles responsible for generating moves and updating game state every time the game (board) changes.
    /// </summary>
    private void HandleGameHasChanged()
    {
      m_possibleMoves.Clear();
      m_board.GeneratePseudoLegalMoves(m_possibleMoves);
      m_possibleMoves.RemoveSelfCheckingMoves(m_board);
    }

    /// <summary>
    /// Used to obtain if the game is a draw due to the current board has been represented tree times in the game.
    /// </summary>
    /// <returns>True if the current board has been repeated three times. False otherwise.</returns>
    private bool IsDrawRepetitionOfMoves()
    {
      return m_board.BoardHistoryFrequency() == 3;
    }

    /// <summary>
    /// Used to obtain if a game is a draw due to 50 consecutive moves of white and of black without
    /// any piece taken and any pawn moves.
    /// </summary>
    /// <returns>True if Draw by 50 move rule. False otherwise.</returns>
    private bool IsDrawFiftyMoveRule()
    {
      return m_board.State.NonHitAndPawnMovesPlayed == 100;
    }

    /// <summary>
    /// Checks if the game is a draw because one of the following endings arises:
    /// (a) king against king;
    /// (b) king against king with only bishop or knight; 
    /// (c) king and bishop against king and bishop, with both bishops on diagonals of the same color. 
    /// </summary>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private bool IsDrawInsufficientPieces()
    {
      bool N = false;  //white knight
      bool n = false;  //black knight
      bool B = false;  //white bishop
      bool b = false;  //black bishop
      bool BW = false;  //white bishop on white square
      bool bw = false;  //black bishop on white square

      foreach (Square square in m_board)
      {
        switch (m_board[square])
        {
          case Piece.WhiteKnight:
            if (N)
              return false;

            N = true;
            break;

          case Piece.BlackKnight:
            if (n)
              return false;

            n = true;
            break;

          case Piece.WhiteBishop:
            if (B)
              return false;

            B = true;
            BW = Board.IsWhiteSquare(square);
            break;

          case Piece.BlackBishop:
            if (b)
              return false;

            b = true;
            bw = Board.IsWhiteSquare(square);
            break;

          case Piece.None:
          case Piece.WhiteKing:
          case Piece.BlackKing:
            //Nothing
            break;

          default:
            return false;
        }
      }

      //only kings
      if (!B && !b && !N && !n)
        return true;

      //only kings and one bishop
      if ((B && !b && !N && !n) || (!B && b && !N && !n))
        return true;

      //only kings and one knight
      if ((!B && !b && N && !n) || (!B && !b && !N && n))
        return true;

      //only kings and one bishop on each side, both on squares with same color
      if (B && b && !N && !n && (bw == BW))
        return true;

      return false;
    }

    /// <summary>
    /// Event handler for the current games time notifier.
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event arguments</param>
    private void WhiteClockEventHandler(object sender, ClockEventArgs e)
    {
      if (WhiteClockNotifier != null)
        WhiteClockNotifier(this, e);
    }

    /// <summary>
    /// Event handler for the current games time notifier.
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event arguments</param>
    private void BlackClockEventHandler(object sender, ClockEventArgs e)
    {
      if (BlackClockNotifier != null)
        BlackClockNotifier(this, e);
    }
  }
}
