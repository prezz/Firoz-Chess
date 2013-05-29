using System;
using System.Collections.Generic;
using Chess.Model;
using Chess.Model.Logic;


namespace Chess.Model.Engine
{
  #region AlphaBetaFlag

  enum AlphaBetaFlag
  {
    NoScore,
    ExactScore,
    AlphaScore,
    BetaScore
  };

  #endregion


  class AlphaBetaTable
  {
    #region HashEntry

    struct HashEntry
    {
      public ZobristHash Hash;
      public AlphaBetaFlag Flag;
      public int Depth;
      public int Score;
      public MoveIdentifier PvMove;
    }

    #endregion


    /// <summary>
    /// Must be power of two.
    /// </summary>
    private const int TABLE_SIZE = 0x100000;


    private bool m_closed;
    private HashEntry[] m_hashTable;


    public AlphaBetaTable()
    {
      m_hashTable = new HashEntry[TABLE_SIZE];
      for (int i = 0; i < TABLE_SIZE; ++i)
      {
        m_hashTable[i].Hash = new ZobristHash(0, 0);
        m_hashTable[i].PvMove = new MoveIdentifier(Square.None, Square.None, Piece.None);
      }
    }

    public void Open()
    {
      m_closed = false;
    }

    public void Close()
    {
      m_closed = true;
    }

    public void RecordHash(ZobristHash hash, AlphaBetaFlag flag, int depth, int score, Move pvMove)
    {
      if (!m_closed)
      {
        int i = CalculateHashIndex(hash);

        m_hashTable[i].Hash.Key = hash.Key;
        m_hashTable[i].Hash.Lock = hash.Lock;
        m_hashTable[i].Flag = flag;
        m_hashTable[i].Depth = depth;
        m_hashTable[i].Score = score;

        if (pvMove != null)
        {
          m_hashTable[i].PvMove.From = pvMove.From;
          m_hashTable[i].PvMove.To = pvMove.To;
          m_hashTable[i].PvMove.PromotionPiece = pvMove.PromoteTo;
        }
        else
        {
          m_hashTable[i].PvMove.From = Square.None;
          m_hashTable[i].PvMove.To = Square.None;
          m_hashTable[i].PvMove.PromotionPiece = Piece.None;
        }
      }
    }

    public bool ProbeScore(ZobristHash hash, int alpha, int beta, int depth, ref int score)
    {
      int i = CalculateHashIndex(hash);

      if (m_hashTable[i].Flag != AlphaBetaFlag.NoScore && hash.Equals(m_hashTable[i].Hash))
      {
        if (m_hashTable[i].Depth >= depth)
        {
          if (m_hashTable[i].Flag == AlphaBetaFlag.ExactScore)
          {
            score = m_hashTable[i].Score;
            return true;
          }

          if (m_hashTable[i].Flag == AlphaBetaFlag.AlphaScore && m_hashTable[i].Score <= alpha)
          {
            score = alpha;
            return true;
          }

          if (m_hashTable[i].Flag == AlphaBetaFlag.BetaScore && m_hashTable[i].Score >= beta)
          {
            score = beta;
            return true;
          }
        }
      }

      return false;
    }

    public MoveIdentifier ProbePvMove(ZobristHash hash)
    {
      int i = CalculateHashIndex(hash);

      if (m_hashTable[i].Flag != AlphaBetaFlag.NoScore && hash.Equals(m_hashTable[i].Hash))
        return m_hashTable[i].PvMove;
      else
        return new MoveIdentifier(Square.None, Square.None, Piece.None);
    }

    private static int CalculateHashIndex(ZobristHash hash)
    {
      return hash.GetHashCode() & (TABLE_SIZE - 1);
    }
  }
}
