using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
    #region MoveLookupKey

    /// <summary>
    /// Struct used to find a corrosponding "Move" object stored in "MoveManager".
    /// </summary>
    struct MoveLookupKey
    {
        /// <summary>
        /// Square the move is performed from.
        /// </summary>
        public Square From;

        /// <summary>
        /// Square move is performed to.
        /// </summary>
        public Square To;

        /// <summary>
        /// The piece type a pawn is promoted to in case the move we are trying to find is a promotion move.
        /// The value of this is irrelevant if the move we want to get is not a pawn promotion move.
        /// </summary>
        public Piece PawnPromotePiece;
    }

    #endregion


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
                get
                {
                    return m_organizer[m_index];
                }
            }
        }

        #endregion


        /// <summary>
        /// List holding the possible moves.
        /// </summary>
        private List<Move> m_moves;


        /// <summary>
        /// Initializes a new instance of MoveOrganizer
        /// </summary>
        public MoveOrganizer()
        {
            m_moves = new List<Move>();
        }

        /// <summary>
        /// Gets the move located at index.
        /// </summary>
        /// <param name="index">The index to get move for.</param>
        /// <returns>Move located at index.</returns>
        public Move this[int index]
        {
            get { return m_moves[index]; }
        }

        /// <summary>
        /// The number of moves in MoveOrganizer.
        /// </summary>
        public int Count
        {
            get { return m_moves.Count; }
        }

        /// <summary>
        /// Remove all moves contained in MoveOrganizer.
        /// </summary>
        public void Clear()
        {
            m_moves.Clear();
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
            m_moves.Add(move);
        }

        /// <summary>
        /// Removes illigal moves due to putting ones own king in check.
        /// </summary>
        public void RemoveSelfCheckingMoves()
        {
            for (int i = 0; i < m_moves.Count; i++)
            {
                Move move = m_moves[i];
                if (move.Execute())
                    move.UnExecute();
                else
                    m_moves.RemoveAt(i--);
            }
        }

        /// <summary>
        /// Sorts the move using the specified IComparer
        /// </summary>
        /// <param name="comparer">The IComparer to use when comparing and sorting moves.</param>
        public void Sort(IComparer<Move> comparer)
        {
            m_moves.Sort(comparer);
        }

        /// <summary>
        /// Finds the move matching the MoveLookupKey.
        /// </summary>
        /// <param name="match">MoveLookupKey to find corrosponding move for.</param>
        /// <returns>The matching move. Null if a move matching 'match' dosn't exist.</returns>
        public Move Find(MoveLookupKey match)
        {
            foreach (Move move in m_moves)
            {
                if (move.From == match.From && move.To == match.To)
                {
                    PawnPromotionMove promotionMove = move as PawnPromotionMove;

                    if (promotionMove != null)
                    {
                        if (promotionMove.PromotedPiece == match.PawnPromotePiece)
                        {
                            return promotionMove;
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
