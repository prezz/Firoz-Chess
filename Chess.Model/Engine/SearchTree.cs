using System;
using System.Collections.Generic;
using System.ComponentModel;
using Chess.Model.Logic;
using Chess.Model.EngineInterface;


namespace Chess.Model.Engine
{
  class SearchTree : ISearchTree
  {
    #region ScoreMove

    /// <summary>
    /// Class holding a move and its associated score.
    /// </summary>
    class ScoreMove
    {
      /// <summary>
      /// The move score is held for.
      /// </summary>
      public Move Move;

      /// <summary>
      /// The score for the corrosponding move.
      /// </summary>
      public int ValidScore;

      /// <summary>
      /// The temporary score for the corrosponding move.
      /// </summary>
      public int TemporaryScore;


      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="move">Move to attach to this move score.</param>
      public ScoreMove(Move move)
      {
        Move = move;
        ValidScore = int.MinValue;
        TemporaryScore = int.MinValue;
      }
    }

    #endregion


    #region CaptureMoveComparer

    /// <summary>
    /// Compare used to order moves according to Most Valuable Victim/Least Valuable Attacker.
    /// But it is also possible to specify a specefic move that is prioritized highest and placed
    /// first regardless of capture.
    /// </summary>
    class CaptureMoveComparer : IComparer<Move>
    {
      /// <summary>
      /// Reference to a board evaluator. Used to get the value of a piece such the can be ordered
      /// by Most Valuable Victim/Least Valuable Attacker.
      /// </summary>
      private IBoardEvaluator m_evaluator;


      /// <summary>
      /// Initializes a new instance of the CaptureMoveComparer.
      /// </summary>
      /// <param name="evaluator"></param>
      public CaptureMoveComparer(IBoardEvaluator evaluator)
      {
        m_evaluator = evaluator;
      }

      /// <summary>
      /// Campares to move to each other.
      /// </summary>
      /// <param name="x">First move to campare.</param>
      /// <param name="y">Second move to compare.</param>
      /// <returns>-1 if x is best. 1 if y is best. 0 otherwise.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
      public int Compare(Move x, Move y)
      {
        if (x.IsCaptureMove && !y.IsCaptureMove)
          return -1;
        if (!x.IsCaptureMove && y.IsCaptureMove)
          return 1;
        if (x.IsCaptureMove && y.IsCaptureMove)
        {
          int xCaptureVal = m_evaluator.PieceValue(x.Capture);
          int yCaptureVal = m_evaluator.PieceValue(y.Capture);

          if (xCaptureVal > yCaptureVal)
            return -1;
          if (xCaptureVal < yCaptureVal)
            return 1;

          int xAttackerVal = m_evaluator.PieceValue(x.Piece);
          int yAttackerVal = m_evaluator.PieceValue(y.Piece);

          if (xAttackerVal < yAttackerVal)
            return -1;
          if (xAttackerVal > yAttackerVal)
            return 1;
        }
        //TODO: Also sort according to pawn promotion moves.

        return 0;
      }
    }

    #endregion


    #region ScoreMoveComparer

    /// <summary>
    /// Compare used to order moves according to score. If two moves has the
    /// same score the move ordering used in CaptureMoveComparer applies instead.
    /// </summary>
    class ScoreMoveComparer : IComparer<ScoreMove>
    {
      /// <summary>
      /// Capture move compare used for sorting moves with equal score.
      /// </summary>
      private CaptureMoveComparer m_captureMoveCompare;


      /// <summary>
      /// Initializes a new instance of the ScoreMoveComparer.
      /// </summary>
      /// <param name="evaluator"></param>
      public ScoreMoveComparer(IBoardEvaluator evaluator)
      {
        m_captureMoveCompare = new CaptureMoveComparer(evaluator);
      }

      /// <summary>
      /// Campares to move to each other.
      /// </summary>
      /// <param name="x">First move to campare.</param>
      /// <param name="y">Second move to compare.</param>
      /// <returns>-1 if x is best. 1 if y is best. 0 otherwise.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
      public int Compare(ScoreMove x, ScoreMove y)
      {
        if (x.ValidScore > y.ValidScore)
          return -1;
        if (x.ValidScore < y.ValidScore)
          return 1;

        return m_captureMoveCompare.Compare(x.Move, y.Move);
      }
    }

    #endregion


    #region NullMove

