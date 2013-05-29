using System;
using System.Threading;
using System.ComponentModel;
using Chess.Model.EngineInterface;


namespace Chess.Model.Logic
{
  #region MoveFoundEventArgs

  /// <summary>
  /// Class containing event data for the ending search event raised from the
  /// EngineThreadManager when a move has been found.
  /// </summary>
  internal class MoveFoundEventArgs : EventArgs
  {
    /// <summary>
    /// Variable holding the move that the engine has found and wants to play.
    /// </summary>
    private Move m_move;

    /// <summary>
    /// Initializes a new MoveFoundEventArgs.
    /// </summary>
    /// <param name="move">The move found by the engine it wants to play.</param>
    internal MoveFoundEventArgs(Move move)
    {
      m_move = move;
    }

    /// <summary>
    /// Move engine has found and wants to play.
    /// </summary>
    internal Move Move
    {
      get { return m_move; }
    }
  }

  #endregion


  /// <summary>
  /// Class encapsulating the search thread for the computer player.
  /// </summary>
  class EngineManager
  {
    #region ThreadParam

    /// <summary>
    /// Object representing the parameters to be passed to the thread when launched.
    /// </summary>
    class ThreadParam
    {
      /// <summary>
      /// The board to find a move on.
      /// </summary>
      private Board m_board;

      /// <summary>
      /// Collection of moves for board to evaluate and carry on search for.
      /// </summary>
      private MoveOrganizer m_moveOrganizer;

      /// <summary>
      /// May the opening book be used to find a move.
      /// </summary>
      private bool m_useBook;

      /// <summary>
      /// Max depth to search.
      /// </summary>
      private int m_maxSearchDepth;

      /// <summary>
      /// Approximated time to search. Search time will typically be a little longer.
      /// </summary>
      private TimeSpan m_aimSearchTime;

      /// <summary>
      /// Maximum time to search. Search time will typically be a little longer.
      /// </summary>
      private TimeSpan m_maxSearchTime;


      /// <summary>
      /// Instantiates a new instance of the "ThreadParam" class.
      /// </summary>
      /// <param name="board">Board associated with move organizer.</param>
      /// <param name="moveOrganizer">Collection of moves to evaluate and carry out search for.</param>
      /// <param name="useBook">May the engine use the opening book when selecting a move.</param>
      /// <param name="searchDepth">Depth to search.</param>
      /// <param name="aimSearchTime">Time to search. Actual search will typically be a little more.</param>
      /// <param name="maxSearchTime">Maximum time to search.</param>
      public ThreadParam(Board board, MoveOrganizer moveOrganizer, bool useBook, int searchDepth, TimeSpan aimSearchTime, TimeSpan maxSearchTime)
      {
        m_board = board;
        m_moveOrganizer = moveOrganizer;
        m_useBook = useBook;
        m_maxSearchDepth = searchDepth;
        m_aimSearchTime = aimSearchTime;
        m_maxSearchTime = maxSearchTime;
      }

      /// <summary>
      /// Gets the board to find a move for.
      /// </summary>
      public Board Board
      {
        get { return m_board; }
      }

      /// <summary>
      /// Gets the collection of moves to evaluate and carry out search for. 
      /// </summary>
      public MoveOrganizer MoveOrganizer
      {
        get { return m_moveOrganizer; }
      }

      /// <summary>
      /// Gets if the opening book be used to find a move.
      /// </summary>
      public bool UseBook
      {
        get { return m_useBook; }
      }

      /// <summary>
      /// Gets depth to search.
      /// </summary>
      public int searchDepth
      {
        get { return m_maxSearchDepth; }
      }

      /// <summary>
      /// Search time to aim for.
      /// </summary>
      public TimeSpan AimSearchTime
      {
        get { return m_aimSearchTime; }
      }

      /// <summary>
      /// Maximum search time..
      /// </summary>
      public TimeSpan MaxSearchTime
      {
        get { return m_maxSearchTime; }
      }
    }

    #endregion


    /// <summary>
    /// Collection of moves to play immediately if possible. Typically done in an
    /// early stages of a game.
    /// </summary>
    private IOpeningBook m_openingBook;

    /// <summary>
    /// Search tree responsible for doing the search.
    /// </summary>
    private ISearchTree m_searchTree;

    /// <summary>
    /// Responsible for calculating the allowed search time based on current clock time.
    /// </summary>
    private ITimeControl m_timeControl;

