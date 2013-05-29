using System;
using System.Collections.Generic;
using Chess.Model.Logic;
using Chess.Model.EngineInterface;
using Chess.Model.Engine;


namespace Chess.Model
{
  /// <summary>
  /// This is the entry class to the whole chess assambly. Instantiate and use this class in your chess application.
  /// </summary>
  public class ChessFacade
  {
    /// <summary>
    /// Engines opening book.
    /// </summary>
    private IOpeningBook m_openingBook;

    /// <summary>
    /// Engines evaluator.
    /// </summary>
    private IBoardEvaluator m_evaluator;

    /// <summary>
    /// Engines search tree.
    /// </summary>
    private ISearchTree m_searchTree;

    /// <summary>
    /// Responsible for calculating the allowed search time based on current clock time.
    /// </summary>
    private ITimeControl m_timeControl;

    /// <summary>
    /// Class responsible for managing the engine and the seperate thread the engine runs in.
    /// </summary>
    private EngineManager m_engineManager;

    /// <summary>
    /// Configuration of the Chess engine.
    /// </summary>
    private EngineConfiguration m_engineConfiguration;

    /// <summary>
    /// Configuration of the Chess clock.
    /// </summary>
    private ClockConfiguration m_clockConfiguration;

    /// <summary>
    /// Reference to the current chess game in progress.
    /// </summary>
    private Game m_currentGame;

    /// <summary>
    /// Stack of previous games played.
    /// </summary>
    private Stack<Game> m_previousGames;

    /// <summary>
    /// Stack of subsequent games played. (I player has gone back to replay a previous game)
    /// </summary>
    private Stack<Game> m_subsequentGames;

    /// <summary>
    /// Copy of the current games board. Used to return the board layout
    /// as the current games board will change when engine is thinking due to searching.
    /// </summary>
    private Board m_boardCopy;

    /// <summary>
    /// Event raised when the chess board has changed.
    /// </summary>
    public event EventHandler<BoardChangedEventArgs> BoardChanged;

    /// <summary>
    /// Event raised when white has a pawn that is promoted promoted.
    /// </summary>
    public event EventHandler<PromotionEventArgs> WhitePawnPromoted;

    /// <summary>
    /// Event raised when black has a pawn that is to be promoted.
    /// </summary>
    public event EventHandler<PromotionEventArgs> BlackPawnPromoted;

    /// <summary>
    /// Event raised when status of the game chenges to somthing special.
    /// (e.g. Checkmate victory, stalemate draw, player is check and etc.)
    /// </summary>
    public event EventHandler<GameStatusEventArgs> StatusInfo;

    /// <summary>
    /// Event raised in regular intervals with the time left for white player.
    /// </summary>
    public event EventHandler<ClockEventArgs> WhiteClockNotifier;

    /// <summary>
    /// Event raised in regular intervals with the time left for black player.
    /// </summary>
    public event EventHandler<ClockEventArgs> BlackClockNotifier;


    /// <summary>
    /// Instantiates a new instance of the chess facade.
    /// </summary>
    public ChessFacade()
    {
      m_openingBook = new OpeningBook();
      m_evaluator = new BoardEvaluator();
      m_searchTree = new SearchTree(m_evaluator);
      m_timeControl = new TimeControl();
      m_engineManager = new EngineManager(m_openingBook, m_searchTree, m_timeControl);
      m_engineManager.MoveFound += EngineMoveFound;

      m_engineConfiguration = new EngineConfiguration(true, true, new TimeSpan(0, 1, 0), 25);
      m_clockConfiguration = new ClockConfiguration(ClockType.Conventional, 40, new TimeSpan(0, 25, 0), new TimeSpan(0, 15, 0), new TimeSpan(0, 0, 10));

      m_currentGame = new Game(null, m_clockConfiguration);
      m_currentGame.WhiteClockNotifier += WhiteClockEventHandler;
      m_currentGame.BlackClockNotifier += BlackClockEventHandler;
      m_boardCopy = new Board(m_currentGame.Board);

      m_previousGames = new Stack<Game>();
      m_subsequentGames = new Stack<Game>();
    }

    /// <summary>
    /// Gets or sets the configuration of the the chess engine.
    /// </summary>
    public EngineConfiguration EngineConfiguration
    {
      get { return m_engineConfiguration; }
      set { m_engineConfiguration = value; }
    }

    /// <summary>
    /// Gets or sets the configuration of the the chess clock.
    /// </summary>
    public ClockConfiguration ClockConfiguration
    {
      get { return m_clockConfiguration; }

      set
      {
        m_clockConfiguration = value;
        m_currentGame.SetClockConfiguration(m_clockConfiguration);
      }
    }