    /// <summary>
    /// Null move performed during search. A null move is a move that is not actually carried out such
    /// a player skibs a turn. The idea is that you give the opponent a free shot at you, and if your
    /// position is still so good that you exceed beta, you assume that you'd also exceed beta if you
    /// went and searched all of your moves. The reason this saves time is that this initial search is made
    /// with reduced depth. The depth reduction factor is called R. So rather than searching all of your
    /// moves to depth a certain depth, you search your opponent's moves with a reduced depth. An excellent
    /// reduction value (R) is 2. So, if for instance you were going to search all of your moves to depth 6,
    /// you end up searching all of your opponent's moves to depth 4.
    /// </summary>
    class NullMove
    {
      /// <summary>
      /// Board to carry out null move on
      /// </summary>
      private Board m_board;

      /// <summary>
      /// Reference to a board evaluator. Used to get the value of a piece such engame can be detected.
      /// </summary>
      private IBoardEvaluator m_evaluator;

      /// <summary>
      /// State before null move was carried out.
      /// </summary>
      private BoardState m_beforeState;


      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="board">Board to carry out null move on.</param>
      /// <param name="evaluator">The current board evaluator.</param>
      public NullMove(Board board, IBoardEvaluator evaluator)
      {
        m_board = board;
        m_evaluator = evaluator;
      }

      /// <summary>
      /// Performs the null move.
      /// </summary>
      /// <returns></returns>
      public bool Execute()
      {
        if (!m_board.ColorToPlayIsCheck() && !ColorToPlayZugzwang(m_board))
        {
          BoardState afterState = m_beforeState = m_board.State;
          afterState.EnPassantTarget = Square.None;
          ++afterState.NonHitAndPawnMovesPlayed;

          switch (afterState.ColorToPlay)
          {
            case PieceColor.White:
              afterState.ColorToPlay = PieceColor.Black;
              break;

            case PieceColor.Black:
              afterState.ColorToPlay = PieceColor.White;
              break;
          }

          m_board.State = afterState;
          return true;
        }
        else
        {
          return false;
        }
      }

      /// <summary>
      /// Unexecutes or undoes the null move.
      /// </summary>
      public void UnExecute()
      {
        m_board.State = m_beforeState;
      }

      /// <summary>
      /// Checks if the current state of the game has potential for
      /// Zugzwang. A zugzwang is a position where you are fine if you don't move, but
      /// your position collapses if you are forced to move. We simply detect this by
      /// checking if we are in an endgame.
      /// </summary>
      /// <param name="board">board to check.</param>
      /// <returns>True if we are in an endgame and there is possabilety of zugzwang.</returns>
      private bool ColorToPlayZugzwang(Board board)
      {
        switch (board.State.ColorToPlay)
        {
          case PieceColor.White:
            {
              int score = 0;
              foreach (Square square in board.WhitePieceLocations)
              {
                if (board[square] == Piece.WhiteRook || board[square] == Piece.WhiteBishop ||
                  board[square] == Piece.WhiteKnight || board[square] == Piece.WhiteQueen)
                {
                  score += m_evaluator.PieceValue(board[square]);
                  if (score > m_evaluator.PieceValue(Piece.WhiteRook))
                    return false;
                }
              }
            }
            break;

          case PieceColor.Black:
            {
              int score = 0;
              foreach (Square square in board.BlackPieceLocations)
              {
                if (board[square] == Piece.BlackRook || board[square] == Piece.BlackBishop ||
                  board[square] == Piece.BlackKnight || board[square] == Piece.BlackQueen)
                {
                  score += m_evaluator.PieceValue(board[square]);
                  if (score > m_evaluator.PieceValue(Piece.BlackRook))
                    return false;
                }
              }
            }
            break;
        }

        return true;
      }
    }

    #endregion


    /// <summary>
    /// Board evaluator to use when a board is to be evaluated.
    /// </summary>
    private IBoardEvaluator m_evaluator;

    /// <summary>
    /// Transposition table used for storing scores for moves during alpha beta search.
    /// Prevents a position to be reseached if it has ben searched previously. 
    /// </summary>
    private AlphaBetaTable m_alphaBetaTable;

    /// <summary>
    /// Transposition table used for storing scores for moves during quiescent search.
    /// Prevents a position to be reseached if it has ben searched previously. 
    /// </summary>
    private QuiescentTable m_quiescentTable;

    /// <summary>
    /// Comparere used when orderring moves according to captures.
    /// </summary>
    private CaptureMoveComparer m_captureMoveCompare;

    /// <summary>
    /// Comparere used when orderring moves according to score.
    /// </summary>
    private ScoreMoveComparer m_scoreMoveCompare;

    /// <summary>
    /// Board being searched.
    /// </summary>
    private Board m_board;

