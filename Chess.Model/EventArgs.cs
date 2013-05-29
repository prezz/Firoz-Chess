using System;
using Chess.Model.Logic;


namespace Chess.Model
{
  /// <summary>
  /// Class containing event data for the "BoardChanged" event raised from the "ChessInterface" class.
  /// </summary>
  public class BoardChangedEventArgs : EventArgs
  {
    /// <summary>
    /// If the change has happend due to a move this variable holds the square the move has been done from.
    /// </summary>
    private Square m_from;

    /// <summary>
    /// If the change has happend due to a move this variable holds the square the move has been done to.
    /// </summary>
    private Square m_to;

    /// <summary>
    /// Initializes a new instance of BoardChangedEventArgs.
    /// </summary>
    /// <param name="move">Move that has chenged the board. Can be null if the move has been changed in some other way.</param>
    internal BoardChangedEventArgs(Move move)
    {
      if (move == null)
      {
        m_from = Square.None;
        m_to = Square.None;
      }
      else
      {
        m_from = move.From;
        m_to = move.To;
      }
    }

    /// <summary>
    /// If the change has happend due to a move, returns square the move has been done from.
    /// </summary>
    public Square From
    {
      get { return m_from; }
    }

    /// <summary>
    /// If the change has happend due to a move, returns square the move has been done from.
    /// </summary>
    public Square To
    {
      get { return m_to; }
    }
  }


  /// <summary>
  /// Class containing event data for the "WhitePawnPromoted" or "BlackPawnPromoted" event raised
  /// from the "ChessInterface" class.
  /// </summary>
  public class PromotionEventArgs : EventArgs
  {
    /// <summary>
    /// Holds the piece a pawn will be promoted to.
    /// </summary>
    private PromotionPiece m_promotionPiece;

    /// <summary>
    /// Initializes a new instance of PromotionEventArgs.
    /// </summary>
    /// <param name="defaultPromotion">The default piece to promote to if another piece is not set in the "PromotionPiece" proberty.</param>
    internal PromotionEventArgs(PromotionPiece defaultPromotion)
    {
      m_promotionPiece = defaultPromotion;
    }

    /// <summary>
    /// Get or sets the piece pawn is promoted to.
    /// </summary>
    public PromotionPiece PromotionPiece
    {
      get { return m_promotionPiece; }
      set { m_promotionPiece = value; }
    }
  }


  /// <summary>
  /// Class containing event data for the "GameOver" event raised from the "ChessInterface" class.
  /// </summary>
  public class GameStatusEventArgs : EventArgs
  {
    /// <summary>
    /// Holds the game over status.
    /// </summary>
    private GameStatus m_status;

    /// <summary>
    /// Initializes a new instance of GameOverEventArgs.
    /// </summary>
    /// <param name="status">The game over status.</param>
    internal GameStatusEventArgs(GameStatus status)
    {
      m_status = status;
    }

    /// <summary>
    /// Gets the game over status.
    /// </summary>
    public GameStatus GameStatus
    {
      get { return m_status; }
    }
  }


  /// <summary>
  /// Class containing event data for the chess clock.
  /// </summary>
  public class ClockEventArgs : EventArgs
  {
    /// <summary>
    /// Type of clock that raised the event.
    /// </summary>
    private ClockType m_clockType;

    /// <summary>
    /// The time of the clock.
    /// </summary>
    private TimeSpan m_time;

    /// <summary>
    /// True if time has run out, false otherwise.
    /// </summary>
    private bool m_timeOut;

    /// <summary>
    /// Initializes a new ClockEventArgs,
    /// </summary>
    /// <param name="clockType">Type of clock raising the event.</param>
    /// <param name="time">Time of the clock.</param>
    /// <param name="timeOut">True if time has run out, false otherwise.</param>
    internal ClockEventArgs(ClockType clockType, TimeSpan time, bool timeOut)
    {
      m_clockType = clockType;
      m_time = time;
      m_timeOut = timeOut;
    }

    /// <summary>
    /// Gets the type of clock that raised the event.
    /// </summary>
    public ClockType ClockType
    {
      get { return m_clockType; }
    }

    /// <summary>
    /// Gets the time of the clock.
    /// </summary>
    public TimeSpan Time
    {
      get { return m_time; }
    }

    /// <summary>
    /// True if time has run out, false otherwise.
    /// </summary>
    public bool TimeOut
    {
      get { return m_timeOut; }
    }
  }
}
