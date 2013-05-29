using System;


namespace Chess.Model.Logic
{
    /// <summary>
    /// Represents an basic undoable chess move.
    /// </summary>
    class Move
	{
        /// <summary>
        /// The board on which the move is performed.
        /// </summary>
		protected Board m_board;

        /// <summary>
        /// Location move is done from.
        /// </summary>
		protected Square m_from;

        /// <summary>
        /// Location move is done to.
        /// </summary>
		protected Square m_to;

        /// <summary>
        /// The piece being moved.
        /// </summary>
        protected Piece m_piece;

        /// <summary>
        /// The piece captured (if any) as a result of the move.
        /// </summary>
		protected Piece m_capture;

        /// <summary>
        /// The state of the board before move has been performed.
        /// </summary>
		protected BoardState m_beforeState;

        /// <summary>
        /// The score of the move. Used in search tree.
        /// </summary>
        protected int m_score;

        /// <summary>
        /// The predicted Principal Variation move following this move.
        /// The PV move is the move that is predicted to follow this move in a search. 
        /// </summary>
        protected Move m_pvNextMove;


        /// <summary>
        /// Initializes a new instance of the Move class.
        /// </summary>
        /// <param name="board">Board on which the move is to be performed.</param>
        /// <param name="from">Location move is done from.</param>
        /// <param name="to">Location move is done to.</param>
		public Move(Board board, Square from, Square to)
		{
			m_board = board;
			m_from = from;
			m_to = to;
            m_piece = m_board[m_from];
            m_capture = m_board[m_to];
            m_score = int.MinValue;
		}

        /// <summary>
        /// Returns or sets the board for this move.
        /// </summary>
        public Board Board
		{
			get	{ return m_board; }
		}

        /// <summary>
        /// Returns location move is done from.
        /// </summary>
        public Square From
		{
			get	{ return m_from; }
		}

        /// <summary>
        /// Returns location move is done to.
        /// </summary>
        public Square To
		{
			get	{ return m_to; }
		}

        /// <summary>
        /// Returns the piece being moved.
        /// </summary>
        public Piece Piece
        {
            get { return m_piece; }
        }

        /// <summary>
        /// Returns the capture for this move.
        /// </summary>
        public Piece Capture
        {
            get { return m_capture; }
        }

        /// <summary>
        /// Get or sets the predicted next move to play after this move also called the Principle Variation move.
        /// </summary>
        public Move PvNextMove
        {
            get { return m_pvNextMove; }
            set { m_pvNextMove = value; }
        }

        /// <summary>
        /// Performs the move. I move is only pseudo legal (putting own king in check)
        /// the move isn't executed.
        /// </summary>
        /// <returns>True if move could be carried out, false otherwise.</returns>
		public virtual bool Execute()
		{
			BoardState afterState = m_beforeState = m_board.State;

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
        /// Takes back (undo) the move. Nothing is done to verify if undoing the move is actually
        /// valid for the board associated to this move.
        /// </summary>
		public virtual void UnExecute()
		{
            m_board.RemoveFromHistory();

            m_board.MovePiece(m_to, m_from);
            m_board.PlacePiece(m_to, m_capture);

			m_board.State = m_beforeState;
		}

        /// <summary>
        /// Sets and gets the score for this move.
        /// </summary>
        public int Score
        {
            get { return m_score; }
            set { m_score = value; }
        }

        /// <summary>
        /// Returns true if move is a capture move. Due to performance result
        /// of this 
        /// </summary>
        public bool IsCaptureMove
        {
            get { return m_capture != Piece.None; }
        }

        /// <summary>
        /// Updates what types of castling is possible after the move has been performed. 
        /// </summary>
        /// <param name="afterState">The "BoardState" to update.</param>
		protected void SetCastlingAvailabilety(ref BoardState afterState)
		{
			switch (m_from)
			{
				case Square.E1:
					afterState.WhiteCanCastleLong = false;
					afterState.WhiteCanCastleShort = false;
				break;

				case Square.E8:
					afterState.BlackCanCastleLong = false;
					afterState.BlackCanCastleShort = false;
				break;
			
				case Square.A1:
					afterState.WhiteCanCastleLong = false;
				break;

				case Square.A8:
					afterState.BlackCanCastleLong = false;
				break;
			
				case Square.H1:
					afterState.WhiteCanCastleShort = false;
				break;

				case Square.H8:
					afterState.BlackCanCastleShort = false;
				break;
			}

			switch (m_to)
			{
				case Square.A1:
					afterState.WhiteCanCastleLong = false;
				break;

				case Square.A8:
					afterState.BlackCanCastleLong = false;
				break;
			
				case Square.H1:
					afterState.WhiteCanCastleShort = false;
				break;

				case Square.H8:
					afterState.BlackCanCastleShort = false;
				break;
			}
		}

        /// <summary>
        /// Updates if the board has a pawn that can be hit En-Passant after the move has been performed.
        /// </summary>
        /// <param name="afterState">The "BoardState" to update.</param>
		protected void SetEnPassentTarget(ref BoardState afterState)
		{
			if ((Board.Rank(m_from) == 1 && Board.Rank(m_to) == 3 && m_piece == Piece.WhitePawn) ||
				(Board.Rank(m_from) == 6 && Board.Rank(m_to) == 4 && m_piece == Piece.BlackPawn))
			{
				afterState.EnPassantTarget = m_to;
			}
			else
			{
				afterState.EnPassantTarget = Square.None;
			}
		}

        /// <summary>
        /// Updates the board state as a result of a move has been performed.
        /// </summary>
        /// <param name="afterState">The "BoardState" to update.</param>
		protected void EndTurn(ref BoardState afterState)
		{
            if (m_capture != Piece.None || m_piece == Piece.WhitePawn || m_piece == Piece.BlackPawn)
                afterState.NonHitAndPawnMovesPlayed = 0;
            else
			    afterState.NonHitAndPawnMovesPlayed++;

			switch (m_beforeState.ColorToPlay)
			{
				case PieceColor.White:
					afterState.ColorToPlay = PieceColor.Black;
				break;

				case PieceColor.Black:
					afterState.ColorToPlay = PieceColor.White;
				break;
			}
		}

        /// <summary>
        /// Compares two moves to each other.
        /// </summary>
        /// <param name="obj">Move to compare tho this move.</param>
        /// <returns>True if the moves are equal. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            Move move = obj as Move;
            if (move != null)
                return (this.From == move.From && this.To == move.To && this.Piece == move.Piece && this.Capture == move.Capture);

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