    /// <summary>
    /// Get will return an instance of PositionEditor object with a board layout equal to the current
    /// board layout of the game. This layout can then be changed. When setting
    /// this new layout a new game will be created with started from this changed layout.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.OutputWriter.Write(System.String)")]
    public PositionEditor PositionEditor
    {
      get
      {
        m_currentGame.Clock.StopClock();
        m_engineManager.Abort();

        HandleChangedGameState(null, false, false);
        return new PositionEditor(m_currentGame.Board);
      }

      set
      {
        if (value != null)
        {
          if (value.ValidatePosition())
          {
            m_currentGame.Clock.StopClock();
            m_engineManager.Abort();

            NewGameFromBoard(value.Board);
          }
          else
          {
            OutputWriter.Write("Position editor board is invalid. Failed to set it");
          }
        }
      }
    }

    /// <summary>
    /// Raises the board changed event and the clock notifier events. This is usefull
    /// if neading to redraw the userinterface after instaniating this chess facade class
    /// an having attached the event handlers.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
    public void RaiseEvents()
    {
      BoardChanged(this, new BoardChangedEventArgs(null));
      m_currentGame.RaiseClockNotifyEvent();
    }

    /// <summary>
    /// Starts a new game of chess.
    /// </summary>
    public void NewGame()
    {
      NewGameFromBoard(null);
    }

