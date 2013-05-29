using System;
using System.Collections.Generic;


namespace Chess.Model.Logic
{
  #region BoardState

  /// <summary>
  /// Used to keep track of the state of the board.
  /// </summary>
  struct BoardState
  {
    /// <summary>
    /// Color to play the next move.
    /// </summary>
    public PieceColor ColorToPlay;

    /// <summary>
    /// True if white still is able to castle short. False otherwise.
    /// </summary>
    public bool WhiteCanCastleShort;

    /// <summary>
    /// True if white still is able to castle long. False otherwise.
    /// </summary>
    public bool WhiteCanCastleLong;

    /// <summary>
    /// True if black still is able to castle short. False otherwise.
    /// </summary>
    public bool BlackCanCastleShort;

    /// <summary>
    /// True if black still is able to castle long. False otherwise.
    /// </summary>
    public bool BlackCanCastleLong;

    /// <summary>
    /// Square on the board where a pawn that can be captured EnPassant is plased.
    /// </summary>
    public Square EnPassantTarget;

    /// <summary>
    /// The amount of moves made in a row with non pawn pieces and no captures.
    /// Used to keep track of fifty move rule.
    /// </summary>
    public int NonHitAndPawnMovesPlayed;
  }

  #endregion


  /// <summary>
  /// Represents a chess board.
  /// </summary>
  class Board
  {
    #region BoardEnumerator

    /// <summary>
    /// Enumerator used to iterate over board.
    /// </summary>
    public class BoardEnumerator
    {
      /// <summary>
      /// Current position of the enumerator.
      /// </summary>
      private Square m_position;

      /// <summary>
      /// Initializes a new instance of the BoardEnumerator class.
      /// </summary>
      public BoardEnumerator()
      {
        m_position = Square.A1 - 1;
      }

      /// <summary>
      /// Iterates one step forward to the next square.
      /// </summary>
      /// <returns>True if if the new square is a valid and existing square. False otherwise.</returns>
      public bool MoveNext()
      {
        ++m_position;
        if (((int)m_position & 0xFFFFFF88) != 0)
          m_position += 8;

        return m_position <= Square.H8;
      }

      /// <summary>
      /// Returns the current square.
      /// </summary>
      public Square Current
      {
        get { return m_position; }
      }
    }

    #endregion

    /// <summary>
    /// Number of squares on the board.
    /// </summary>
    public const int NOF_SQUARS = 128;

    /// <summary>
    /// Array where each index represents a board square.
    /// </summary>
    private Piece[] m_squares;

    /// <summary>
    /// Current state of the board.
    /// </summary>
    private BoardState m_state;

    /// <summary>
    /// Holds a history of the frequency of a board.
    /// </summary>
    private HashHistory m_history;

    /// <summary>
    /// Reference to the FlyweightPieceFactory with piece classes used to move generation.
    /// </summary>
    private FlyweightPieceFactory m_pieceFactory;

    /// <summary>
    /// Holds a list of exact piece locations for white. Used to speed up board traversal.
    /// </summary>
    private PieceLocationManager m_whiteLocationList;

    /// <summary>
    /// Holds a list of exact piece locations for black. Used to speed up board traversal.
    /// </summary>
    private PieceLocationManager m_blackLocationList;

    /// <summary>
    /// Hold exact location of white king
    /// </summary>
    private Square m_whiteKingLocation;

    /// <summary>
    /// Holds exact location of black king
    /// </summary>
    private Square m_blackKingLocation;

    /// <summary>
    /// Current hash value of the board.
    /// </summary>
    private ZobristHash m_boardHash;


