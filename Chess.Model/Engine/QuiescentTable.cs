using System;
using System.Collections.Generic;
using Chess.Model;
using Chess.Model.Logic;


namespace Chess.Model.Engine
{
  #region QuiescentFlag

  /// <summary>
  /// Enumeration if the type of values/scores contained at a position in the table.
  /// </summary>
  enum QuiescentFlag
  {
    NoScore,
    ExactScore,
    AlphaScore,
    BetaScore
  };

  #endregion


  /// <summary>
  /// Transposition table used to hold scores for already evaluated positions
  /// during Quiescent search.
  /// </summary>
  class QuiescentTable
  {
    #region HashEntry

    /// <summary>
    /// Data for a specefic position/hash entry in the table.
    /// </summary>
    struct HashEntry
    {
      public ZobristHash Hash;
      public QuiescentFlag Flag;
      public int Score;
    }

    #endregion


    /// <summary>
    /// Must be power of two.
    /// </summary>
    private const int TABLE_SIZE = 0x100000;


    private bool m_closed;
    private HashEntry[] m_hashTable;


    public QuiescentTable()
    {
      m_hashTable = new HashEntry[TABLE_SIZE];
      for (int i = 0; i < TABLE_SIZE; ++i)
        m_hashTable[i].Hash = new ZobristHash(0, 0);
    }

    public void Open()
    {
      m_closed = false;
    }

    public void Close()
    {
      m_closed = true;
    }

    public void RecordHash(ZobristHash hash, QuiescentFlag flag, int score)
    {
      if (!m_closed)
      {
        int i = CalculateHashIndex(hash);

        m_hashTable[i].Hash.Key = hash.Key;
        m_hashTable[i].Hash.Lock = hash.Lock;
        m_hashTable[i].Flag = flag;
        m_hashTable[i].Score = score;
      }
    }

    public bool ProbeScore(ZobristHash hash, int alpha, int beta, ref int score)
    {
      int i = CalculateHashIndex(hash);

      if (m_hashTable[i].Flag != QuiescentFlag.NoScore && hash.Equals(m_hashTable[i].Hash))
      {
        if (m_hashTable[i].Flag == QuiescentFlag.ExactScore)
        {
          score = m_hashTable[i].Score;
          return true;
        }

        if (m_hashTable[i].Flag == QuiescentFlag.AlphaScore && m_hashTable[i].Score <= alpha)
        {
          score = alpha;
          return true;
        }

        if (m_hashTable[i].Flag == QuiescentFlag.BetaScore && m_hashTable[i].Score >= beta)
        {
          score = beta;
          return true;
        }
      }

      return false;
    }

    private static int CalculateHashIndex(ZobristHash hash)
    {
      return hash.GetHashCode() & (TABLE_SIZE - 1);
    }
  }
}
