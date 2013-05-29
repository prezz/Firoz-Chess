using System;
using System.ComponentModel;
using System.Threading;


namespace Chess.Model.Logic
{
  #region StopwatchTimerEventArgs

  /// <summary>
  /// 
  /// </summary>
  public class StopwatchTimerEventArgs : EventArgs
  {
    private TimeSpan m_timeSpent;

    internal StopwatchTimerEventArgs(TimeSpan timeSpent)
    {
      m_timeSpent = timeSpent;
    }

    /// <summary>
    /// 
    /// </summary>
    public TimeSpan TimeSpent
    {
      get { return m_timeSpent; }
    }
  }

  #endregion


  class StopwatchTimer
  {
    private volatile bool m_running;
    private TimeSpan m_currentTime;
    private ManualResetEvent m_stoppingWaitHandle;
    private ManualResetEvent m_notifierWaitHandle;
    private BackgroundWorker m_timerNotifier;
    private DateTime m_startTime;


    public event EventHandler<StopwatchTimerEventArgs> TimerNotify;


    public StopwatchTimer()
    {
      m_running = false;
      m_currentTime = new TimeSpan();
      m_stoppingWaitHandle = new ManualResetEvent(true);
      m_notifierWaitHandle = new ManualResetEvent(true);
      m_timerNotifier = new BackgroundWorker();
      m_timerNotifier.WorkerReportsProgress = true;
      m_timerNotifier.DoWork += NotifierThread;
      m_timerNotifier.ProgressChanged += TimeLeftNotifier;
    }

    public TimeSpan Time
    {
      get
      {
        if (m_running)
          return m_currentTime + (DateTime.Now - m_startTime);
        else
          return m_currentTime;
      }

      set
      {
        m_currentTime = value;

        if (TimerNotify != null)
          TimerNotify(this, new StopwatchTimerEventArgs(m_currentTime));
      }
    }

    public void Start()
    {
      if (!m_running)
      {
        while (m_timerNotifier.IsBusy)
          Thread.Sleep(1);

        m_running = true;
        m_notifierWaitHandle.Reset();
        m_stoppingWaitHandle.Reset();
        m_startTime = DateTime.Now;
        m_timerNotifier.RunWorkerAsync();
      }
    }

    public void Stop()
    {
      if (m_running)
      {
        DateTime stopTime = DateTime.Now;
        m_running = false;
        m_notifierWaitHandle.Set();
        m_stoppingWaitHandle.WaitOne();
        m_currentTime += (stopTime - m_startTime);
      }
    }

    public void RaiseTimeNotifyEvent()
    {
      if (TimerNotify != null)
        TimerNotify(this, new StopwatchTimerEventArgs(m_currentTime));
    }

    private void NotifierThread(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = (BackgroundWorker)sender;

      while (m_running)
      {
        TimeSpan currentTime = m_currentTime + (DateTime.Now - m_startTime);
        worker.ReportProgress(0, currentTime);
        m_notifierWaitHandle.WaitOne((currentTime.Milliseconds != 0) ? 1000 - currentTime.Milliseconds : 1000, false);
      }

      m_stoppingWaitHandle.Set();
    }

    void TimeLeftNotifier(object sender, ProgressChangedEventArgs e)
    {
      TimeSpan timeSpan = (TimeSpan)e.UserState;

      if (TimerNotify != null)
        TimerNotify(this, new StopwatchTimerEventArgs(timeSpan));
    }
  }
}