    /// <summary>
    /// Initializes a new instance of Board ready to be played on
    /// and with pieces put on their initial locations.
    /// </summary>
    public Board()
    {
      //we make board twice as big as we use the 0x88 scheme for representing the board
      m_squares = new Piece[NOF_SQUARS];
      for (int i = 0; i < m_squares.Length; ++i)
        m_squares[i] = Piece.None;

      m_squares[(int)Square.A1] = Piece.WhiteRook;
      m_squares[(int)Square.B1] = Piece.WhiteKnight;
      m_squares[(int)Square.C1] = Piece.WhiteBishop;
      m_squares[(int)Square.D1] = Piece.WhiteQueen;
      m_squares[(int)Square.E1] = Piece.WhiteKing;
      m_squares[(int)Square.F1] = Piece.WhiteBishop;
      m_squares[(int)Square.G1] = Piece.WhiteKnight;
      m_squares[(int)Square.H1] = Piece.WhiteRook;
      m_squares[(int)Square.A2] = Piece.WhitePawn;
      m_squares[(int)Square.B2] = Piece.WhitePawn;
      m_squares[(int)Square.C2] = Piece.WhitePawn;
      m_squares[(int)Square.D2] = Piece.WhitePawn;
      m_squares[(int)Square.E2] = Piece.WhitePawn;
      m_squares[(int)Square.F2] = Piece.WhitePawn;
      m_squares[(int)Square.G2] = Piece.WhitePawn;
      m_squares[(int)Square.H2] = Piece.WhitePawn;
      m_squares[(int)Square.A7] = Piece.BlackPawn;
      m_squares[(int)Square.B7] = Piece.BlackPawn;
      m_squares[(int)Square.C7] = Piece.BlackPawn;
      m_squares[(int)Square.D7] = Piece.BlackPawn;
      m_squares[(int)Square.E7] = Piece.BlackPawn;
      m_squares[(int)Square.F7] = Piece.BlackPawn;
      m_squares[(int)Square.G7] = Piece.BlackPawn;
      m_squares[(int)Square.H7] = Piece.BlackPawn;
      m_squares[(int)Square.A8] = Piece.BlackRook;
      m_squares[(int)Square.B8] = Piece.BlackKnight;
      m_squares[(int)Square.C8] = Piece.BlackBishop;
      m_squares[(int)Square.D8] = Piece.BlackQueen;
      m_squares[(int)Square.E8] = Piece.BlackKing;
      m_squares[(int)Square.F8] = Piece.BlackBishop;
      m_squares[(int)Square.G8] = Piece.BlackKnight;
      m_squares[(int)Square.H8] = Piece.BlackRook;

      m_state.ColorToPlay = PieceColor.White;
      m_state.WhiteCanCastleShort = true;
      m_state.WhiteCanCastleLong = true;
      m_state.BlackCanCastleShort = true;
      m_state.BlackCanCastleLong = true;
      m_state.EnPassantTarget = Square.None;
      m_state.NonHitAndPawnMovesPlayed = 0;

      m_history = new HashHistory();
      m_pieceFactory = FlyweightPieceFactory.Instance();

      m_whiteLocationList = new PieceLocationManager();
      m_blackLocationList = new PieceLocationManager();
      foreach (Square square in this)
      {
        if (GetPieceColor(square) == PieceColor.White)
          m_whiteLocationList.PlacePiece(square);

        if (GetPieceColor(square) == PieceColor.Black)
          m_blackLocationList.PlacePiece(square);
      }
      m_whiteKingLocation = Square.E1;
      m_blackKingLocation = Square.E8;

      m_boardHash = new ZobristHash(this);
      AddToHistory();
    }

    public Board(Board board)
    {
      m_squares = new Piece[board.m_squares.Length];
      for (int i = 0; i < m_squares.Length; ++i)
        m_squares[i] = board.m_squares[i];

      m_state = board.m_state;

      m_history = new HashHistory(board.m_history);
      m_pieceFactory = FlyweightPieceFactory.Instance();
      m_whiteLocationList = new PieceLocationManager(board.m_whiteLocationList);
      m_blackLocationList = new PieceLocationManager(board.BlackPieceLocations);
      m_whiteKingLocation = board.m_whiteKingLocation;
      m_blackKingLocation = board.m_blackKingLocation;
      m_boardHash = new ZobristHash(board.m_boardHash);
    }

    /// <summary>
    /// Gets the piece on a board square.
    /// </summary>
    /// <param name="square">The square to get piece for.</param>
    /// <returns>The piece on the square.</returns>
    public Piece this[Square square]
    {
      get { return m_squares[(int)square]; }
    }

