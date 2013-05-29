using System;


namespace Chess.Model.Logic
{
    /// <summary>
    /// Represents the Pawn FlyweightPiece contained in FlyweightPieceFactory.
    /// </summary>
    class Pawn : IFlyweightPiece
	{
        /// <summary>
        /// The color, white or black, this FlyweightPiece represents and is able to generate moves for.
        /// </summary>
        private PieceColor m_color;

        /// <summary>
        /// Iterator used to iterate over the board when generating moves.
        /// </summary>
        private BoardIterator m_iterator;

        /// <summary>
        /// The directions m_iterator iterates when generating moves.
        /// </summary>
        private Direction[] m_directions = { Direction.NoDirection, Direction.NoDirection, Direction.NoDirection };


        /// <summary>
        /// Initializes a new instance of the Pawn class.
        /// </summary>
        /// <param name="color">Color, white or black, the piece represents.</param>
        public Pawn(PieceColor color)
		{
			m_color = color;
			m_iterator = new BoardIterator(null, Square.None, Direction.NoDirection);

			if (color == PieceColor.White)
			{
				m_directions[0] = Direction.Up;
				m_directions[1] = Direction.UpLeft;
				m_directions[2] = Direction.UpRight;
			}

			if (color == PieceColor.Black)
			{
				m_directions[0] = Direction.Down;
				m_directions[1] = Direction.DownLeft;
				m_directions[2] = Direction.DownRight;
			}
		}

        /// <summary>
        /// Verifies if a piece attacks a square. This method does not check if the piece on "from"
        /// on the board is of the same type as this FlyweightPiece represents.
        /// </summary>
        /// <param name="board">Board to check attacking on.</param>
        /// <param name="from">Square on which attacking piece is placed.</param>
        /// <param name="to">The square to check if attacked.</param>
        /// <returns>True if a FlyweightPiece placed on "from" square attacks the "to" square. False otherwise.</returns>
        public bool Attacks(Board board, Square from, Square to)
		{
			if ((m_color == PieceColor.White && Board.Rank(to) == Board.Rank(from) + 1) ||
				(m_color == PieceColor.Black && Board.Rank(to) == Board.Rank(from) - 1))
			{
				if ((Math.Abs(Board.File(to) - Board.File(from)) == 1) &&
					(board.GetPieceColor(to) != m_color))
					return true;
			}

			return false;
		}

        /// <summary>
        /// Generate moves a FlyweightPiece can make.
        /// This method does not varifies if a move puts its own king in check.
        /// This is done in MoveOrganizer when adding a move.
        /// </summary>
        /// <param name="board">Board to generate moves for.</param>
        /// <param name="location">Location of the piece.</param>
        /// <param name="moves">Container class to which all generated moves are added.</param>
        public void GenerateMoves(Board board, Square location, MoveOrganizer moves)
		{
            if (m_color == board.State.ColorToPlay)
            {
                for (int i = 0; i < m_directions.Length; i++)
                {
                    m_iterator.Reset(board, location, m_directions[i]);
                    if (m_iterator.Next())
                    {
                        if (m_iterator.CurrentPiece() == Piece.None &&
                            (m_iterator.CurrentDirection() == Direction.Up || m_iterator.CurrentDirection() == Direction.Down))
                        {
                            //Non hitting moves
                            switch (Board.Rank(m_iterator.CurrentSquare()))
                            {
                                case 0:
                                    //Black non hitting promotion move
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackBishop));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackKnight));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackQueen));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackRook));
                                break;

                                case 7:
                                    //White non hitting promotion move
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteBishop));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteKnight));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteQueen));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteRook));
                                break;

                                default:
                                    //Basic non hitting move
                                    moves.Add(new Move(board, location, m_iterator.CurrentSquare()));
                                break;
                            }

                            if ((Board.Rank(m_iterator.CurrentSquare()) == 2 && m_iterator.CurrentDirection() == Direction.Up) ||
                                (Board.Rank(m_iterator.CurrentSquare()) == 5 && m_iterator.CurrentDirection() == Direction.Down))
                            {   //Two squares forward opening move
                                m_iterator.Next();
                                if (m_iterator.CurrentPiece() == Piece.None)
                                    moves.Add(new Move(board, location, m_iterator.CurrentSquare()));
                            }
                        }
                        else if (m_iterator.CurrentPiece() != Piece.None && m_iterator.CurrentPieceColor() != m_color &&
                                 m_iterator.CurrentDirection() != Direction.Up && m_iterator.CurrentDirection() != Direction.Down)
                        {
                            //Hitting moves
                            switch (Board.Rank(m_iterator.CurrentSquare()))
                            {
                                case 0:
                                    //Black hitting promotion move
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackBishop));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackKnight));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackQueen));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.BlackRook));
                                break;

                                case 7:
                                    //White hitting promotion move
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteBishop));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteKnight));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteQueen));
                                    moves.Add(new PawnPromotionMove(board, location, m_iterator.CurrentSquare(), Piece.WhiteRook));
                                break;

                                default:
                                    //Basic hitting move
                                    moves.Add(new Move(board, location, m_iterator.CurrentSquare()));
                                break;
                            }
                        }
                    }
                }

                //EnPassant moves
                if (board.State.EnPassantTarget != Square.None &&
                    Math.Abs(board.State.EnPassantTarget - location) == 1 &&
                    Board.Rank(board.State.EnPassantTarget) == Board.Rank(location))
                {
                    switch (m_color)
                    {
                        case PieceColor.White:
                            moves.Add(new EnPassantCaptureMove(board, location, Board.Position(Board.File(board.State.EnPassantTarget), Board.Rank(location) + 1)));
                        break;

                        case PieceColor.Black:
                            moves.Add(new EnPassantCaptureMove(board, location, Board.Position(Board.File(board.State.EnPassantTarget), Board.Rank(location) - 1)));
                        break;
                    }
                }
            }
		}
	}
}
