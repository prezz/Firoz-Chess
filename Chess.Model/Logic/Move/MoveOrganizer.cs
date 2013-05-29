using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Class used to store generated and possible moves for a given chess position.
  /// </summary>
  class MoveOrganizer
  {
    #region MoveCollectionEnumerator

    /// <summary>
    /// Enumerator used to iterate the moves stored in "MoveCollection".
    /// </summary>
    public class MoveOrganizerEnumerator
    {
      /// <summary>
      /// Current index of the enumerator.
      /// </summary>
      private int m_index;

      /// <summary>
      /// The "MoveOrganizer" to iterate.
      /// </summary>
      private MoveOrganizer m_organizer;


      /// <summary>
      /// Instantiates an instance of the enumerator.
      /// </summary>
      /// <param name="organizer">The "MoveOrganizer" this enumerator iterates.</param>
      public MoveOrganizerEnumerator(MoveOrganizer organizer)
      {
        m_organizer = organizer;
        m_index = -1;
      }

      /// <summary>
      /// Iterates one step forward to the next move.
      /// </summary>
      /// <returns>True if a new "Move" exists. False otherwise.</returns>
      public bool MoveNext()
      {
        return ++m_index < m_organizer.Count;
      }

      /// <summary>
      /// Returns the current "Move".
      /// </summary>
      public Move Current
      {
        get { return m_organizer[m_index]; }
      }
    }

    #endregion


    /// <summary>
    /// List holding the possible moves.
    /// </summary>
    private List<Move> m_moves;

    /// <summary>
    /// 
    /// </summary>
    private MoveIdentifier m_pvMove;


    /// <summary>
    /// Initializes a new instance of MoveOrganizer.
    /// </summary>
    public MoveOrganizer()
    {
      m_moves = new List<Move>(32);
      m_pvMove = new MoveIdentifier(Square.None, Square.None, Piece.None);

      //start adding a null value to make space for the pv move if it is encountered during add
      m_moves.Add(null);
    }

    /// <summary>
    /// Initializes a new instance of MoveOrganizer.
    /// </summary>
    /// <param name="pvMove">Sets the pv move that should be prioratized if encoutered during add.</param>
    public MoveOrganizer(MoveIdentifier pvMove)
    {
      m_moves = new List<Move>(32);
      m_pvMove = pvMove;

      //start adding a null value to make space for the pv move if it is encountered during add
      m_moves.Add(null);
    }

    /// <summary>
    /// Gets the move located at index.
    /// </summary>
    /// <param name="index">The index to get move for.</param>
    /// <returns>Move located at index.</returns>
    public Move this[int index]
    {
      get
      {
        if (m_moves[0] == null)
          return m_moves[index + 1];
        else
          return m_moves[index];
      }
    }

    /// <summary>
    /// The number of moves in MoveOrganizer including the leading space allocated .
    /// </summary>
    public int Count
    {
      get
      {
        if (m_moves[0] == null)
          return m_moves.Count - 1;
        else
          return m_moves.Count;
      }
    }

    /// <summary>
    /// Remove all moves contained in MoveOrganizer.
    /// </summary>
    public void Clear()
    {
      m_moves.Clear();

      //start adding a null value to make space for the pv move if it is encountered during add
      m_moves.Add(null);
    }

    /// <summary>
    /// Returns an enumerator iterating over all moves contained in MoveOrganizer.
    /// </summary>
    /// <returns></returns>
    public MoveOrganizerEnumerator GetEnumerator()
    {
      return new MoveOrganizerEnumerator(this);
    }

    /// <summary>
    /// Appends a move at the end of the MoveOrganizer.
    /// </summary>
    /// <param name="move">Move to add.</param>
    public void Add(Move move)
    {
      if (m_pvMove.From == move.From && m_pvMove.To == move.To && m_pvMove.PromotionPiece == move.PromoteTo)
        m_moves[0] = move;
      else
        m_moves.Add(move);
    }

    /// <summary>
    /// Removes illigal moves due to putting ones own king in check.
    /// </summary>
    public void RemoveSelfCheckingMoves(Board board)
    {
      for (int i = 0; i < m_moves.Count; ++i)
      {
        Move move = m_moves[i];
        if (move != null)
        {
          if (move.Execute(board))
            move.UnExecute(board);
          else
            m_moves.RemoveAt(i--);
        }
      }
    }

    /// <summary>
    /// Sorts the move using the specified IComparer.
    /// The Pv move will not be effected by this sorting.
    /// </summary>
    /// <param name="comparer">The IComparer to use when comparing and sorting moves.</param>
    public void Sort(IComparer<Move> comparer)
    {
      m_moves.Sort(1, m_moves.Count - 1, comparer);
    }

    /// <summary>
    /// Finds the move matching the MoveLookupKey.
    /// </summary>
    /// <param name="match">MoveIdentifier to find corrosponding move for.</param>
    /// <returns>The matching move. Null if a move matching 'match' dosn't exist.</returns>
    public Move Find(MoveIdentifier match)
    {
      foreach (Move move in m_moves)
      {
        if (move != null && move.From == match.From && move.To == match.To)
        {
          if (move.MoveType == MoveType.PawnPromotionMove)
          {
            if (move.PromoteTo == match.PromotionPiece)
            {
              return move;
            }
          }
          else
          {
            return move;
          }
        }
      }

      return null;
    }
  }
}