    /// <summary>
    /// Is the engine currently thinking.
    /// </summary>
    private bool m_thinking;

    /// <summary>
    /// wait handle ensuring worker thread is terminated in a safe manner.
    /// </summary>
    private ManualResetEvent m_waitHandle;

    /// <summary>
    /// The actual thinking is done in this background worker thread.
    /// </summary>
    private BackgroundWorker m_workerThread;

    /// <summary>
    /// Event raised when a move has been found.
    /// </summary>
    public event EventHandler<MoveFoundEventArgs> MoveFound;


    /// <summary>
    /// Instantiates a new instance of the EngineThreadManager.
    /// </summary>
    /// <param name="openingBook">Opening book to use. Can be null if no opening book is to be used.</param>
    /// <param name="searchEvaluator">Search tree with evaluator to use for moves not pressent in the opening book.</param>
    /// <param name="timeControl">Time controller responsible for calculating search times.</param>
    public EngineManager(IOpeningBook openingBook, ISearchTree searchEvaluator, ITimeControl timeControl)
    {
      m_openingBook = openingBook;
      m_searchTree = searchEvaluator;
      m_timeControl = timeControl;

      m_waitHandle = new ManualResetEvent(true);
      m_workerThread = new BackgroundWorker();
      m_workerThread.DoWork += FindMove;
      m_workerThread.RunWorkerCompleted += FindMoveCompleted;
    }

    /// <summary>
    /// Gats if the engine is thinking. 
    /// </summary>
    public bool Thinking
    {
      get { return m_thinking; }
    }

    /// <summary>
    /// Makes computer think. When the move to play as been decided the
    /// "MoveFound" event is raised.
    /// </summary>
    /// <param name="board">The board to find a move for.</param>
    /// <param name="moveOrganizer">List of possible moves for the current position on board.</param>
    /// <param name="useBook">May the engine use the opening book when selecting a move.</param>
    /// <param name="maxSearchDepth">Maximum AlphaBeta depth to search. Must be equal to, or larger then one.</param>
    /// <param name="maxSearchTime">Time before search is stoppen.</param>
    /// <param name="clockTime">The remaining time of the clock.</param>
    public void Think(Board board, MoveOrganizer moveOrganizer, bool useBook, int maxSearchDepth, TimeSpan maxSearchTime, TimeSpan clockTime)
    {
      if (!m_thinking)
      {
        m_waitHandle.Reset();
        m_thinking = true;

        TimeSpan calculatedAimTime = m_timeControl.CalculateAimedSearchTime(clockTime);
        TimeSpan calculatedMaxTime = m_timeControl.CalculateMaxSearchTime(clockTime);

        if (calculatedMaxTime > maxSearchTime)
          calculatedMaxTime = maxSearchTime;

        if (calculatedAimTime > calculatedMaxTime)
          calculatedAimTime = calculatedMaxTime;

        m_workerThread.RunWorkerAsync(new ThreadParam(board, moveOrganizer, useBook, maxSearchDepth, calculatedAimTime, calculatedMaxTime));
      }
    }

    /// <summary>
    /// Call this to abort the current search without raising the move found event.
    /// </summary>
    public void Abort()
    {
      if (m_thinking)
      {
        m_searchTree.SignalStopSearch();
        m_waitHandle.WaitOne();
      }
    }

    /// <summary>
    /// The function launched in a seperate thread that will carry out the actual search/thinking.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    private void FindMove(object sender, DoWorkEventArgs e)
    {
      ThreadParam param = (ThreadParam)e.Argument;
      Move move = null;

      if (param.UseBook)
        move = m_openingBook.Query(param.Board, param.MoveOrganizer);

      if (move == null)
        move = m_searchTree.FindBestMove(param.Board, param.MoveOrganizer, param.AimSearchTime, param.MaxSearchTime, param.searchDepth);

      e.Result = move;
      m_waitHandle.Set();
    }

    /// <summary>
    /// Event handler called in the main threads context by the m_workerThread when the engine thead finishes.
    /// This class will then call the public event on this class signaling a move has been found.
    /// </summary>
    /// <param name="sender">The background worker class that has done the search.</param>
    /// <param name="e">Event args from the background worker.</param>
    private void FindMoveCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Result != null && MoveFound != null)
        MoveFound(this, new MoveFoundEventArgs((Move)e.Result));

      m_thinking = false;
    }
  }
}
