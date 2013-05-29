using System;
using System.Text;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  /// <summary>
  /// A regular chess clock that counts down the time. A player then has a certain amount of time to make a
  /// specified amount of moves.
  /// </summary>
  class ConventionalClock : IClock
  {
    #region ClockState

    /// <summary>
    /// State used internally in ConventionalClock to represent a clock state.
    /// Used for undoing and redoing.
    /// </summary>
    struct ClockState
    {
      /// <summary>
      /// Moves played by white.
      /// </summary>
      public int WhiteMoves;

      /// <summary>
      /// Moves played by black.
      /// </summary>
      public int BlackMoves;

      /// <summary>
      /// Time remaining for white.
      /// </summary>
      public TimeSpan WhiteTime;

      /// <summary>
      /// Time remaining for black.
      /// </summary>
      public TimeSpan BlackTime;

      /// <summary>
      /// Should time out be signalled by the clock.
      /// </summary>
      public bool SignalTimeout;

      /// <summary>
      /// Initializes a new clock state
      /// </summary>
      /// <param name="whiteMoves">Moves played by white.</param>
      /// <param name="blackMoves">Moves played by black.</param>
      /// <param name="whiteTime">Time remaining for white.</param>
      /// <param name="blackTime">Time remaining for black.</param>
      /// <param name="signalTimeout">Should time out be signalled by the clock.</param>
      public ClockState(int whiteMoves, int blackMoves, TimeSpan whiteTime, TimeSpan blackTime, bool signalTimeout)
      {
        WhiteMoves = whiteMoves;
        BlackMoves = blackMoves;
        WhiteTime = whiteTime;
        BlackTime = blackTime;
        SignalTimeout = signalTimeout;
      }
    }

    #endregion

    /// <summary>
    /// Total amount of moves that must be played before time runs out.
    /// </summary>
    private int m_moves;

    /// <summary>
    /// The total time the player has to make the given amount of moves.
    /// </summary>
    private TimeSpan m_time;

    /// <summary>
    /// White moves remaining of the moves that must be played.
    /// </summary>
    private int m_whiteMoves;

    /// <summary>
    /// Black moves remaining of the moves that must be played.
    /// </summary>
    private int m_blackMoves;

    /// <summary>
    /// Timer responsible for counting down time for white.
    /// </summary>
    private CountdownTimer m_whiteTimer;

    /// <summary>
    /// Timer responsible for counting down time for black.
    /// </summary>
    private CountdownTimer m_blackTimer;

    /// <summary>
    /// Current state of the clock.
    /// </summary>
    private ClockState m_currentState;

    /// <summary>
    /// History of previous states.
    /// </summary>
    private Stack<ClockState> m_undoHistory;

    /// <summary>
    /// History of subsequent states if undoes has been made.
    /// </summary>
    private Stack<ClockState> m_redoHistory;


    /// <summary>
    /// Event that must be raised to notify about whites clock. It is advisable to raise this
    /// event once every second when white is to move.
    /// </summary>
    public event EventHandler<ClockEventArgs> WhiteClockNotifier;

    /// <summary>
    /// Event that must be raised to notify about blacks clock. It is advisable to raise this
    /// event once every second when black is to move.
    /// </summary>
    public event EventHandler<ClockEventArgs> BlackClockNotifier;


    /// <summary>
    /// Initializes a new conventional clock.
    /// </summary>
    /// <param name="moves">Moves that must be taken within the given time.</param>
    /// <param name="time">Time available to make the given amount of moves.</param>
    public ConventionalClock(int moves, TimeSpan time)
    {
      m_moves = moves;
      m_time = time;

      m_whiteMoves = moves;
      m_blackMoves = moves;

      m_whiteTimer = new CountdownTimer(time);
      m_blackTimer = new CountdownTimer(time);
      m_whiteTimer.TimerNotify += WhiteClockEventHandler;
      m_blackTimer.TimerNotify += BlackClockEventHandler;

      m_currentState = new ClockState(moves, moves, time, time, true);

      m_undoHistory = new Stack<ClockState>();
      m_redoHistory = new Stack<ClockState>();
    }

    /// <summary>
    /// Returns the remaining time for white.
    /// </summary>
    public TimeSpan WhiteTime
    {
      get { return m_whiteTimer.Time; }
    }

    /// <summary>
    /// Returns the remaining time for black.
    /// </summary>
    public TimeSpan BlackTime
    {
      get { return m_blackTimer.Time; }
    }

    /// <summary>
    /// Return true if white is out of time, otherwise return true.
    /// </summary>
    public bool WhiteTimeOut
    {
      get { return m_currentState.SignalTimeout && m_whiteTimer.Time.TotalMilliseconds <= 0.0; }
    }

    /// <summary>
    /// Return true if black is out of time, otherwise return true.
    /// </summary>
    public bool BlackTimeOut
    {
      get { return m_currentState.SignalTimeout && m_blackTimer.Time.TotalMilliseconds <= 0.0; }
    }

    /// <summary>
    /// Get or sets whether the clock currently should signal timeout.
    /// Undoing and redoung may change this value to prevous states.
    /// </summary>
    public bool SignalTimeout
    {
      get { return m_currentState.SignalTimeout; }
      set { m_currentState.SignalTimeout = value; }
    }

    /// <summary>
    /// Called when white has performed a move.
    /// </summary>
    public void EndWhiteTurn()
    {
      m_whiteTimer.Stop();

      m_undoHistory.Push(m_currentState);
      m_redoHistory.Clear();

      if (--m_whiteMoves == 0)
      {
        m_whiteTimer.Time += m_time;
        m_whiteMoves = m_moves;
      }
    }

    /// <summary>
    /// Called when black has performed a move.
    /// </summary>
    public void EndBlackTurn()
    {
      m_blackTimer.Stop();

      m_undoHistory.Push(m_currentState);
      m_redoHistory.Clear();

      if (--m_blackMoves == 0)
      {
        m_blackTimer.Time += m_time;
        m_blackMoves = m_moves;
      }
    }

    /// <summary>
    /// Called when white is to perform a move.
    /// </summary>
    public void BeginWhiteTurn()
    {
      m_currentState = new ClockState(m_whiteMoves, m_blackMoves, m_whiteTimer.Time, m_blackTimer.Time, m_currentState.SignalTimeout);

      m_whiteTimer.Start();
    }

    /// <summary>
    /// Called when black is to perform a move.
    /// </summary>
    public void BeginBlackTurn()
    {
      m_currentState = new ClockState(m_whiteMoves, m_blackMoves, m_whiteTimer.Time, m_blackTimer.Time, m_currentState.SignalTimeout);

      m_blackTimer.Start();
    }

    /// <summary>
    /// Called when undoing a move. The clock must be responsible for keeping track
    /// of its undo and redo history.
    /// </summary>
    public void Undo()
    {
      StopClock();

      m_redoHistory.Push(m_currentState);
      m_currentState = m_undoHistory.Pop();
      m_whiteMoves = m_currentState.WhiteMoves;
      m_blackMoves = m_currentState.BlackMoves;
      m_whiteTimer.Time = m_currentState.WhiteTime;
      m_blackTimer.Time = m_currentState.BlackTime;
    }

    /// <summary>
    /// Called when redoing a move. The clock must be responsible for keeping track
    /// of its undo and redo history.
    /// </summary>
    public void Redo()
    {
      StopClock();

      m_undoHistory.Push(m_currentState);
      m_currentState = m_redoHistory.Pop();
      m_whiteMoves = m_currentState.WhiteMoves;
      m_blackMoves = m_currentState.BlackMoves;
      m_whiteTimer.Time = m_currentState.WhiteTime;
      m_blackTimer.Time = m_currentState.BlackTime;
    }

    /// <summary>
    /// Stops the clock.
    /// </summary>
    public void StopClock()
    {
      m_whiteTimer.Stop();
      m_blackTimer.Stop();
    }

    /// <summary>
    /// Forces the WhiteClockNotifier and BlackClockNotifier events to be raised.
    /// </summary>
    public void RaiseTimeNotifyEvent()
    {
      m_whiteTimer.RaiseTimeNotifyEvent();
      m_blackTimer.RaiseTimeNotifyEvent();
    }

    /// <summary>
    /// Handles the countdown timer event for white when the timer is counting down. 
    /// </summary>
    /// <param name="sender">The countdown timer class</param>
    /// <param name="e">The event arguments.</param>
    private void WhiteClockEventHandler(object sender, CountdownTimerEventArgs e)
    {
      if (WhiteClockNotifier != null)
        WhiteClockNotifier(this, new ClockEventArgs(ClockType.Conventional, e.TimeLeft, m_currentState.SignalTimeout && e.TimeLeft.TotalMilliseconds <= 0.0));
    }

    /// <summary>
    /// Handles the countdown timer event for black when the timer is counting down. 
    /// </summary>
    /// <param name="sender">The countdown timer class</param>
    /// <param name="e">The event arguments.</param>
    private void BlackClockEventHandler(object sender, CountdownTimerEventArgs e)
    {
      if (BlackClockNotifier != null)
      {
        BlackClockNotifier(this, new ClockEventArgs(ClockType.Conventional, e.TimeLeft, m_currentState.SignalTimeout && e.TimeLeft.TotalMilliseconds <= 0.0));
      }
    }
  }
}
