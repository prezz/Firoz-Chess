using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Holds the Zobrist hash history of boards played in a game of chess.
  /// </summary>
  class HashHistory
  {
    /// <summary>
    /// Contains pairs consisting of a board hash and the frequency of that
    /// board in the history of the game.
    /// </summary>
    private Dictionary<ZobristHash, int> m_gameHashHistory;


    /// <summary>
    /// Initializes a new instance of the HashHistory class.
    /// </summary>
    public HashHistory()
    {
      m_gameHashHistory = new Dictionary<ZobristHash, int>();
    }

    /// <summary>
    /// Initializes a new instance of the HashHistory class from another HashHistory.
    /// </summary>
    /// <param name="hashHistory">HashHistory to clone from.</param>
    public HashHistory(HashHistory hashHistory)
      : this()
    {
      foreach (KeyValuePair<ZobristHash, int> valuePair in hashHistory.m_gameHashHistory)
        m_gameHashHistory.Add(new ZobristHash(valuePair.Key), valuePair.Value);
    }

    /// <summary>
    /// Adds a board to the history. If the board already exists the frequency is increased.
    /// </summary>
    /// <param name="board">Board to add.</param>
    public void AddBoard(Board board)
    {
      int count;
      ZobristHash hash = board.BoardHash(true);

      m_gameHashHistory.TryGetValue(hash, out count);
      m_gameHashHistory[hash] = count + 1;
    }

    /// <summary>
    /// Removes a board from the history. If the frequency for
    /// the board is larger then one the frequency is decreased.
    /// </summary>
    /// <param name="board">Board to remove.</param>
    public void RemoveBoard(Board board)
    {
      int count;
      ZobristHash hash = board.BoardHash(true);

      m_gameHashHistory.TryGetValue(hash, out count);
      if (count > 1)
        m_gameHashHistory[hash] = count - 1;
      else
        m_gameHashHistory.Remove(hash);
    }

    /// <summary>
    /// Obtains the frequency of a board in the history.
    /// </summary>
    /// <param name="board">Board to return frequency for.</param>
    /// <returns>Frequency of board.</returns>
    public int BoardFrequency(Board board)
    {
      int count;
      m_gameHashHistory.TryGetValue(board.BoardHash(false), out count);
      return count;
    }
  }
}
