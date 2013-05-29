using System;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Represents the Pawn FlyweightPiece contained in FlyweightPieceFactory.
  /// Used for move generation.
  /// </summary>
  class Pawn : IFlyweightPiece
  {
    /// <summary>
    /// The color, white or black, this FlyweightPiece represents and is able to generate moves for.
    /// </summary>
    private PieceColor m_color;

    /// <summary>
    /// The opposite color of m_color;
    /// </summary>
    private PieceColor m_opponentColor;

    /// <summary>
    /// Iterator used to iterate over the board when generating moves.
    /// </summary>
    private BoardIterator m_iterator;

    /// <summary>
    /// The directions m_iterator iterates when generating moves.
    /// </summary>
    private Direction[] m_directions = { Direction.NoDirection, Direction.NoDirection, Direction.NoDirection };

    /// <summary>
    /// Row where it is allowed to move once more.
    /// </summary>
    int moveAgainRow;


    /// <summary>
    /// Initializes a new instance of the Pawn class.
    /// </summary>
    /// <param name="color">Color, white or black, the piece represents.</param>
    public Pawn(PieceColor color)
    {
      m_iterator = new BoardIterator(null, Square.None, Direction.NoDirection);

      if (color == PieceColor.White)
      {
        m_color = PieceColor.White;
        m_opponentColor = PieceColor.Black;

        m_directions[0] = Direction.Up;
        m_directions[1] = Direction.UpLeft;
        m_directions[2] = Direction.UpRight;

        moveAgainRow = 2;
      }

      if (color == PieceColor.Black)
      {
        m_color = PieceColor.Black;
        m_opponentColor = PieceColor.White;

        m_directions[0] = Direction.Down;
        m_directions[1] = Direction.DownLeft;
        m_directions[2] = Direction.DownRight;

        moveAgainRow = 5;
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
    /// This is done by a move when its carried out.
    /// </summary>
    /// <param name="board">Board to generate moves for.</param>
    /// <param name="location">Location of the piece.</param>
    /// <param name="moves">Container class to which all generated moves are added.</param>
    public void GenerateMoves(Board board, Square location, MoveOrganizer moves)
    {
      if (m_color == board.State.ColorToPlay)
      {
        //Non hitting moves
        m_iterator.Reset(board, location, m_directions[0]);
        if (m_iterator.Next())
        {
          if (m_iterator.CurrentPiece() == Piece.None)
          {
            switch (Board.Rank(m_iterator.CurrentSquare()))
            {
              case 0:
                //Black non hitting promotion move
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackBishop));
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackKnight));
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackQueen));
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackRook));
                break;

              case 7:
                //White non hitting promotion move
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteBishop));
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteKnight));
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteQueen));
                moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteRook));
                break;

              default:
                //Basic non hitting move
                moves.Add(new Move(board, location, m_iterator.CurrentSquare()));
                break;
            }

            //Two squares forward opening move
            if (Board.Rank(m_iterator.CurrentSquare()) == moveAgainRow)
            {
              m_iterator.Next();
              if (m_iterator.CurrentPiece() == Piece.None)
                moves.Add(new Move(board, location, m_iterator.CurrentSquare()));
            }
          }
        }

        //Hitting moves to the left and right
        for (int i = 1; i < m_directions.Length; ++i)
        {
          m_iterator.Reset(board, location, m_directions[i]);
          if (m_iterator.Next())
          {
            if (m_iterator.CurrentPieceColor() == m_opponentColor)
            {
              switch (Board.Rank(m_iterator.CurrentSquare()))
              {
                case 0:
                  //Black hitting promotion move
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackBishop));
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackKnight));
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackQueen));
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.BlackRook));
                  break;

                case 7:
                  //White hitting promotion move
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteBishop));
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteKnight));
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteQueen));
                  moves.Add(new Move(board, location, m_iterator.CurrentSquare(), Piece.WhiteRook));
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
        if (Math.Abs(board.State.EnPassantTarget - location) == 1)
        {
          switch (m_color)
          {
            case PieceColor.White:
              moves.Add(new Move(board, location, Board.Position(Board.File(board.State.EnPassantTarget), Board.Rank(location) + 1), board.State.EnPassantTarget));
              break;

            case PieceColor.Black:
              moves.Add(new Move(board, location, Board.Position(Board.File(board.State.EnPassantTarget), Board.Rank(location) - 1), board.State.EnPassantTarget));
              break;
          }
        }
      }
    }
  }
}
