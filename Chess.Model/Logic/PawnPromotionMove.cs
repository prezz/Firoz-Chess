using System;


namespace Chess.Model.Logic
{
    /// <summary>
    /// Represents an undoable pawn promotion chess move.
    /// </summary>
    class PawnPromotionMove : Move
	{
        /// <summary>
        /// The piece to promote a pawn to.
        /// </summary>
		protected Piece m_promotionPiece;


        /// <summary>
        /// Initializes a new instance of the PawnPromotionMove class.
        /// </summary>
        /// <param name="board">Board on which the move is to be performed.</param>
        /// <param name="from">Location move is done from.</param>
        /// <param name="to">Location move is done to.</param>
        /// <param name="promoteTo">The piece to promote pawn to.</param>
		public PawnPromotionMove(Board board, Square from, Square to, Piece promoteTo)
			: base(board, from, to)
		{
            m_promotionPiece = promoteTo;
		}

        /// <summary>
        /// If the move as not yet been executed returns the piece the pawn is going to be promoted to.
        /// Otherwise it returns a pawn.
        /// </summary>
		public Piece PromotedPiece
		{
			get { return m_promotionPiece; }
		}

        /// <summary>
        /// Performs the move. I move is only pseudo legal (putting own king in check)
        /// the move isn't executed.
        /// </summary>
        /// <returns>True if move could be carried out, false otherwise.</returns>
        public override bool Execute()
		{
			BoardState afterState = m_beforeState = m_board.State;

            m_board.MovePiece(m_from, m_to);
            m_board.PlacePiece(m_to, m_promotionPiece);

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
        /// Takes back (undo) the move. Nothing is done to verify if undoing the move is actually
        /// valid for the board associated to this move.
        /// </summary>
        public override void UnExecute()
		{
            m_board.RemoveFromHistory();

            m_board.PlacePiece(m_to, m_piece);
            m_board.MovePiece(m_to, m_from);
            m_board.PlacePiece(m_to, m_capture);

			m_board.State = m_beforeState;
        }

        /// <summary>
        /// Compares two moves to each other.
        /// </summary>
        /// <param name="obj">Move to compare tho this move.</param>
        /// <returns>True if the moves are equal. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            PawnPromotionMove move = obj as PawnPromotionMove;
            if (move != null)
                return (base.Equals(obj) && this.m_promotionPiece == move.m_promotionPiece);

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