    /// <summary>
    /// Performs a single move.
    /// </summary>
    /// <param name="from">Where to move from.</param>
    /// <param name="to">Where to move to.</param>
    /// <returns>True if the move was possible. False otherwise.</returns>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    public bool PerformMove(Square from, Square to)
    {
      if (!m_engineManager.Thinking)
      {
        Move move = m_currentGame.Moves.Find(CreateMoveIdentifier(from, to));
        if (move != null)
        {
          m_currentGame.MakeMove(move);
          HandleChangedGameState(move, m_engineConfiguration.EngineAutoPlay, true);
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Forces the engine to make the current move (if possible).
    /// If the engine currently isn't thinking it starts thinking.
    /// If its already is thinking, it aborts an plays as fast as possible.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    public void ForceEngineToMove()
    {
      if (!m_engineManager.Thinking)
        HandleChangedGameState(null, true, false);
    }

    /// <summary>
    /// Returns the piece on a specefic square.
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public Piece PieceAt(Square square)
    {
      return m_boardCopy[square];
    }

    /// <summary>
    /// Number of previous games played.
    /// </summary>
    public int PreviousGameCount
    {
      get { return m_previousGames.Count; }
    }

    /// <summary>
    /// Number of subsequent games played.
    /// </summary>
    public int SubsequentGameCount
    {
      get { return m_subsequentGames.Count; }
    }

    /// <summary>
    /// Number of moves that can be undone.
    /// </summary>
    public int UndoCount
    {
      get { return m_currentGame.UndoCount; }
    }

    /// <summary>
    /// Number of moves that can be redone.
    /// </summary>
    public int RedoCount
    {
      get { return m_currentGame.RedoCount; }
    }

    /// <summary>
    /// Undo the last move.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    public void UndoMove()
    {
      m_currentGame.Clock.StopClock();
      m_engineManager.Abort();

      Move move = m_currentGame.Undo();
      HandleChangedGameState(move, false, false);
    }

    /// <summary>
    /// Redo the last move that was undone.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    public void RedoMove()
    {
      m_currentGame.Clock.StopClock();
      m_engineManager.Abort();

      Move move = m_currentGame.Redo();
      HandleChangedGameState(move, false, false);
    }

    /// <summary>
    /// Returns to the previous game.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    public void PreviousGame()
    {
      if (m_previousGames.Count > 0)
      {
        m_currentGame.Clock.StopClock();
        m_currentGame.WhiteClockNotifier -= WhiteClockEventHandler;
        m_currentGame.BlackClockNotifier -= BlackClockEventHandler;

        m_engineManager.Abort();

        m_subsequentGames.Push(m_currentGame);
        m_currentGame = m_previousGames.Pop();
        m_currentGame.WhiteClockNotifier += WhiteClockEventHandler;
        m_currentGame.BlackClockNotifier += BlackClockEventHandler;
        m_currentGame.RaiseClockNotifyEvent();

        HandleChangedGameState(null, false, false);
      }
    }

    /// <summary>
    /// Returns to the subsequent game.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    public void SubsequentGame()
    {
      if (m_subsequentGames.Count > 0)
      {
        m_currentGame.Clock.StopClock();
        m_currentGame.WhiteClockNotifier -= WhiteClockEventHandler;
        m_currentGame.BlackClockNotifier -= BlackClockEventHandler;

        m_engineManager.Abort();

        m_previousGames.Push(m_currentGame);
        m_currentGame = m_subsequentGames.Pop();
        m_currentGame.WhiteClockNotifier += WhiteClockEventHandler;
        m_currentGame.BlackClockNotifier += BlackClockEventHandler;
        m_currentGame.RaiseClockNotifyEvent();

        HandleChangedGameState(null, false, false);
      }
    }

    /// <summary>
    /// Loads the opening book
    /// </summary>
    public void LoadOpeningBook()
    {
      m_openingBook.Load();
    }

    /// <summary>
    /// Adds the current board to the engines opening book.
    /// </summary>
    /// <param name="adjustValue">value to adjust the boards value with.</param>
    public void AddToOpeningBook(int adjustValue)
    {
      m_openingBook.AddPosition(m_currentGame.Board, adjustValue);
    }

    /// <summary>
    /// Saves the opening book
    /// </summary>
    public void SaveOpeningBook()
    {
      m_openingBook.Save();
    }

    /// <summary>
    /// Returns the valid squares a piece can move to.
    /// </summary>
    /// <param name="from">Location of the piece valid destination squares are returned for.</param>
    /// <returns>Array of valid destination squares.</returns>
    public Square[] GetValidSquaresForPiece(Square from)
    {
      List<Square> buffer = new List<Square>();

      if (!m_engineManager.Thinking)
        foreach (Move move in m_currentGame.Moves)
          if (move.From == from)
            buffer.Add(move.To);

      return buffer.ToArray();
    }

    /// <summary>
    /// Users of this class should call this method when quitting as this allows
    /// the computer players thinking thread to abort in a safe manner if it is running.
    /// </summary>
    public void Quit()
    {
      m_currentGame.WhiteClockNotifier -= WhiteClockEventHandler;
      m_currentGame.BlackClockNotifier -= BlackClockEventHandler;
      m_engineManager.MoveFound -= EngineMoveFound;

      m_currentGame.Clock.StopClock();
      m_engineManager.Abort();
    }

    /// <summary>
    /// Returns the rank (or row) the square is located on.
    /// </summary>
    /// <param name="square">Valid arguments are all squares except Square.NoSquare.</param>
    /// <returns>The rank [0..7], viewed from white player, where square is located on the board.</returns>
    public static int Rank(Square square)
    {
      return Board.Rank(square);
    }

    /// <summary>
    /// Returns the file (or column) the square is located on.
    /// </summary>
    /// <param name="square">Valid arguments are all squares except Square.NoSquare.</param>
    /// <returns>The rank [0..7], left to right viewed from white player, where square is located on the board.</returns>
    public static int File(Square square)
    {
      return Board.File(square);
    }

    /// <summary>
    /// Converts a rank and file to a square.
    /// </summary>
    /// <param name="file">Valid arguments are [0..7] left to right viewed from white player.</param>
    /// <param name="rank">Valid arguments are [0..7] viewed from white player.</param>
    /// <returns></returns>
    public static Square Position(int file, int rank)
    {
      return Board.Position(file, rank);
    }

    /// <summary>
    /// Event handler called by the engine manager when a move has been found.
    /// </summary>
    /// <param name="sender">sender of event which is the engine manager.</param>
    /// <param name="e">Event arguments from the engine manager.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.OutputWriter.Write(System.String)")]
    private void EngineMoveFound(object sender, MoveFoundEventArgs e)
    {
      Move move = e.Move;
      if (!m_currentGame.MakeMove(move))
        OutputWriter.Write("Engine tried playing an invalid move");

      HandleChangedGameState(move, false, true);
    }

    /// <summary>
    /// Starts a new game with a board layout equal to another board.
    /// </summary>
    /// <param name="board">Board holding the position new game also will have. If this value is null a standard chess layout is created.</param>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    private void NewGameFromBoard(Board board)
    {
      m_currentGame.Clock.StopClock();
      m_currentGame.WhiteClockNotifier -= WhiteClockEventHandler;
      m_currentGame.BlackClockNotifier -= BlackClockEventHandler;

      m_engineManager.Abort();

      m_previousGames.Push(m_currentGame);
      while (m_subsequentGames.Count > 0)
        m_previousGames.Push(m_subsequentGames.Pop());

      m_currentGame = new Game(board, m_clockConfiguration);
      m_currentGame.WhiteClockNotifier += WhiteClockEventHandler;
      m_currentGame.BlackClockNotifier += BlackClockEventHandler;
      m_currentGame.RaiseClockNotifyEvent();

      HandleChangedGameState(null, false, false);
    }

    /// <summary>
    /// Creates a key that can be used to return an actual "Move" from the "MoveOrganizer"
    /// This method is also responsible for raising the Pawn promotion events.
    /// </summary>
    /// <param name="from">Where to move from.</param>
    /// <param name="to">Where to move to.</param>
    /// <returns>Key matching the requested move.</returns>
    private MoveIdentifier CreateMoveIdentifier(Square from, Square to)
    {
      MoveIdentifier result;
      result.From = from;
      result.To = to;
      result.PromotionPiece = Piece.None;

      if (Board.Rank(to) == 7 && m_currentGame.Board[from] == Piece.WhitePawn)
      {
        result.PromotionPiece = Piece.WhiteQueen;
        if (m_currentGame.Moves.Find(result) == null)
        {
          result.PromotionPiece = Piece.None;
        }
        else if (WhitePawnPromoted != null)
        {
          PromotionEventArgs promotionArgs = new PromotionEventArgs(PromotionPiece.Queen);
          WhitePawnPromoted(this, promotionArgs);
          switch (promotionArgs.PromotionPiece)
          {
            case PromotionPiece.Bishop:
              result.PromotionPiece = Piece.WhiteBishop;
              break;

            case PromotionPiece.Knight:
              result.PromotionPiece = Piece.WhiteKnight;
              break;

            case PromotionPiece.Rook:
              result.PromotionPiece = Piece.WhiteRook;
              break;
          }
        }
      }

      if (Board.Rank(to) == 0 && m_currentGame.Board[from] == Piece.BlackPawn)
      {
        result.PromotionPiece = Piece.BlackQueen;
        if (m_currentGame.Moves.Find(result) == null)
        {
          result.PromotionPiece = Piece.None;
        }
        else if (BlackPawnPromoted != null)
        {
          PromotionEventArgs promotionArgs = new PromotionEventArgs(PromotionPiece.Queen);
          BlackPawnPromoted(this, promotionArgs);
          switch (promotionArgs.PromotionPiece)
          {
            case PromotionPiece.Bishop:
              result.PromotionPiece = Piece.BlackBishop;
              break;

            case PromotionPiece.Knight:
              result.PromotionPiece = Piece.BlackKnight;
              break;

            case PromotionPiece.Rook:
              result.PromotionPiece = Piece.BlackRook;
              break;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Handles everything related to that the state of the chess game has changed. That includes raising of events
    /// end executing the engine to start thinking.
    /// </summary>
    /// <param name="move">Move that has changed the board. May be null if change was due to somthing else.</param>
    /// <param name="engineToPlay">Value endicating if engine are to play the next move (if possible).</param>
    /// <param name="raiseStatusEvent">Value endicating if status events should be raised.</param>
    private void HandleChangedGameState(Move move, bool engineToPlay, bool raiseStatusEvent)
    {
      m_boardCopy = new Board(m_currentGame.Board);

      if (BoardChanged != null)
        BoardChanged(this, new BoardChangedEventArgs(move));

      GameStatus status = m_currentGame.GetGameStatus();

      if (status == GameStatus.WhiteTimeVictory || status == GameStatus.BlackTimeVictory)
        m_currentGame.Clock.SignalTimeout = false;

      if (status == GameStatus.WhiteCheckmateVictory || status == GameStatus.BlackCheckmateVictory || status == GameStatus.StalemateDraw)
        m_currentGame.Clock.StopClock();

      if (raiseStatusEvent && StatusInfo != null)
        StatusInfo(this, new GameStatusEventArgs(status));

      if (status == GameStatus.Normal || status == GameStatus.WhiteIsCheck || status == GameStatus.BlackIsCheck)
      {
        if (engineToPlay)
          m_engineManager.Think(m_currentGame.Board, m_currentGame.Moves, m_engineConfiguration.UseBook, m_engineConfiguration.MaxSearchDepth, m_engineConfiguration.MaxSearchTime, CurrentRemainingTime());
      }
    }

    /// <summary>
    /// Returns the remaining time for the player that is to play.
    /// </summary>
    /// <returns></returns>
    private TimeSpan CurrentRemainingTime()
    {
      switch (m_currentGame.Board.State.ColorToPlay)
      {
        case PieceColor.White:
          return m_currentGame.Clock.WhiteTime;

        case PieceColor.Black:
          return m_currentGame.Clock.BlackTime;

        default:
          return new TimeSpan();
      }
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

      if (e.TimeOut)
        HandleChangedGameState(null, false, true);
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

      if (e.TimeOut)
        HandleChangedGameState(null, false, true);
    }
  }
}