    /// <summary>
    /// Returns the current state of the board.
    /// </summary>
    public BoardState State
    {
      get { return m_state; }

      set
      {
        if (m_state.ColorToPlay != value.ColorToPlay)
          m_boardHash.HashFlipColorToPlay();

        if (m_state.WhiteCanCastleLong != value.WhiteCanCastleLong)
          m_boardHash.HashFlipWhiteCanCastleLong();

        if (m_state.WhiteCanCastleShort != value.WhiteCanCastleShort)
          m_boardHash.HashFlipWhiteCanCastleShort();

        if (m_state.BlackCanCastleLong != value.BlackCanCastleLong)
          m_boardHash.HashFlipBlackCanCastleLong();

        if (m_state.BlackCanCastleShort != value.BlackCanCastleShort)
          m_boardHash.HashFlipBlackCanCastleShort();

        if (m_state.EnPassantTarget != value.EnPassantTarget)
        {
          if (m_state.EnPassantTarget != Square.None)
            m_boardHash.HashFlipEnPassantTarget(m_state.EnPassantTarget);

          if (value.EnPassantTarget != Square.None)
            m_boardHash.HashFlipEnPassantTarget(value.EnPassantTarget);
        }

        m_state = value;
      }
    }

    /// <summary>
    /// Returns a list holding the location of all white pieces on the board.
    /// </summary>
    public PieceLocationManager WhitePieceLocations
    {
      get { return m_whiteLocationList; }
    }

    /// <summary>
    /// Returns a list holding the location of all black pieces on the board.
    /// </summary>
    public PieceLocationManager BlackPieceLocations
    {
      get { return m_blackLocationList; }
    }

    /// <summary>
    /// Gets the exact position of the white king
    /// </summary>
    public Square WhiteKingLocation
    {
      get
      {
        if (m_squares[(int)m_whiteKingLocation] != Piece.WhiteKing)
        {
          foreach (Square square in m_whiteLocationList)
          {
            if (m_squares[(int)square] == Piece.WhiteKing)
            {
              m_whiteKingLocation = square;
              break;
            }
          }
        }

        return m_whiteKingLocation;
      }
    }

    /// <summary>
    /// Gets the exact position of the black king
    /// </summary>
    public Square BlackKingLocation
    {
      get
      {
        if (m_squares[(int)m_blackKingLocation] != Piece.BlackKing)
        {
          foreach (Square square in m_blackLocationList)
          {
            if (m_squares[(int)square] == Piece.BlackKing)
            {
              m_blackKingLocation = square;
              break;
            }
          }
        }

        return m_blackKingLocation;
      }
    }

    /// <summary>
    /// Sets the piece on a board square. Don't call this function to update bord when moving a piece,
    /// only call it to place one piece directly on board.
    /// </summary>
    /// <param name="square">The square to place a piece.</param>
    /// <param name="piece">The piece to place. To remove a piece use "Piece.None".</param>
    public void PlacePiece(Square square, Piece piece)
    {
      //Hash out current piece on square
      if (m_squares[(int)square] != Piece.None)
        m_boardHash.HashFlipPieceSquare(m_squares[(int)square], square);

      //Hash in new piece
      if (piece != Piece.None)
        m_boardHash.HashFlipPieceSquare(piece, square);

      //Update quick indexes
      switch (GetPieceColor(piece))
      {
        case PieceColor.White:
          m_whiteLocationList.PlacePiece(square);
          m_blackLocationList.RemovePiece(square);
          break;

        case PieceColor.Black:
          m_blackLocationList.PlacePiece(square);
          m_whiteLocationList.RemovePiece(square);
          break;

        case PieceColor.None:
          m_whiteLocationList.RemovePiece(square);
          m_blackLocationList.RemovePiece(square);
          break;
      }

      //Place piece on board
      m_squares[(int)square] = piece;

      //and update king location
      if (piece == Piece.WhiteKing)
        m_whiteKingLocation = square;

      if (piece == Piece.BlackKing)
        m_blackKingLocation = square;
    }

