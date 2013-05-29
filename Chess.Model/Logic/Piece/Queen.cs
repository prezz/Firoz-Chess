using System;


namespace Chess.Model.Logic
{
  /// <summary>
  /// Represents the Queen FlyweightPiece contained in FlyweightPieceFactory.
  /// Used for move generation.
  /// </summary>
  class Queen : IFlyweightPiece
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
    private Direction[] m_directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right,
											                   Direction.DownLeft, Direction.DownRight, Direction.UpLeft, Direction.UpRight };


    /// <summary>
    /// Initializes a new instance of the Queen class.
    /// </summary>
    /// <param name="color">Color, white or black, the piece represents.</param>
    public Queen(PieceColor color)
    {
      m_color = color;
      m_iterator = new BoardIterator(null, Square.None, Direction.NoDirection);
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
      if (Board.Rank(from) == Board.Rank(to) ||
        Board.File(from) == Board.File(to) ||
        Math.Abs(Board.File(from) - Board.File(to)) == Math.Abs(Board.Rank(from) - Board.Rank(to)))
      {
        return board.IsPathClear(from, to);
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
        for (int i = 0; i < m_directions.Length; ++i)
        {
          m_iterator.Reset(board, location, m_directions[i]);
          while (m_iterator.Next())
          {
            if (m_iterator.CurrentPiece() == Piece.None)
            {
              moves.Add(new Move(board, location, m_iterator.CurrentSquare()));
            }
            else
            {
              if (m_iterator.CurrentPieceColor() != m_color)
                moves.Add(new Move(board, location, m_iterator.CurrentSquare()));

              break;
            }
          }
        }
      }
    }
  }
}