    /// <summary>
    /// Time when the search was started.
    /// </summary>
    private DateTime m_searchStartTime;

    /// <summary>
    /// Maximum time to search for a move.
    /// </summary>
    private TimeSpan m_searchMaxTime;

    /// <summary>
    /// Number of nodes visited in the search.
    /// </summary>
    private int m_nodesVisited;

    /// <summary>
    /// Number of nodes that should be visited before checking for abortion during search.
    /// </summary>
    private int m_nextRollbackCheck;

    /// <summary>
    /// Flag indicating if search has been aborted down in search three.
    /// </summary>
    private bool m_searchRollback;

    /// <summary>
    /// Holds the total search depth we are searching.
    /// </summary>
    int m_searchDepth;

    /// <summary>
    /// Flag indicating if search should be aborted.
    /// </summary>
    volatile private bool m_externalAbort;

    /// <summary>
    /// Amount to reduce search depth when a null move is being performed.
    /// </summary>
    const int NULL_REDUCTION = 2;

    /// <summary>
    /// Number of nodes to visit between making a check for if search should be aborted.
    /// </summary>
    const int ROLLBACK_INTERVAL_COUNT = 5000;


    /// <summary>
    /// Initializes a new instance of a search tree.
    /// </summary>
    /// <param name="evaluator">Evaluator to use when evaluating a board position.</param>
    public SearchTree(IBoardEvaluator evaluator)
    {
      m_evaluator = evaluator;
      m_alphaBetaTable = new AlphaBetaTable();
      m_quiescentTable = new QuiescentTable();
      m_captureMoveCompare = new CaptureMoveComparer(m_evaluator);
      m_scoreMoveCompare = new ScoreMoveComparer(m_evaluator);
    }

    /// <summary>
    /// Signals that the search tree should stop searching when possible.
    /// </summary>
    public void SignalStopSearch()
    {
      m_externalAbort = true;
    }

    /// <summary>
    /// Starts a iterative depending search, returning the move engine is to play.
    /// </summary>
    /// <param name="board">Board to find move for.</param>
    /// <param name="moveOrganizer">move organizer to where best move is to be found in.</param>
    /// <param name="aimSearchTime">The search time to aim for. Actual search time might be between this and maxSearchTime.</param>
    /// <param name="maxSearchTime">The maximum amount of seconds to seach for move.</param>
    /// <param name="maxSearchDepth">Maximum depth for full width search.</param>
    /// <returns>Move considered best.</returns>
    public Move FindBestMove(Board board, MoveOrganizer moveOrganizer, TimeSpan aimSearchTime, TimeSpan maxSearchTime, int maxSearchDepth)
    {
      moveOrganizer.Sort(m_captureMoveCompare);

      List<ScoreMove> scoredMoves = new List<ScoreMove>(moveOrganizer.Count);
      foreach (Move move in moveOrganizer)
        scoredMoves.Add(new ScoreMove(move));

      if (scoredMoves.Count == 1)
        return scoredMoves[0].Move;

      m_alphaBetaTable.Open();
      m_quiescentTable.Open();
      m_searchRollback = m_externalAbort = false;
      m_board = board;
      m_searchStartTime = DateTime.Now;
      m_searchMaxTime = maxSearchTime;
      m_nodesVisited = 0;
      m_nextRollbackCheck = ROLLBACK_INTERVAL_COUNT;

      bool continueSearch = true;
      for (m_searchDepth = 0; m_searchDepth <= maxSearchDepth && continueSearch; ++m_searchDepth)
      {
        int movesSearched = 0;
        int alpha = m_evaluator.AlphaValue;
        int beta = -m_evaluator.AlphaValue;

        foreach (ScoreMove scoreMove in scoredMoves)
        {
          scoreMove.Move.Execute(m_board);

          if (movesSearched == 0)
          {
            scoreMove.TemporaryScore = -AlphaBetaSearch(-beta, -alpha, m_searchDepth, false, true);
          }
          else //search in the same way as we do with pv moves by narrowing together alpha and beta
          {
            scoreMove.TemporaryScore = -AlphaBetaSearch(-alpha - 1, -alpha, m_searchDepth, false, false);
            if ((scoreMove.TemporaryScore > alpha) && (scoreMove.TemporaryScore < beta))
              scoreMove.TemporaryScore = -AlphaBetaSearch(-beta, -alpha, m_searchDepth, false, true);
          }

          scoreMove.Move.UnExecute(m_board);

          if (scoreMove.TemporaryScore > alpha)
            alpha = scoreMove.TemporaryScore - 1;

          if (m_searchRollback || m_externalAbort || (scoreMove.ValidScore >= -m_evaluator.MateValue))
          {
            continueSearch = false;
            break;
          }

          ++movesSearched;
        }

        if (movesSearched == scoredMoves.Count)
          foreach (ScoreMove scoreMove in scoredMoves)
            scoreMove.ValidScore = scoreMove.TemporaryScore;

        scoredMoves.Sort(m_scoreMoveCompare);
        OutputWriter.Write("Depth: " + m_searchDepth + ", Score: " + scoredMoves[0].ValidScore);

        if ((DateTime.Now - m_searchStartTime) >= aimSearchTime)
          continueSearch = false;
      }

      OutputWriter.Write("Nodes: " + m_nodesVisited);
      OutputWriter.Write("Time: " + (DateTime.Now - m_searchStartTime).ToString());

      if (m_externalAbort)
      {
        OutputWriter.Write("Search aborted");
        return null;
      }

      int canditates;
      for (canditates = 1; canditates < scoredMoves.Count; ++canditates)
      {
        if (m_scoreMoveCompare.Compare(scoredMoves[0], scoredMoves[canditates]) != 0)
          break;
      }

      Random selector = new Random();
      int idx = selector.Next(canditates);
      OutputWriter.Write("Selecting: " + scoredMoves[idx].Move.From + " " + scoredMoves[idx].Move.To + " from " + scoredMoves.Count + " candidates");
      return scoredMoves[idx].Move;
    }

