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
    class EngineThreadManager
    {
        #region ThreadParam

        /// <summary>
        /// Object representing the parameters to be passed to the thread when launched.
        /// </summary>
        class ThreadParam
        {
            /// <summary>
            /// Collection of moves to evaluate and carry on search for.
            /// </summary>
            private MoveOrganizer m_moveOrganizer;

            /// <summary>
            /// Max depth to search.
            /// </summary>
            private int m_maxSearchDepth;

            /// <summary>
            /// Approximated time to search. Search time will typically be a little longer.
            /// </summary>
            private int m_searchTime;


            /// <summary>
            /// Instantiates a new instance of the "ThreadParam" class.
            /// </summary>
            /// <param name="moveOrganizer">Collection of moves to evaluate and carry out search for.</param>
            /// <param name="searchDepth">Depth to search.</param>
            /// <param name="searchTime">Time to search. Actual search will typically be a little more.</param>
            public ThreadParam(MoveOrganizer moveOrganizer, int searchDepth, int searchTime)
            {
                m_moveOrganizer = moveOrganizer;
                m_maxSearchDepth = searchDepth;
                m_searchTime = searchTime;
            }

            /// <summary>
            /// Gets the collection of moves to evaluate and carry out search for. 
            /// </summary>
            public MoveOrganizer MoveOrganizer
            {
                get { return m_moveOrganizer; }
            }

            /// <summary>
            /// Gets depth to search.
            /// </summary>
            public int searchDepth
            {
                get { return m_maxSearchDepth; }
            }

            public int searchTime
            {
                get { return m_searchTime; }
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
		/// wait handle ensuring worker thread is terminated in a safe manner.
		/// </summary>
		private ManualResetEvent m_waitHandle;

        /// <summary>
        /// The actual thinking is done in this background worker thread.
        /// </summary>
        private BackgroundWorker m_workerThread;

		/// <summary>
		/// Value indicating if the current search is being aborted.
		/// </summary>
		private bool m_abortingSearch;


        /// <summary>
        /// Event raised when a move has been found.
        /// </summary>
        public event EventHandler<MoveFoundEventArgs> MoveFound;


        /// <summary>
        /// Instantiates a new instance of the EngineThreadManager.
        /// </summary>
        /// <param name="openingBook">Opening book to use. Can be null if no opening book is to be used.</param>
        /// <param name="searchEvaluator">Search tree with evaluator to use for moves not pressent in the opening book.</param>
        public EngineThreadManager(IOpeningBook openingBook, ISearchTree searchEvaluator)
        {
            m_openingBook = openingBook;
            m_searchTree = searchEvaluator;

			m_waitHandle = new ManualResetEvent(true);
            m_workerThread = new BackgroundWorker();
            m_workerThread.DoWork += FindMove;
            m_workerThread.RunWorkerCompleted += FindMoveCompleted;
        }

        /// <summary>
        /// Makes computer think. When the move to play as been decided the
        /// "MoveFound" event is raised.
        /// </summary>
        /// <param name="moveOrganizer">List of possible moves to select a move from.</param>
        /// <param name="maxSearchDepth">Maximum AlphaBeta depth to search. Must be equal to, or larger then one.</param>
        /// <param name="searchTime">Time before search is stoppen.</param>
        public void Think(MoveOrganizer moveOrganizer, int maxSearchDepth, int searchTime)
        {
			if (!m_workerThread.IsBusy)
			{
				m_abortingSearch = false;
				m_waitHandle.Reset();
				m_workerThread.RunWorkerAsync(new ThreadParam(moveOrganizer, maxSearchDepth, searchTime));
			}
        }

        /// <summary>
		/// Call this to abort the current search without raising the move found event.
        /// </summary>
        public void Abort()
        {
			m_abortingSearch = true;
			m_searchTree.SignalStopSearch();
			m_waitHandle.WaitOne();
        }

        /// <summary>
        /// The function launched in a seperate thread that will carry out the actual search/thinking.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void FindMove(object sender, DoWorkEventArgs e)
        {
			ThreadParam param = (ThreadParam)e.Argument;

			Move move = m_openingBook.Query(param.MoveOrganizer);
			if (move == null)
			{
				move = m_searchTree.FindBestMove(param.MoveOrganizer, param.searchDepth, param.searchTime);
			}

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
            if (!m_abortingSearch && MoveFound != null)
				MoveFound(this, new MoveFoundEventArgs((Move)e.Result));
        }
    }
}
