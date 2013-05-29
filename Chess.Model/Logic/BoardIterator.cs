using System;


namespace Chess.Model.Logic
{
    #region Direction

    /// <summary>
    /// Directions BoardIterator can iterate on the board.
    /// </summary>
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        NoDirection,
    }

    #endregion


    /// <summary>
    /// Iterator used by all FlyweightPieces when generating moves.
    /// </summary>
    class BoardIterator
	{
        /// <summary>
        /// Board to iterate.
        /// </summary>
		private Board m_board;

        /// <summary>
        /// Direction to iterate.
        /// </summary>
		private Direction m_direction;

        /// <summary>
        /// The current position of the iterator.
        /// </summary>
		private Square m_currentSquare;


        /// <summary>
        /// Initializes a new instance of the BoardIterator class.
        /// </summary>
        /// <param name="board">Board to iterate</param>
        /// <param name="startSquare">Square where iteration is started.</param>
        /// <param name="direction">Direction to iterate.</param>
		public BoardIterator(Board board, Square startSquare, Direction direction)
		{
			m_board = board;
			m_currentSquare = startSquare;
			m_direction = direction;
		}

        /// <summary>
        /// Moves the iterator to the next square.
        /// </summary>
        /// <param name="direction">Direction to move.</param>
        /// <returns>True if if the new square is a valid and existing square. False otherwise.</returns>
		public bool Next(Direction direction)
		{
			m_direction = direction;
			return Next();
		}

        /// <summary>
        /// Moves the iterator to the next square.
        /// </summary>
        /// <returns>True if if the new square is a valid and existing square. False otherwise.</returns>
		public bool Next()
		{
			switch (m_direction)
			{
				case Direction.Up:
					if (Board.Rank(m_currentSquare) == Board.NOF_SIDE_SQUARS - 1)
							return false;

					m_currentSquare += 8;
					return true;

				case Direction.UpRight:
					if (Board.Rank(m_currentSquare) == Board.NOF_SIDE_SQUARS - 1 ||
						Board.File(m_currentSquare) == Board.NOF_SIDE_SQUARS - 1)
							return false;

					m_currentSquare += 9;
					return true;

				case Direction.Right:
					if (Board.File(m_currentSquare) == Board.NOF_SIDE_SQUARS - 1)
						return false;

					m_currentSquare += 1;
					return true;

				case Direction.DownRight:
					if (Board.Rank(m_currentSquare) == 0 ||
						Board.File(m_currentSquare) == Board.NOF_SIDE_SQUARS - 1)
						return false;

					m_currentSquare -= 7;
					return true;

				case Direction.Down:
					if (Board.Rank(m_currentSquare) == 0)
						return false;

					m_currentSquare -= 8;
					return true;

				case Direction.DownLeft:
					if (Board.Rank(m_currentSquare) == 0 ||
						Board.File(m_currentSquare) == 0)
						return false;

					m_currentSquare -= 9;
					return true;

				case Direction.Left:
					if (Board.File(m_currentSquare) == 0)
						return false;

					m_currentSquare -= 1;
					return true;

				case Direction.UpLeft:
					if (Board.Rank(m_currentSquare) == Board.NOF_SIDE_SQUARS - 1 ||
						Board.File(m_currentSquare) == 0)
						return false;

					m_currentSquare += 7;
					return true;

				case Direction.NoDirection:
				default:
					return false;
			}
		}

        /// <summary>
        /// Returns the square the iterator is currently located on.
        /// </summary>
        /// <returns>Current square.</returns>
		public Square CurrentSquare()
		{
			return m_currentSquare;
		}

        /// <summary>
        /// Returns the piece on the square the iterator is currently located on.
        /// </summary>
        /// <returns>Current piece.</returns>
		public Piece CurrentPiece()
		{
			return m_board[m_currentSquare];
		}

        /// <summary>
        /// Returns the color of the piece on the square the iterator is currently located on.
        /// </summary>
        /// <returns>Current color of piece.</returns>
        public PieceColor CurrentPieceColor()
		{
			return m_board.GetPieceColor(m_currentSquare);
		}

        /// <summary>
        /// Returns the direction iterator currently is iterating.
        /// </summary>
        /// <returns>Current direction.</returns>
		public Direction CurrentDirection()
		{
			return m_direction;
		}

        /// <summary>
        /// Resets the iterator.
        /// </summary>
        /// <param name="board">Board to iterate</param>
        /// <param name="startSquare">Square where iteration is started.</param>
        /// <param name="direction">Direction to iterate.</param>
        public void Reset(Board board, Square startSquare, Direction direction)
		{
			m_board = board;
			m_currentSquare = startSquare;
			m_direction = direction;
		}
	}
}