    /// <summary>
    /// Recursively carries out a full width alpha beta search untill the bottom is reached.
    /// From then a Quiescent search is carried out to avoid horrison effects.
    /// </summary>
    /// <param name="alpha">This is the best score that can be forced by some means. Anything worth less than this is of no use, because there is a strategy that is known to result in a score of alpha.</param>
    /// <param name="beta">Beta is the worst thing that the opponent has to endure. If the search finds something that returns a score of beta or better, it's too good, so the side to move is not going to get a chance to use this strategy.</param>
    /// <param name="remainingDepth">Remaining depth to search before bottom is reached.</param>
    /// <param name="allowNullMove">Is a null move allowed at this search depth.</param>
    /// <param name="allowTranspotitionTable">Is it allowed to use the transpotition table to probe for scores.</param>
    /// <returns>The score of a board.</returns>
    private int AlphaBetaSearch(int alpha, int beta, int remainingDepth, bool allowNullMove, bool allowTranspotitionTable)
    {
      if (m_searchRollback)
        return beta;

      //At interval check if time is up and rollback if so.
      if (m_nodesVisited == m_nextRollbackCheck)
      {
        if (AbortingSearch())
        {
          m_alphaBetaTable.Close();
          m_quiescentTable.Close();
          m_searchRollback = true;
          return beta;
        }

        m_nextRollbackCheck += ROLLBACK_INTERVAL_COUNT;
      }
      ++m_nodesVisited;

      int score = 0;
      Move pvMove = null;
      AlphaBetaFlag alphaBetaFlag = AlphaBetaFlag.AlphaScore;

      if (m_board.State.NonHitAndPawnMovesPlayed >= 100)
        return 0;

      if (m_board.BoardHistoryFrequency() >= 3)
        return 0;

      //don't allow transposition table before both players has passed the above repetetion draw checks.
      if (allowTranspotitionTable && (m_searchDepth - remainingDepth) > 0)
        if (m_alphaBetaTable.ProbeScore(m_board.BoardHash(false), alpha, beta, remainingDepth, ref score))
          if (Math.Abs(score) != Math.Abs(m_evaluator.MateValue)) //don't probe a mate value as this actuall might be a mate far down in the tree, and we want to find the mate at lowest depth.
            return score;

      if (remainingDepth == 0)
      {
        score = QuiescentSearch(alpha, beta);

        if (allowTranspotitionTable)
          m_alphaBetaTable.RecordHash(m_board.BoardHash(false), AlphaBetaFlag.ExactScore, remainingDepth, score, pvMove);

        return score;
      }

      if (allowNullMove)
      {
        //Carry out a null move which is a move where nothing is actually moved. This makes the opponent move
        //twice in a row. The idea is then to search to a reduced depth and if nothing good come out of this
        //(for the opponent) the move is considered garbage and we cut it off.
        NullMove nullMove = new NullMove(m_board, m_evaluator);
        if (nullMove.Execute())
        {
          int remDepth = (remainingDepth > NULL_REDUCTION) ? remainingDepth - 1 - NULL_REDUCTION : 0;
          score = -AlphaBetaSearch(-beta, -beta + 1, remDepth, false, false);
          nullMove.UnExecute();

          if (score >= beta)
            return beta;
        }
      }

      MoveOrganizer currentMoves = new MoveOrganizer(m_alphaBetaTable.ProbePvMove(m_board.BoardHash(false)));
      m_board.GeneratePseudoLegalMoves(currentMoves);
      currentMoves.Sort(m_captureMoveCompare);

      bool validMoveFound = false;
      foreach (Move move in currentMoves)
      {
        if (move.Execute(m_board))
        {
          validMoveFound = true;

          if (pvMove == null)
          {
            score = -AlphaBetaSearch(-beta, -alpha, remainingDepth - 1, true, allowTranspotitionTable);
          }
          else
          {
            score = -AlphaBetaSearch(-alpha - 1, -alpha, remainingDepth - 1, true, false);
            if ((score > alpha) && (score < beta))
              score = -AlphaBetaSearch(-beta, -alpha, remainingDepth - 1, true, allowTranspotitionTable);
          }
          move.UnExecute(m_board);

          if (score >= beta)
          {
            if (allowTranspotitionTable)
              m_alphaBetaTable.RecordHash(m_board.BoardHash(false), AlphaBetaFlag.BetaScore, remainingDepth, beta, pvMove);

            return beta;
          }

          if (score > alpha)
          {
            alpha = score;
            pvMove = move;
            alphaBetaFlag = AlphaBetaFlag.ExactScore;
          }
        }
      }

      if (!validMoveFound)
      {
        if (m_board.ColorToPlayIsCheck())
          alpha = m_evaluator.MateValue; //Checkmate
        else
          alpha = 0; //Stalemate
      }

      if (allowTranspotitionTable)
        m_alphaBetaTable.RecordHash(m_board.BoardHash(false), alphaBetaFlag, remainingDepth, alpha, pvMove);

      return alpha;
    }