    /// <summary>
    /// Performs a move on the board.
    /// </summary>
    /// <param name="from">Square to move piece from.</param>
    /// <param name="to">Square to move piece to.</param>
    public void MovePiece(Square from, Square to)
    {
      //Hash out current piece at destination (if any).
      if (m_squares[(int)to] != Piece.None)
        m_boardHash.HashFlipPieceSquare(m_squares[(int)to], to);

      //Hash out piece at current location
      if (m_squares[(int)from] != Piece.None)
        m_boardHash.HashFlipPieceSquare(m_squares[(int)from], from);

      //Update quick indexes
      switch (GetPieceColor(from))
      {
        case PieceColor.White:
          m_whiteLocationList.MovePiece(from, to);
          m_blackLocationList.RemovePiece(to);
          break;

        case PieceColor.Black:
          m_blackLocationList.MovePiece(from, to);
          m_whiteLocationList.RemovePiece(to);
          break;

        case PieceColor.None:
          m_whiteLocationList.RemovePiece(to);
          m_blackLocationList.RemovePiece(to);
          break;
      }

      //Perform the move
      m_squares[(int)to] = m_squares[(int)from];
      m_squares[(int)from] = Piece.None;

      //Hash in piece at new location
      m_boardHash.HashFlipPieceSquare(m_squares[(int)to], to);

      //and update king location
      if (m_squares[(int)to] == Piece.WhiteKing)
        m_whiteKingLocation = to;

      if (m_squares[(int)to] == Piece.BlackKing)
        m_blackKingLocation = to;
    }

    /// <summary>
    /// Adds the current board layout to the history of boards that has been played.
    /// </summary>
    public void AddToHistory()
    {
      m_history.AddBoard(this);
    }

    /// <summary>
    /// Removes the current board layout from the history of boards that has been played.
    /// </summary>
    public void RemoveFromHistory()
    {
      m_history.RemoveBoard(this);
    }

    /// <summary>
    /// Returns the amount of times the current board layout has been represented on the board. 
    /// </summary>
    /// <returns>Amount of times board layout has been represented.</returns>
    public int BoardHistoryFrequency()
    {
      return m_history.BoardFrequency(this);
    }

    /// <summary>
    /// Returns the current hash value for the board.
    /// </summary>
    /// <param name="asNewInstance">
    /// True if ZobristHash is a new instance, typically used if hash is to be used as key in a hash table.
    /// False if the returned instance is the same as the one internally used within "Board".
    /// </param>
    public ZobristHash BoardHash(bool asNewInstance)
    {
      if (asNewInstance)
        return new ZobristHash(m_boardHash);
      else
        return m_boardHash;
    }

    /// <summary>
    /// Returns an enumerator that iterates through all squares on the board.
    /// </summary>
    /// <returns>Returns a BoardEnumerator</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    public BoardEnumerator GetEnumerator()
    {
      return new BoardEnumerator();
    }

    /// <summary>
    /// Returns the color of a piece located on a square.
    /// </summary>
    /// <param name="square">The square to return piece color for.</param>
    /// <returns>Color of piece, white or black. If no piece is located on square "PieceColor.NoColor" is returned</returns>
    public PieceColor GetPieceColor(Square square)
    {
      return GetPieceColor(this[square]);
    }

    /// <summary>
    /// Verifies whether a given square is attacked.
    /// </summary>
    /// <param name="attacker">The player that potenially is attacking.</param>
    /// <param name="target">The square to check if attacked.</param>
    /// <returns>True if target is attacked by player.</returns>
    public bool IsSquareAttacted(PieceColor attacker, Square target)
    {
      switch (attacker)
      {
        case PieceColor.White:
          foreach (Square square in m_whiteLocationList)
            if (m_pieceFactory.GetFlyweightPiece(this[square]).Attacks(this, square, target))
              return true;
          break;

        case PieceColor.Black:
          foreach (Square square in m_blackLocationList)
            if (m_pieceFactory.GetFlyweightPiece(this[square]).Attacks(this, square, target))
              return true;
          break;
      }

      return false;
    }

    /// <summary>
    /// Verifies if a player is check.
    /// </summary>
    /// <param name="color">The player to verify if is in check.</param>
    /// <returns>True if player is check. False otherwise.</returns>
    public bool IsCheck(PieceColor color)
    {
      bool result = false;

      switch (color)
      {
        case PieceColor.White:
          return IsSquareAttacted(PieceColor.Black, WhiteKingLocation);

        case PieceColor.Black:
          return IsSquareAttacted(PieceColor.White, BlackKingLocation);
      }

      return result;
    }

    /// <summary>
    /// Verifies if current player is checked py opponent.
    /// </summary>
    /// <returns>True if in check. False otherwise.</returns>
    public bool ColorToPlayIsCheck()
    {
      return IsCheck(m_state.ColorToPlay);
    }

