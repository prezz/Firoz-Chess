using System;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Datastructure used to keep track of the exact piece locations on the board.
  /// </summary>
  class PieceLocationManager
  {
    #region PieceLocationEnumerator

    /// <summary>
    /// Enumerator used to iterate over the locations where there exist a piece.
    /// </summary>
    public class PieceLocationEnumerator
    {
      /// <summary>
      /// Current position of the enumerator in m_locationTable.
      /// </summary>
      private int m_index;

      /// <summary>
      /// Last index in m_locationTable a piece is located.
      /// </summary>
      private int m_lastIndex;

      /// <summary>
      /// Array holding the exact piece locations.
      /// </summary>
      private Square[] m_locationTable;


      /// <summary>
      /// Initializes a new instance of the PieceLocationEnumerator.
      /// </summary>
      /// <param name="locationTable">Location table to iterate.</param>
      /// <param name="lastIndex">Last index in locationTable where a piece exist.</param>
      public PieceLocationEnumerator(Square[] locationTable, int lastIndex)
      {
        m_index = -1;
        m_lastIndex = lastIndex;
        m_locationTable = locationTable;
      }

      /// <summary>
      /// Iterates one step forward to next square where a piece exist.
      /// </summary>
      /// <returns>True if there is a next square with a piece. False otherwise.</returns>
      public bool MoveNext()
      {
        return (++m_index <= m_lastIndex);
      }

      /// <summary>
      /// Returns current square.
      /// </summary>
      public Square Current
      {
        get { return m_locationTable[m_index]; }
      }
    }

    #endregion

    /// <summary>
    /// Size of array holding exact piece locations.
    /// Since all squares might be occupied by pieces when editing board, location list has size equal to board
    /// although it at max holds 16 pieces in actual play.
    /// </summary>
    private const int SIZEOF_LOCATION_LIST = Board.NOF_SQUARS;

    /// <summary>
    /// Size of board array holding board map of piece locations in m_pieceLocationList.
    /// </summary>
    private const int SIZEOF_BOARD_MAP = Board.NOF_SQUARS;

    /// <summary>
    /// Value used in m_boardToListMap to indicate that the square is empty.
    /// </summary>
    private const int NO_TABLE_INDEX = -1;

    /// <summary>
    /// Last index in m_pieceLocationList holding location of a piece.
    /// </summary>
    private int m_locationLast;

    /// <summary>
    /// Array holding all exact piece locations.
    /// </summary>
    private Square[] m_pieceLocationList;

    /// <summary>
    /// Array representing board where each index (square) points to the corrosponding location in m_pieceLocationList.
    /// </summary>
    private int[] m_boardToListMap;


    /// <summary>
    /// Initializes a new instance of PieceLocationManager.
    /// </summary>
    public PieceLocationManager()
    {
      m_pieceLocationList = new Square[SIZEOF_LOCATION_LIST];
      m_boardToListMap = new int[SIZEOF_BOARD_MAP];
      Reset();
    }

    /// <summary>
    /// Initializes a new instance of PieceLocationManager from another PieceLocationManager.
    /// </summary>
    /// <param name="pieceLocationManager">PieceLocationManager to clone from.</param>
    public PieceLocationManager(PieceLocationManager pieceLocationManager)
      : this()
    {
      m_locationLast = pieceLocationManager.m_locationLast;

      for (int i = 0; i < SIZEOF_LOCATION_LIST; ++i)
        m_pieceLocationList[i] = pieceLocationManager.m_pieceLocationList[i];

      for (int i = 0; i < SIZEOF_BOARD_MAP; ++i)
        m_boardToListMap[i] = pieceLocationManager.m_boardToListMap[i];
    }

    /// <summary>
    /// Resets the PieceLocationManager such no information about piece locations are stored.
    /// </summary>
    public void Reset()
    {
      m_locationLast = -1;

      for (int i = 0; i < m_pieceLocationList.Length; ++i)
        m_pieceLocationList[i] = Square.None;

      for (int i = 0; i < m_boardToListMap.Length; ++i)
        m_boardToListMap[i] = NO_TABLE_INDEX;
    }

    /// <summary>
    /// Place a piece on a location.
    /// </summary>
    /// <param name="square">Location to place piece.</param>
    public void PlacePiece(Square square)
    {
      if (m_boardToListMap[(int)square] == NO_TABLE_INDEX)
      {
        ++m_locationLast;
        m_boardToListMap[(int)square] = m_locationLast;
        m_pieceLocationList[m_locationLast] = square;
      }
    }

    /// <summary>
    /// Removes a piece from a location.
    /// </summary>
    /// <param name="square">Location to remove piece from.</param>
    public void RemovePiece(Square square)
    {
      if (m_boardToListMap[(int)square] != NO_TABLE_INDEX)
      {
        if (square == m_pieceLocationList[m_locationLast])
        {
          m_boardToListMap[(int)square] = NO_TABLE_INDEX;
          m_pieceLocationList[m_locationLast] = Square.None;
          --m_locationLast;
        }
        else
        {
          int indexToUpdate = m_boardToListMap[(int)square];
          m_boardToListMap[(int)square] = NO_TABLE_INDEX;

          Square squareToUpdate = m_pieceLocationList[m_locationLast];
          m_pieceLocationList[indexToUpdate] = m_pieceLocationList[m_locationLast];
          m_pieceLocationList[m_locationLast] = Square.None;
          m_boardToListMap[(int)squareToUpdate] = indexToUpdate;
          --m_locationLast;
        }
      }
    }

    /// <summary>
    /// Moves a piece from one location to another.
    /// </summary>
    /// <param name="from">Location to remove piece from.</param>
    /// <param name="to">New location for piece.</param>
    public void MovePiece(Square from, Square to)
    {
      int indexToUpdate = m_boardToListMap[(int)from];
      m_boardToListMap[(int)from] = NO_TABLE_INDEX;

      m_boardToListMap[(int)to] = indexToUpdate;
      m_pieceLocationList[indexToUpdate] = to;
    }

    /// <summary>
    /// Returns an enumerator that will iterate a PieceLocationManager and return all piece locations.
    /// </summary>
    /// <returns></returns>
    public PieceLocationEnumerator GetEnumerator()
    {
      return new PieceLocationEnumerator(m_pieceLocationList, m_locationLast);
    }
  }
}
