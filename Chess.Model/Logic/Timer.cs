using System;
using System.ComponentModel;
using System.Threading;


namespace Chess.Model.Logic
{
	#region TimerEventArgs

	public class TimerEventArgs : EventArgs
	{
		private TimeSpan m_time;

		internal TimerEventArgs(TimeSpan timeLeft)
		{
			m_time = timeLeft;
		}

		public TimeSpan TimeLeft
		{
			get { return m_time; }
		}
	}

	#endregion


	class Timer
	{
		private volatile bool m_running;
		private TimeSpan m_remainingTime;
		private ManualResetEvent m_stoppingWaitHandle;
		private ManualResetEvent m_notifierWaitHandle;
		private BackgroundWorker m_timerNotifier;
		private DateTime m_startTime;


		public event EventHandler<TimerEventArgs> TimerNotify;


		public Timer(TimeSpan time)
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
			get { return m_remainingTime; }
			set
			{
				m_remainingTime = value;

				if (TimerNotify != null)
					TimerNotify(this, new TimerEventArgs(m_remainingTime));
			}
		}

		public void Start()
		{
			if (!m_running)
			{
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
				TimerNotify(this, new TimerEventArgs(m_remainingTime));
		}

		private void NotifierThread(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender;

			while (m_running)
			{
				TimeSpan remainingTime = m_remainingTime - (DateTime.Now - m_startTime);
				worker.ReportProgress(0, remainingTime);
				m_notifierWaitHandle.WaitOne((remainingTime.Milliseconds > 0)? remainingTime.Milliseconds : 1000, false);
			}

			m_stoppingWaitHandle.Set();
		}

		void TimeLeftNotifier(object sender, ProgressChangedEventArgs e)
		{
			TimeSpan timeSpan = (TimeSpan)e.UserState;

			if (timeSpan.TotalMilliseconds <= 0.0)
			{
				Stop();
				timeSpan = new TimeSpan();
			}

			if (TimerNotify != null)
				TimerNotify(this, new TimerEventArgs(timeSpan));
		}
	}
}