    /// <summary>
    /// Carries out a narrow Quiescent search where only capure moves are searched.
    /// </summary>
    /// <param name="alpha">Current alpha value.</param>
    /// <param name="beta">Current beta value.</param>
    /// <returns>The score of a board.</returns>
    private int QuiescentSearch(int alpha, int beta)
    {
      if (m_searchRollback)
        return beta;

      //At interval check if time is up and rollback if so.
      if (m_nodesVisited == m_nextRollbackCheck)
      {
        if (AbortingSearch())
        {
          m_alphaBetaTable.Close();
          m_quiescentTable.Close();
          m_searchRollback = true;
          return beta;
        }

        m_nextRollbackCheck += ROLLBACK_INTERVAL_COUNT;
      }
      ++m_nodesVisited;

      int score = 0;
      QuiescentFlag quiescentFlag = QuiescentFlag.AlphaScore;

      if (m_quiescentTable.ProbeScore(m_board.BoardHash(false), alpha, beta, ref score))
        return score;

      score = m_evaluator.EvaluatePosition(m_board);

      if (score >= beta)
      {
        m_quiescentTable.RecordHash(m_board.BoardHash(false), QuiescentFlag.BetaScore, beta);
        return beta;
      }

      if (score > alpha)
      {
        alpha = score;
        quiescentFlag = QuiescentFlag.ExactScore;
      }

      MoveOrganizer currentMoves = new MoveOrganizer();
      m_board.GeneratePseudoLegalMoves(currentMoves);
      currentMoves.Sort(m_captureMoveCompare);

      foreach (Move move in currentMoves)
      {
        if (!move.IsCaptureMove)
          break;

        if (move.Execute(m_board))
        {
          score = -QuiescentSearch(-beta, -alpha);
          move.UnExecute(m_board);

          if (score >= beta)
          {
            m_quiescentTable.RecordHash(m_board.BoardHash(false), QuiescentFlag.BetaScore, beta);
            return beta;
          }

          if (score > alpha)
          {
            alpha = score;
            quiescentFlag = QuiescentFlag.ExactScore;
          }
        }
      }

      m_quiescentTable.RecordHash(m_board.BoardHash(false), quiescentFlag, alpha);
      return alpha;
    }

    /// <summary>
    /// Used to check if the search should be aborted either becourse time is up or becource a user has aborted.
    /// </summary>
    /// <returns>True if requirements to abort are met. False if search can continue.</returns>
    private bool AbortingSearch()
    {
      return m_externalAbort || ((DateTime.Now - m_searchStartTime) >= m_searchMaxTime);
    }
  }
}
