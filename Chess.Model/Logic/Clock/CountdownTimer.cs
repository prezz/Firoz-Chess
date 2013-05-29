using System;
using System.ComponentModel;
using System.Threading;


namespace Chess.Model.Logic
{
  #region CountdownTimerEventArgs

  /// <summary>
  /// 
  /// </summary>
  public class CountdownTimerEventArgs : EventArgs
  {
    private TimeSpan m_time;

    internal CountdownTimerEventArgs(TimeSpan timeLeft)
    {
      m_time = timeLeft;
    }

    /// <summary>
    /// 
    /// </summary>
    public TimeSpan TimeLeft
    {
      get { return m_time; }
    }
  }

  #endregion


  class CountdownTimer
  {
    private volatile bool m_running;
    private TimeSpan m_remainingTime;
    private ManualResetEvent m_stoppingWaitHandle;
    private ManualResetEvent m_notifierWaitHandle;
    private BackgroundWorker m_timerNotifier;
    private DateTime m_startTime;


    public event EventHandler<CountdownTimerEventArgs> TimerNotify;


    public CountdownTimer(TimeSpan time)
    {
      m_running = false;
      m_remainingTime = time;
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
          return m_remainingTime - (DateTime.Now - m_startTime);
        else
          return m_remainingTime;
      }

      set
      {
        m_remainingTime = value;

        if (TimerNotify != null)
          TimerNotify(this, new CountdownTimerEventArgs(m_remainingTime));
      }
    }

    public void Start()
    {
      if (!m_running)
      {
        //Since m_stoppingWaitHandle.Set() is called last in other thread its actually 
        //possible to get here if calling Start() extremly short after a call to Stop()
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
        m_remainingTime -= (stopTime - m_startTime);
      }
    }

    public void RaiseTimeNotifyEvent()
    {
      if (TimerNotify != null)
        TimerNotify(this, new CountdownTimerEventArgs(m_remainingTime));
    }

    private void NotifierThread(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = (BackgroundWorker)sender;

      while (m_running)
      {
        TimeSpan remainingTime = m_remainingTime - (DateTime.Now - m_startTime);
        worker.ReportProgress(0, remainingTime);
        m_notifierWaitHandle.WaitOne((remainingTime.Milliseconds > 0) ? remainingTime.Milliseconds : 1000 + remainingTime.Milliseconds, false);
      }

      m_stoppingWaitHandle.Set();
    }

    void TimeLeftNotifier(object sender, ProgressChangedEventArgs e)
    {
      TimeSpan timeSpan = (TimeSpan)e.UserState;

      if (TimerNotify != null)
        TimerNotify(this, new CountdownTimerEventArgs(timeSpan));
    }
  }
}
