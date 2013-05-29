using System;


namespace Chess.Model.Logic
{
    /// <summary>
    /// Represents an undoable en passant capture chess move.
    /// </summary>
    class EnPassantCaptureMove : Move
	{
        /// <summary>
        /// Initializes a new instance of the EnPassantCaptureMove class.
        /// </summary>
        /// <param name="board">Board on which the move is to be performed.</param>
        /// <param name="from">Location move is done from.</param>
        /// <param name="to">Location move is done to.</param>
		public EnPassantCaptureMove(Board board, Square from, Square to)
			: base(board, from, to)
		{
            m_capture = board[board.State.EnPassantTarget];
        }

        /// <summary>
        /// Performs the move. I move is only pseudo legal (putting own king in check)
        /// the move isn't executed.
        /// </summary>
        /// <returns>True if move could be carried out, false otherwise.</returns>
        public override bool Execute()
		{
			BoardState afterState = m_beforeState = m_board.State;

            m_board.PlacePiece(m_beforeState.EnPassantTarget, Piece.None);
            m_board.MovePiece(m_from, m_to);

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
        /// Takes back (undo) the En-Passant capture move. Nothing is done to verify if undoing the move is actually
        /// valid for the board associated to this move.
        /// </summary>
        public override void UnExecute()
		{
            m_board.RemoveFromHistory();

            m_board.MovePiece(m_to, m_from);
            m_board.PlacePiece(m_beforeState.EnPassantTarget, m_capture);

			m_board.State = m_beforeState;
        }
	}
}
