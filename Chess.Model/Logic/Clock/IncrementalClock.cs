using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  /// <summary>
  /// A incremental chess clock that counts down the time. A player starts out with some initial
  /// time and then gains some time for each performed move. I time runs out the player looses.
  /// </summary>
  class IncrementalClock : IClock
  {
    #region ClockState

    /// <summary>
    /// State used internally in IncrementalClock to represent a clock state.
    /// Used for undoing and redoing.
    /// </summary>
    struct ClockState
    {
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
      /// <param name="whiteTime">Time remaining for white.</param>
      /// <param name="blackTime">Time remaining for black.</param>
      /// <param name="signalTimeout">Should time out be signalled by the clock.</param>
      public ClockState(TimeSpan whiteTime, TimeSpan blackTime, bool signalTimeout)
      {
        WhiteTime = whiteTime;
        BlackTime = blackTime;
        SignalTimeout = signalTimeout;
      }
    }

    #endregion

    /// <summary>
    /// How much time should be added for each move.
    /// </summary>
    private TimeSpan m_addTime;

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
    /// Initializes a new incremental clock.
    /// </summary>
    /// <param name="startTime">Initial time for the clock.</param>
    /// <param name="addTime">time added for each move performed.</param>
    public IncrementalClock(TimeSpan startTime, TimeSpan addTime)
    {
      m_addTime = addTime;

      m_whiteTimer = new CountdownTimer(startTime);
      m_blackTimer = new CountdownTimer(startTime);
      m_whiteTimer.TimerNotify += WhiteClockEventHandler;
      m_blackTimer.TimerNotify += BlackClockEventHandler;

      m_currentState = new ClockState(startTime, startTime, true);

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

      m_whiteTimer.Time += m_addTime;
    }

    /// <summary>
    /// Called when black has performed a move.
    /// </summary>
    public void EndBlackTurn()
    {
      m_blackTimer.Stop();

      m_undoHistory.Push(m_currentState);
      m_redoHistory.Clear();

      m_blackTimer.Time += m_addTime;
    }

    /// <summary>
    /// Called when white is to perform a move.
    /// </summary>
    public void BeginWhiteTurn()
    {
      m_currentState = new ClockState(m_whiteTimer.Time, m_blackTimer.Time, m_currentState.SignalTimeout);

      m_whiteTimer.Start();
    }

    /// <summary>
    /// Called when black is to perform a move.
    /// </summary>
    public void BeginBlackTurn()
    {
      m_currentState = new ClockState(m_whiteTimer.Time, m_blackTimer.Time, m_currentState.SignalTimeout);

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
    /// Handles the countdown timer event for white when the timer is counting down. 
    /// </summary>
    /// <param name="sender">The countdown timer class</param>
    /// <param name="e">The event arguments.</param>
    private void WhiteClockEventHandler(object sender, CountdownTimerEventArgs e)
    {
      if (WhiteClockNotifier != null)
        WhiteClockNotifier(this, new ClockEventArgs(ClockType.Incremental, e.TimeLeft, m_currentState.SignalTimeout && e.TimeLeft.TotalMilliseconds <= 0.0));
    }

    /// <summary>
    /// Handles the countdown timer event for black when the timer is counting down. 
    /// </summary>
    /// <param name="sender">The countdown timer class</param>
    /// <param name="e">The event arguments.</param>
    private void BlackClockEventHandler(object sender, CountdownTimerEventArgs e)
    {
      if (BlackClockNotifier != null)
        BlackClockNotifier(this, new ClockEventArgs(ClockType.Incremental, e.TimeLeft, m_currentState.SignalTimeout && e.TimeLeft.TotalMilliseconds <= 0.0));
    }
  }
}
