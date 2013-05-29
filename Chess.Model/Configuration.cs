using System;


namespace Chess.Model
{
  /// <summary>
  /// Configuration of the chess engine.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
  public struct EngineConfiguration
  {
    /// <summary>
    /// Endicates if the engine automaticly playes the the next move after a human (or other extern
    /// facade user) has played a move.
    /// </summary>
    private bool m_engineAutoPlay;

    /// <summary>
    /// May the engine use the opening book to find a move to play.
    /// </summary>
    private bool m_useBook;

    /// <summary>
    /// The time in seconds before engines stops searching for the best move.
    /// The actual search time will always be somewhat higher then this value.
    /// </summary>
    private TimeSpan m_maxSearchTime;

    /// <summary>
    /// Maximum full width depth the engine will search.
    /// </summary>
    private int m_maxSearchDepth;


    /// <summary>
    /// Instantiates a new EngineConfiguration.
    /// </summary>
    /// <param name="engineAutoPlay">Should engine automaticly play after a human move.</param>
    /// <param name="useBook">May the engine use the opening book to find a move to play.</param>
    /// <param name="maxTime">Time before search is stopped.</param>
    /// <param name="maxDepth">Depth off full width search.</param>
    public EngineConfiguration(bool engineAutoPlay, bool useBook, TimeSpan maxTime, int maxDepth)
    {
      m_engineAutoPlay = engineAutoPlay;
      m_useBook = useBook;
      m_maxSearchTime = maxTime;
      m_maxSearchDepth = maxDepth;
    }

    /// <summary>
    /// Should engine automaticly play after a human move.
    /// </summary>
    public bool EngineAutoPlay
    {
      get { return m_engineAutoPlay; }
      set { m_engineAutoPlay = value; }
    }

    /// <summary>
    /// May the engine use the opening book to find a move to play.
    /// </summary>
    public bool UseBook
    {
      get { return m_useBook; }
      set { m_useBook = value; }
    }

    /// <summary>
    /// Time before search is stopped.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.ChessModelException.#ctor(System.String)")]
    public TimeSpan MaxSearchTime
    {
      get { return m_maxSearchTime; }

      set
      {
        if (value.TotalMilliseconds < 0.0)
          throw new ChessModelException("Search time can not be negative.");

        m_maxSearchTime = value;
      }
    }

    /// <summary>
    /// Depth off full width search.
    /// </summary>
    /// <exception cref="Chess.Model.ChessModelException"></exception>"
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.ChessModelException.#ctor(System.String)")]
    public int MaxSearchDepth
    {
      get { return m_maxSearchDepth; }

      set
      {
        if (value < 0)
          throw new ChessModelException("Search depth can not be negative.");

        m_maxSearchDepth = value;
      }
    }
  }


  /// <summary>
  /// Configuration of the chess clock.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
  public struct ClockConfiguration
  {
    /// <summary>
    /// Type of clock to use.
    /// </summary>
    private ClockType m_clockType;

    /// <summary>
    /// Number of moves that must be taken before time runs out with the conventional clock.
    /// </summary>
    private int m_conventionalMoves;

    /// <summary>
    /// Amount of time a player has to take the specified amount of moves with the conventional clock.
    /// </summary>
    private TimeSpan m_conventionalTime;

    /// <summary>
    /// Start time if using the incremental clock.
    /// </summary>
    private TimeSpan m_incrementStartTime;

    /// <summary>
    /// Time added for avery move taken with the incremental clock.
    /// </summary>
    private TimeSpan m_incrementPlusTime;


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="clockType">Type of clock to use.</param>
    /// <param name="conventionalMoves">Number of moves that must be taken before time runs out with the conventional clock.</param>
    /// <param name="conventionalTime">Amount of time a player has to take the specified amount of moves with the conventional clock.</param>
    /// <param name="incrementStartTime">Start time if using the incremental clock.</param>
    /// <param name="incrementPlusTime">Time added for avery move taken with the incremental clock.</param>
    public ClockConfiguration(ClockType clockType, int conventionalMoves, TimeSpan conventionalTime, TimeSpan incrementStartTime, TimeSpan incrementPlusTime)
    {
      m_clockType = clockType;

      m_conventionalMoves = conventionalMoves;
      m_conventionalTime = conventionalTime;

      m_incrementStartTime = incrementStartTime;
      m_incrementPlusTime = incrementPlusTime;
    }

    /// <summary>
    /// Get or sets the configured clock type.
    /// </summary>
    public ClockType ClockType
    {
      get { return m_clockType; }
      set { m_clockType = value; }
    }

    /// <summary>
    /// Get or sets the number of moves that must be taken before time runs out with the conventional clock.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.ChessModelException.#ctor(System.String)")]
    public int ConventionalMoves
    {
      get { return m_conventionalMoves; }

      set
      {
        if (value < 0)
          throw new ChessModelException("Moves can not be negative.");

        m_conventionalMoves = value;
      }
    }

    /// <summary>
    /// Get or sets the amount of time a player has to take the specified amount of moves with the conventional clock.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.ChessModelException.#ctor(System.String)")]
    public TimeSpan ConventionalTime
    {
      get { return m_conventionalTime; }

      set
      {
        if (value.TotalMilliseconds < 0)
          throw new ChessModelException("Time cannot be negative");

        m_conventionalTime = value;
      }
    }

    /// <summary>
    /// Get or sets start time if using the incremental clock.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.ChessModelException.#ctor(System.String)")]
    public TimeSpan IncrementStartTime
    {
      get { return m_incrementStartTime; }

      set
      {
        if (value.TotalMilliseconds < 0)
          throw new ChessModelException("Time cannot be negative");

        m_incrementStartTime = value;
      }
    }

    /// <summary>
    /// Get or sets the time added for avery move taken with the incremental clock.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Chess.Model.ChessModelException.#ctor(System.String)")]
    public TimeSpan IncrementPlusTime
    {
      get { return m_incrementPlusTime; }

      set
      {
        if (value.TotalMilliseconds < 0)
          throw new ChessModelException("Time cannot be negative");

        m_incrementPlusTime = value;
      }
    }
  }
}
