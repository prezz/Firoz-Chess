using System;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Interface a chess clocks must implement.
  /// </summary>
  interface IClock
  {
    /// <summary>
    /// Event that must be raised to notify about whites clock. It is advisable to raise this
    /// event once every second when white is to move.
    /// </summary>
    event EventHandler<ClockEventArgs> WhiteClockNotifier;

    /// <summary>
    /// Event that must be raised to notify about blacks clock. It is advisable to raise this
    /// event once every second when black is to move.
    /// </summary>
    event EventHandler<ClockEventArgs> BlackClockNotifier;

    /// <summary>
    /// Returns the remaining time for white.
    /// </summary>
    TimeSpan WhiteTime { get;}

    /// <summary>
    /// Returns the remaining time for black.
    /// </summary>
    TimeSpan BlackTime { get;}

    /// <summary>
    /// Return true if white is out of time, otherwise return true.
    /// </summary>
    bool WhiteTimeOut { get; }

    /// <summary>
    /// Return true if black is out of time, otherwise return true.
    /// </summary>
    bool BlackTimeOut { get; }

    /// <summary>
    /// Set or gets whether timeout will be signalled. If set to
    /// false timeout will not be signalled even thoug remaining time is negative.
    /// </summary>
    bool SignalTimeout { get; set; }

    /// <summary>
    /// Called when white has performed a move.
    /// </summary>
    void EndWhiteTurn();

    /// <summary>
    /// Called when black has performed a move.
    /// </summary>
    void EndBlackTurn();

    /// <summary>
    /// Called when white is to perform a move.
    /// </summary>
    void BeginWhiteTurn();

    /// <summary>
    /// Called when black is to perform a move.
    /// </summary>
    void BeginBlackTurn();

    /// <summary>
    /// Called when undoing a move. The clock must be responsible for keeping track
    /// of its undo and redo history.
    /// </summary>
    void Undo();

    /// <summary>
    /// Called when redoing a move. The clock must be responsible for keeping track
    /// of its undo and redo history.
    /// </summary>
    void Redo();

    /// <summary>
    /// Stops the clock.
    /// </summary>
    void StopClock();

    /// <summary>
    /// Forces the WhiteClockNotifier and BlackClockNotifier events to be raised.
    /// </summary>
    void RaiseTimeNotifyEvent();
  }
}
