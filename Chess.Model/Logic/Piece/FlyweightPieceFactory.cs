using System;


namespace Chess.Model.Logic
{
  /// <summary>
  /// The Flyweight factory is responsible for creating and managing the flyweight piece objects.
  /// It ensures that FlyweigtPieces are shared properly.
  /// <seealso cref="Chess.Model.Logic.IFlyweightPiece"/>
  /// </summary>
  class FlyweightPieceFactory
  {
    /// <summary>
    /// Holds the reference to the singelton instance of this class.
    /// </summary>
    private static FlyweightPieceFactory m_singelton;

    /// <summary>
    /// Array of the FlyweightPiece instances.
    /// </summary>
    private IFlyweightPiece[] m_pieceFlyweights;


    /// <summary>
    /// Static constructor that insures that only one instance of FlyweightPieceFactory exists.
    /// </summary>
    /// <returns>Returns the singelton instance of the FlyweightPieceFactory.</returns>
    public static FlyweightPieceFactory Instance()
    {
      if (m_singelton == null)
        m_singelton = new FlyweightPieceFactory();

      return m_singelton;
    }

    /// <summary>
    /// Private constructor that initializes a new instance of the FlyweightPieceFactory class.
    /// </summary>
    private FlyweightPieceFactory()
    {
      m_pieceFlyweights = new IFlyweightPiece[13];

      m_pieceFlyweights[(int)Piece.WhitePawn] = new Pawn(PieceColor.White);
      m_pieceFlyweights[(int)Piece.BlackPawn] = new Pawn(PieceColor.Black);
      m_pieceFlyweights[(int)Piece.WhiteKnight] = new Knight(PieceColor.White);
      m_pieceFlyweights[(int)Piece.BlackKnight] = new Knight(PieceColor.Black);
      m_pieceFlyweights[(int)Piece.WhiteBishop] = new Bishop(PieceColor.White);
      m_pieceFlyweights[(int)Piece.BlackBishop] = new Bishop(PieceColor.Black);
      m_pieceFlyweights[(int)Piece.WhiteRook] = new Rook(PieceColor.White);
      m_pieceFlyweights[(int)Piece.BlackRook] = new Rook(PieceColor.Black);
      m_pieceFlyweights[(int)Piece.WhiteQueen] = new Queen(PieceColor.White);
      m_pieceFlyweights[(int)Piece.BlackQueen] = new Queen(PieceColor.Black);
      m_pieceFlyweights[(int)Piece.WhiteKing] = new King(PieceColor.White);
      m_pieceFlyweights[(int)Piece.BlackKing] = new King(PieceColor.Black);
      m_pieceFlyweights[(int)Piece.None] = null;
    }

    /// <summary>
    /// Used to return the FlyweightPiece instance that is related to a piece.
    /// </summary>
    /// <param name="piece">The Piece to obtain the FlyweightPiece instance for.</param>
    /// <returns>The FlyweightPiece associated to "piece".</returns>
    public IFlyweightPiece GetFlyweightPiece(Piece piece)
    {
      return m_pieceFlyweights[(int)piece];
    }
  }
}
