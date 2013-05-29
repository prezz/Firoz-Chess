using System;
using Chess.Model.Logic;
using Chess.Model.EngineInterface;


namespace Chess.Model.Engine
{
  /// <summary>
  /// Class responsible for evaluating a board and returning a score for that board.
  /// The board is always evaluated from the current players perspective.
  /// </summary>
  partial class BoardEvaluator : IBoardEvaluator
  {
    /// <summary>
    /// Value of a pawn.
    /// </summary>
    private const int PAWN_VALUE = 100;

    /// <summary>
    /// Value of a knight.
    /// </summary>
    private const int KNIGHT_VALUE = 300;

    /// <summary>
    /// Value of a bishop.
    /// </summary>
    private const int BISHOP_VALUE = 325;

    /// <summary>
    /// Value of a rook.
    /// </summary>
    private const int ROOK_VALUE = 500;

    /// <summary>
    /// Value of a queen.
    /// </summary>
    private const int QUEEN_VALUE = 900;

    /// <summary>
    /// Value of king is defined to zero since it never will be missing on the board.
    /// </summary>
    private const int KING_VALUE = 0;

    /// <summary>
    /// Table holding the value of each piece. To extract
    /// a piece value index the array like m_PieceValueTable[(int)piece]
    /// </summary>
    private static readonly int[] m_PieceValueTable =
		{
			PAWN_VALUE,
			PAWN_VALUE,
			KNIGHT_VALUE,
			KNIGHT_VALUE,
			BISHOP_VALUE,
			BISHOP_VALUE,
			ROOK_VALUE,
			ROOK_VALUE,
			QUEEN_VALUE,
			QUEEN_VALUE,
			KING_VALUE,
			KING_VALUE,
		};

    /// <summary>
    /// Board to evaluate.
    /// </summary>
    private Board m_board;

    /// <summary>
    /// The exact location of the white pieces.
    /// </summary>
    private PieceLocationManager m_whiteLocations;

    /// <summary>
    /// The exact location of the black pieces.
    /// </summary>
    private PieceLocationManager m_blackLocations;

    /// <summary>
    /// The score for white.
    /// </summary>
    private int m_whiteScore;

    /// <summary>
    /// The score for black.
    /// </summary>
    private int m_blackScore;


    /// <summary>
    /// Initializes a new instance of the BoardEvaluator.
    /// </summary>
    public BoardEvaluator()
    {
      //Material();
      //Positions();
      Pawns();
    }

    /// <summary>
    /// Defined alpha value. Must be negative.
    /// </summary>
    public int AlphaValue
    {
      get { return -30000; }
    }

    /// <summary>
    /// Defined mate value. mist be a negative number larger then alpha.
    /// </summary>
    public int MateValue
    {
      get { return -25000; }
    }

    /// <summary>
    /// Returns defined piece value.
    /// </summary>
    /// <param name="piece">Piece to get value for.</param>
    /// <returns>Value of piece.</returns>
    public int PieceValue(Piece piece)
    {
      return m_PieceValueTable[(int)piece];
    }

    /// <summary>
    /// Evaluates a board from the perspective of the current player.
    /// </summary>
    /// <param name="board">Board to evaluate.</param>
    /// <returns>Score for board.</returns>
    public int EvaluatePosition(Board board)
    {
      m_board = board;
      m_whiteLocations = board.WhitePieceLocations;
      m_blackLocations = board.BlackPieceLocations;

      m_whiteScore = 0;
      m_blackScore = 0;

      EvaluateMaterial();
      EvaluatePositions();
      EvaluatePawns();

      switch (m_board.State.ColorToPlay)
      {
        case PieceColor.Black:
          return m_blackScore - m_whiteScore;

        case PieceColor.White:
          return m_whiteScore - m_blackScore;

        default:
          return 0;
      }
    }
  }
}