    /// <summary>
    /// Verifies whether no pieces occupies the squares between from and to.
    /// Valid directions is up/down, left/right and diagonals.
    /// </summary>
    /// <param name="from">Square to check path from.</param>
    /// <param name="to">Square to check path to</param>
    /// <returns>True if the path is clear. False otherwise.</returns>
    public bool IsPathClear(Square from, Square to)
    {
      int rowDiff = Math.Sign(Rank(to) - Rank(from));
      int fileDiff = Math.Sign(File(to) - File(from));

      int rank = Rank(from) + rowDiff;
      int file = File(from) + fileDiff;
      while (rank != Rank(to) || file != File(to))
      {
        if (this[Position(file, rank)] != Piece.None)
          return false;

        rank += rowDiff;
        file += fileDiff;
      }
      return true;
    }

    /// <summary>
    /// Generates valid moves on piece level for all pieces on the board and adds them to moves.
    /// </summary>
    /// <remarks>
    /// Valid moves on piece level are moves that for the isolated piece is valid. Such a move might
    /// however on board level still be invalid if the move puts its own king in check.
    /// </remarks>
    /// <param name="moves">The MoveOrganizer where generated moves are added.</param>
    public void GeneratePseudoLegalMoves(MoveOrganizer moves)
    {
      switch (State.ColorToPlay)
      {
        case PieceColor.White:
          foreach (Square square in m_whiteLocationList)
          {
            IFlyweightPiece piece = m_pieceFactory.GetFlyweightPiece(this[square]);
            piece.GenerateMoves(this, square, moves);
          }
          break;

        case PieceColor.Black:
          foreach (Square square in m_blackLocationList)
          {
            IFlyweightPiece piece = m_pieceFactory.GetFlyweightPiece(this[square]);
            piece.GenerateMoves(this, square, moves);
          }
          break;
      }
    }

    /// <summary>
    /// Returns the color of a piece.
    /// </summary>
    /// <param name="piece">The piece to return piece color for.</param>
    /// <returns>Color of piece, white, black or none.</returns>
    public static PieceColor GetPieceColor(Piece piece)
    {
      switch (piece)
      {
        case Piece.WhitePawn:
        case Piece.WhiteKnight:
        case Piece.WhiteBishop:
        case Piece.WhiteRook:
        case Piece.WhiteQueen:
        case Piece.WhiteKing:
          return PieceColor.White;

        case Piece.BlackPawn:
        case Piece.BlackKnight:
        case Piece.BlackBishop:
        case Piece.BlackRook:
        case Piece.BlackQueen:
        case Piece.BlackKing:
          return PieceColor.Black;

        default:
          return PieceColor.None;
      }
    }

    /// <summary>
    /// Returns the rank (or row) the square is located on.
    /// </summary>
    /// <param name="square">Valid arguments are all squares except Square.NoSquare.</param>
    /// <returns>The rank [0..7], viewed from white player, where square is located on the board.</returns>
    public static int Rank(Square square)
    {
      return ((int)square) / 16; // ((int)square) >> 4
    }

    /// <summary>
    /// Returns the file (or column) the square is located on.
    /// </summary>
    /// <param name="square">Valid arguments are all squares except Square.NoSquare.</param>
    /// <returns>The rank [0..7], left to right viewed from white player, where square is located on the board.</returns>
    public static int File(Square square)
    {
      return ((int)square) % 16;
    }

    /// <summary>
    /// Converts a rank and file to a square.
    /// </summary>
    /// <param name="file">Valid arguments are [0..7] left to right viewed from white player.</param>
    /// <param name="rank">Valid arguments are [0..7] viewed from white player.</param>
    /// <returns></returns>
    public static Square Position(int file, int rank)
    {
      return (Square)(file + (rank * 16));
    }

    /// <summary>
    /// Verifies the square color on the board.
    /// </summary>
    /// <param name="square">Square to verify. Square.NoSquare is an invalid argument.</param>
    /// <returns>True if the square is white. False otherwise.</returns>
    public static bool IsWhiteSquare(Square square)
    {
      return (((Rank(square) + File(square)) & 1) == 1);
    }
  }
}
