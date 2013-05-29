using System;


namespace Chess.Model.Logic
{
    /// <summary>
    /// Represents an undoable castling chess move.
    /// </summary>
	class CastlingMove : Move
	{
        /// <summary>
        /// Location "Rook" is moved from.
        /// </summary>
		protected Square m_rookFrom;

        /// <summary>
        /// Location "Rook" is moved to.
        /// </summary>
		protected Square m_rookTo;


        /// <summary>
        /// Initializes a new instance of the CastlingMove class.
        /// </summary>
        /// <param name="board">Board on which the move is to be performed.</param>
        /// <param name="from">Location kings move is done from.</param>
        /// <param name="to">Location kings move is done to.</param>
        public CastlingMove(Board board, Square from, Square to)
			: base(board, from, to)
		{
			switch (from)
			{
				case Square.E1:
					switch (to)
					{
						case Square.C1:
							m_rookFrom = Square.A1;
							m_rookTo = Square.D1;
						break;
					
						case Square.G1:
							m_rookFrom = Square.H1;
							m_rookTo = Square.F1;
						break;
					}
				break;

				case Square.E8:
					switch (to)
					{
						case Square.C8:
							m_rookFrom = Square.A8;
							m_rookTo = Square.D8;
						break;

						case Square.G8:
							m_rookFrom = Square.H8;
							m_rookTo = Square.F8;
						break;
					}
				break;
			}
		}

        /// <summary>
        /// Performs the move. I move is only pseudo legal (putting own king in check)
        /// the move isn't executed.
        /// </summary>
        /// <returns>True if move could be carried out, false otherwise.</returns>
        public override bool Execute()
		{
			BoardState afterState = m_beforeState = m_board.State;

			//Kings move
            m_board.MovePiece(m_from, m_to);

			//Rooks move
            m_board.MovePiece(m_rookFrom, m_rookTo);

			SetCastlingAvailabilety(ref afterState);
			SetEnPassentTarget(ref afterState);
			EndTurn(ref afterState);

			m_board.State = afterState;
            m_board.AddToHistory();

            if (m_board.IsCheck(m_beforeState.ColorToPlay))
            {
                UnExecute();
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Takes back (undo) the castling move. Nothing is done to verify if undoing the move is actually
        /// valid for the board associated to this move.
        /// </summary>
        public override void UnExecute()
		{
            m_board.RemoveFromHistory();
            
            //Undoing rooks move
            m_board.MovePiece(m_rookTo, m_rookFrom);

			//Undoing kings move
            m_board.MovePiece(m_to, m_from);

			m_board.State = m_beforeState;
		}

        /// <summary>
        /// Compares two moves to each other.
        /// </summary>
        /// <param name="obj">Move to compare tho this move.</param>
        /// <returns>True if the moves are equal. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            CastlingMove move = obj as CastlingMove;
            if (move != null)
                return (base.Equals(obj) && this.m_rookFrom == move.m_rookFrom && this.m_rookTo == move.m_rookTo);

            return false;
        }

        /// <summary>
        /// Not used in chess program. Only declared to avoid compiler warning due to implementation of Equals.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
	}
}
