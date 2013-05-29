using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Chess.Model.Logic;
using Chess.Model.EngineInterface;


namespace Chess.Model.Engine
{
  /// <summary>
  /// Opening book able to hold positions the engine should play immiedetly if encountered
  /// with out doing a search.
  /// </summary>
  class OpeningBook : IOpeningBook
  {
    #region QueryEntry

    struct QueryEntry
    {
      public Move QueryMove;
      public int QueryScore;

      public QueryEntry(Move queryMove, int queryScore)
      {
        QueryMove = queryMove;
        QueryScore = queryScore;
      }
    };

    #endregion


    /// <summary>
    /// File where opening book are stored.
    /// </summary>
    private const string OPENING_BOOK_FILE = "Book.dat";

    /// <summary>
    /// If there are multiple moves to select from this will select a random move of the candidates.
    /// </summary>
    private Random m_moveSelector;

    /// <summary>
    /// List holding the moves m_moveSelector can select from.
    /// </summary>
    private List<QueryEntry> m_queryMoves;

    /// <summary>
    /// Structure holding the ZobristHashes of the positions contained in the opening book.
    /// </summary>
    private Dictionary<ZobristHash, int> m_positionTable;


    /// <summary>
    /// Initializes a new instance of the opening book.
    /// </summary>
    public OpeningBook()
    {
      m_moveSelector = new Random();
      m_queryMoves = new List<QueryEntry>();
      m_positionTable = new Dictionary<ZobristHash, int>();
    }

    /// <summary>
    /// Adds a position to the opening book.
    /// </summary>
    /// <param name="board">Board with position to add.</param>
    /// <param name="adjustValue">value to adjust the boards value with.</param>
    public void AddPosition(Board board, int adjustValue)
    {
      int currentValue = 0;
      ZobristHash boardHash = board.BoardHash(true);

      m_positionTable.TryGetValue(boardHash, out currentValue);
      m_positionTable[boardHash] = currentValue + adjustValue;
    }

    /// <summary>
    /// Returns a move from move organizer contained in the opening book.
    /// </summary>
    /// <param name="board">Board to query best move for.</param>
    /// <param name="moveOrganizer">Move organizer to select move from.</param>
    /// <returns>Null if no moves in moveOrganizer axists in the opening book.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
    public Move Query(Board board, MoveOrganizer moveOrganizer)
    {
      int totalQuaryVal = 0;
      m_queryMoves.Clear();

      foreach (Move move in moveOrganizer)
      {
        if (move.Execute(board))
        {
          int currentQueryVal = 0;
          if (board.State.NonHitAndPawnMovesPlayed < 100 &&
            board.BoardHistoryFrequency() < 3 &&
            m_positionTable.TryGetValue(board.BoardHash(false), out currentQueryVal))
          {
            totalQuaryVal += currentQueryVal;
            m_queryMoves.Add(new QueryEntry(move, currentQueryVal));
          }
          move.UnExecute(board);
        }
      }

      //select a move using probabilety based on the quary value
      int selectValueCurrent = 0;
      int selectValueTarget = (int)Math.Round(m_moveSelector.NextDouble() * totalQuaryVal);
      for (int i = 0; i < m_queryMoves.Count; ++i)
      {
        selectValueCurrent += m_queryMoves[i].QueryScore;
        if (selectValueCurrent >= selectValueTarget)
        {
          OutputWriter.Write("Book moves: " + m_queryMoves.Count + ", Selecting: " + Math.Round((float)m_queryMoves[i].QueryScore / (float)totalQuaryVal, 6));
          return m_queryMoves[i].QueryMove;
        }
      }

      return null;
    }

    /// <summary>
    /// Saves the opening book to disk.
    /// </summary>
    public void Save()
    {
      try
      {
        FileStream fs = new FileStream(OPENING_BOOK_FILE, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);
        foreach (KeyValuePair<ZobristHash, int> keyValuePair in m_positionTable)
        {
          bw.Write(keyValuePair.Key.Key);
          bw.Write(keyValuePair.Key.Lock);
          bw.Write(keyValuePair.Value);
        }
        bw.Close();
        fs.Close();
      }
      catch (Exception e)
      {
        OutputWriter.Write("Exception thrown when saving book: " + e.ToString());
      }
    }

    /// <summary>
    /// Loads the opening book from disk.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
    public void Load()
    {
      try
      {
        if (File.Exists(OPENING_BOOK_FILE))
        {
          FileStream fs = new FileStream(OPENING_BOOK_FILE, FileMode.Open);
          BinaryReader br = new BinaryReader(fs);
          while (br.BaseStream.Position < br.BaseStream.Length)
          {
            int keyVal = br.ReadInt32();
            int lockVal = br.ReadInt32();
            int queryVal = br.ReadInt32();
            ZobristHash zobristHash = new ZobristHash(keyVal, lockVal);
            m_positionTable[zobristHash] = queryVal;
          }
          br.Close();
          fs.Close();

          OutputWriter.Write(m_positionTable.Count.ToString() + " opening moves loaded");
        }
      }
      catch (Exception e)
      {
        OutputWriter.Write("Exception thrown when loading book: " + e.ToString());
      }
    }
  }
}
