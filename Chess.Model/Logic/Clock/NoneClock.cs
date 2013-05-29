using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Represents no chess clock. This clock counts up instead of down to keep track of the time
  /// used by each player.
  /// </summary>
  class NoneClock : IClock
  {
    #region ClockState

    /// <summary>
    /// State used internally in NoneClock to represent a clock state.
    /// Used for undoing and redoing.
    /// </summary>
    struct ClockState
    {
      /// <summary>
      /// Time used by white.
      /// </summary>
      public TimeSpan WhiteTime;

      /// <summary>
      /// Time used by black.
      /// </summary>
      public TimeSpan BlackTime;

      /// <summary>
      /// Initializes a new clock state
      /// </summary>
      /// <param name="whiteTime">Time used by white.</param>
      /// <param name="blackTime">Time used by black.</param>
      public ClockState(TimeSpan whiteTime, TimeSpan blackTime)
      {
        WhiteTime = whiteTime;
        BlackTime = blackTime;
      }
    }

    #endregion

    /// <summary>
    /// Timer responsible for counting up time for white.
    /// </summary>
    private StopwatchTimer m_whiteTimer;

    /// <summary>
    /// Timer responsible for counting up time for black.
    /// </summary>
    private StopwatchTimer m_blackTimer;

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
    /// Initializes a new none clock.
    /// </summary>
    public NoneClock()
    {
      m_whiteTimer = new StopwatchTimer();
      m_blackTimer = new StopwatchTimer();
      m_whiteTimer.TimerNotify += WhiteClockEventHandler;
      m_blackTimer.TimerNotify += BlackClockEventHandler;

      m_currentState = new ClockState(new TimeSpan(), new TimeSpan());

      m_undoHistory = new Stack<ClockState>();
      m_redoHistory = new Stack<ClockState>();
    }

    /// <summary>
    /// Returns the remaining time for white.
    /// </summary>
    public TimeSpan WhiteTime
    {
      get { return TimeSpan.MaxValue; }
    }

    /// <summary>
    /// Returns the remaining time for black.
    /// </summary>
    public TimeSpan BlackTime
    {
      get { return TimeSpan.MaxValue; }
    }

    /// <summary>
    /// Return true if white is out of time, otherwise return true.
    /// </summary>
    public bool WhiteTimeOut
    {
      get { return false; }
    }

    /// <summary>
    /// Return true if black is out of time, otherwise return true.
    /// </summary>
    public bool BlackTimeOut
    {
      get { return false; }
    }

    /// <summary>
    /// Always false as this clock never will signal a timeout.
    /// </summary>
    public bool SignalTimeout
    {
      get { return false; }
      set { }
    }

    /// <summary>
    /// Called when white has performed a move.
    /// </summary>
    public void EndWhiteTurn()
    {
      m_whiteTimer.Stop();

      m_undoHistory.Push(m_currentState);
      m_redoHistory.Clear();
    }

    /// <summary>
    /// Called when black has performed a move.
    /// </summary>
    public void EndBlackTurn()
    {
      m_blackTimer.Stop();

      m_undoHistory.Push(m_currentState);
      m_redoHistory.Clear();
    }

    /// <summary>
    /// Called when white is to perform a move.
    /// </summary>
    public void BeginWhiteTurn()
    {
      m_currentState = new ClockState(m_whiteTimer.Time, m_blackTimer.Time);

      m_whiteTimer.Start();
    }

    /// <summary>
    /// Called when black is to perform a move.
    /// </summary>
    public void BeginBlackTurn()
    {
      m_currentState = new ClockState(m_whiteTimer.Time, m_blackTimer.Time);

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
    /// Handles the countdown timer event for white when the timer is counting up. 
    /// </summary>
    /// <param name="sender">The countdown timer class</param>
    /// <param name="e">The event arguments.</param>
    private void WhiteClockEventHandler(object sender, StopwatchTimerEventArgs e)
    {
      if (WhiteClockNotifier != null)
        WhiteClockNotifier(this, new ClockEventArgs(ClockType.None, e.TimeSpent, false));
    }

    /// <summary>
    /// Handles the countdown timer event for black when the timer is counting up. 
    /// </summary>
    /// <param name="sender">The countdown timer class</param>
    /// <param name="e">The event arguments.</param>
    private void BlackClockEventHandler(object sender, StopwatchTimerEventArgs e)
    {
      if (BlackClockNotifier != null)
        BlackClockNotifier(this, new ClockEventArgs(ClockType.None, e.TimeSpent, false));
    }
  }
}
