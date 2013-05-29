using System;


namespace Chess.Model.Engine
{
  /// <summary>
  /// Part of the partial class BoardEvaluator.
  /// This part is responsible for evaluating material value.
  /// </summary>
  partial class BoardEvaluator
  {
    /// <summary>
    /// Array of bonus score added to white pawns depending on board location.
    /// </summary>
    private static readonly int[] m_whitePawnSquareBonus = 
        {
	         0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,	0 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	         2 + PAWN_VALUE,  3 + PAWN_VALUE,	 4 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  4 + PAWN_VALUE,	 3 + PAWN_VALUE,	2 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         4 + PAWN_VALUE,  6 + PAWN_VALUE,	12 + PAWN_VALUE, 12 + PAWN_VALUE,	12 + PAWN_VALUE,  4 + PAWN_VALUE,	 6 + PAWN_VALUE,  4 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         4 + PAWN_VALUE,  7 + PAWN_VALUE,	18 + PAWN_VALUE, 25 + PAWN_VALUE,	25 + PAWN_VALUE, 16 + PAWN_VALUE,	 7 + PAWN_VALUE,  4 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         6 + PAWN_VALUE, 11 + PAWN_VALUE,	18 + PAWN_VALUE, 27 + PAWN_VALUE,	27 + PAWN_VALUE, 16 + PAWN_VALUE,	11 + PAWN_VALUE,  6 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        10 + PAWN_VALUE, 15 + PAWN_VALUE,	24 + PAWN_VALUE, 32 + PAWN_VALUE,	32 + PAWN_VALUE, 24 + PAWN_VALUE,	15 + PAWN_VALUE, 10 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        10 + PAWN_VALUE, 15 + PAWN_VALUE,	24 + PAWN_VALUE, 32 + PAWN_VALUE,	32 + PAWN_VALUE, 24 + PAWN_VALUE,	15 + PAWN_VALUE, 10 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black pawns depending on board location.
    /// </summary>
    private static readonly int[] m_blackPawnSquareBonus = 
        {
	         0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,	0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        10 + PAWN_VALUE, 15 + PAWN_VALUE,	24 + PAWN_VALUE, 32 + PAWN_VALUE,	32 + PAWN_VALUE, 24 + PAWN_VALUE,	15 + PAWN_VALUE, 10 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        10 + PAWN_VALUE, 15 + PAWN_VALUE,	24 + PAWN_VALUE, 32 + PAWN_VALUE,	32 + PAWN_VALUE, 24 + PAWN_VALUE,	15 + PAWN_VALUE, 10 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         6 + PAWN_VALUE, 11 + PAWN_VALUE,	18 + PAWN_VALUE, 27 + PAWN_VALUE,	27 + PAWN_VALUE, 16 + PAWN_VALUE,	11 + PAWN_VALUE,  6 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         4 + PAWN_VALUE,  7 + PAWN_VALUE,	18 + PAWN_VALUE, 25 + PAWN_VALUE,	25 + PAWN_VALUE, 16 + PAWN_VALUE,	 7 + PAWN_VALUE,	4 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         4 + PAWN_VALUE,  6 + PAWN_VALUE,	12 + PAWN_VALUE, 12 + PAWN_VALUE,	12 + PAWN_VALUE,  4 + PAWN_VALUE,	 6 + PAWN_VALUE,	4 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
           2 + PAWN_VALUE,  3 + PAWN_VALUE,	 4 + PAWN_VALUE,	0 + PAWN_VALUE,	 0 + PAWN_VALUE,  4 + PAWN_VALUE,	 3 + PAWN_VALUE,	2 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,	0 + PAWN_VALUE,	 0 + PAWN_VALUE,  0 + PAWN_VALUE,	 0 + PAWN_VALUE,	0 + PAWN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to white knights depending on board location.
    /// </summary>
    private static readonly int[] m_whiteKnightSquareBonus = 
        {
	        -7 + KNIGHT_VALUE, -3 + KNIGHT_VALUE,	 1 + KNIGHT_VALUE,  3 + KNIGHT_VALUE,	 3 + KNIGHT_VALUE,  1 + KNIGHT_VALUE, -3 + KNIGHT_VALUE, -7 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	         2 + KNIGHT_VALUE,	6 + KNIGHT_VALUE,	14 + KNIGHT_VALUE, 20 + KNIGHT_VALUE,	20 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	 6 + KNIGHT_VALUE,  2 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         6 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	22 + KNIGHT_VALUE, 26 + KNIGHT_VALUE,	26 + KNIGHT_VALUE, 22 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,  6 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         8 + KNIGHT_VALUE, 18 + KNIGHT_VALUE,	26 + KNIGHT_VALUE, 30 + KNIGHT_VALUE,	30 + KNIGHT_VALUE, 26 + KNIGHT_VALUE, 18 + KNIGHT_VALUE,  8 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         8 + KNIGHT_VALUE, 18 + KNIGHT_VALUE,	30 + KNIGHT_VALUE, 32 + KNIGHT_VALUE,	32 + KNIGHT_VALUE, 30 + KNIGHT_VALUE, 18 + KNIGHT_VALUE,  8 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         6 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	28 + KNIGHT_VALUE, 32 + KNIGHT_VALUE,	32 + KNIGHT_VALUE, 28 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,  6 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         2 + KNIGHT_VALUE,	6 + KNIGHT_VALUE,	14 + KNIGHT_VALUE, 20 + KNIGHT_VALUE,	20 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	 6 + KNIGHT_VALUE,  2 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -7 + KNIGHT_VALUE, -3 + KNIGHT_VALUE,	 1 + KNIGHT_VALUE,  3 + KNIGHT_VALUE,	 3 + KNIGHT_VALUE,  1 + KNIGHT_VALUE, -3 + KNIGHT_VALUE, -7 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black knights depending on board location.
    /// </summary>
    private static readonly int[] m_blackKnightSquareBonus = 
        {
	        -7 + KNIGHT_VALUE, -3 + KNIGHT_VALUE,	 1 + KNIGHT_VALUE,	3 + KNIGHT_VALUE,	 3 + KNIGHT_VALUE,	1 + KNIGHT_VALUE,	-3 + KNIGHT_VALUE, -7 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	         2 + KNIGHT_VALUE,	6 + KNIGHT_VALUE,	14 + KNIGHT_VALUE, 20 + KNIGHT_VALUE,	20 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	 6 + KNIGHT_VALUE,	2 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         6 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	28 + KNIGHT_VALUE, 32 + KNIGHT_VALUE,	32 + KNIGHT_VALUE, 28 + KNIGHT_VALUE,	14 + KNIGHT_VALUE,	6 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         8 + KNIGHT_VALUE, 18 + KNIGHT_VALUE,	30 + KNIGHT_VALUE, 32 + KNIGHT_VALUE,	32 + KNIGHT_VALUE, 30 + KNIGHT_VALUE,	18 + KNIGHT_VALUE,	8 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         8 + KNIGHT_VALUE, 18 + KNIGHT_VALUE,	26 + KNIGHT_VALUE, 30 + KNIGHT_VALUE,	30 + KNIGHT_VALUE, 26 + KNIGHT_VALUE,	18 + KNIGHT_VALUE,	8 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         6 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	22 + KNIGHT_VALUE, 26 + KNIGHT_VALUE,	26 + KNIGHT_VALUE, 22 + KNIGHT_VALUE,	14 + KNIGHT_VALUE,	6 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
           2 + KNIGHT_VALUE,	6 + KNIGHT_VALUE,	14 + KNIGHT_VALUE, 20 + KNIGHT_VALUE,	20 + KNIGHT_VALUE, 14 + KNIGHT_VALUE,	 6 + KNIGHT_VALUE,	2 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -7 + KNIGHT_VALUE, -3 + KNIGHT_VALUE,	 1 + KNIGHT_VALUE,	3 + KNIGHT_VALUE,	 3 + KNIGHT_VALUE,	1 + KNIGHT_VALUE,	-3 + KNIGHT_VALUE, -7 + KNIGHT_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to white bishops depending on board location.
    /// </summary>
    private static readonly int[] m_whiteBishopSquareBonus = 
        {
	        16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        26 + BISHOP_VALUE, 29 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	29 + BISHOP_VALUE, 26 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        26 + BISHOP_VALUE, 28 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	28 + BISHOP_VALUE, 26 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 26 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	26 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 26 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	26 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 28 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	28 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 29 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	29 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black bishops depending on board location.
    /// </summary>
    private static readonly int[] m_blackBishopSquareBonus = 
        {
	        16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        16 + BISHOP_VALUE, 29 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	29 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 28 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	28 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 26 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	26 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 26 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	26 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        26 + BISHOP_VALUE, 28 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	32 + BISHOP_VALUE, 32 + BISHOP_VALUE,	28 + BISHOP_VALUE, 26 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
          26 + BISHOP_VALUE, 29 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	31 + BISHOP_VALUE, 31 + BISHOP_VALUE,	29 + BISHOP_VALUE, 26 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE,	16 + BISHOP_VALUE, 16 + BISHOP_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to white rooks depending on board location.
    /// </summary>
    private static readonly int[] m_whiteRookSquareBonus =
        {
	         0 + ROOK_VALUE,  0 + ROOK_VALUE,	 0 + ROOK_VALUE,	3 + ROOK_VALUE,	 3 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        -2 + ROOK_VALUE,  0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,  0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,  0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,  0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,  0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        10 + ROOK_VALUE, 10 + ROOK_VALUE,	10 + ROOK_VALUE, 10 + ROOK_VALUE,	10 + ROOK_VALUE, 10 + ROOK_VALUE,	10 + ROOK_VALUE, 10 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black rooks depending on board location.
    /// </summary>
    private static readonly int[] m_blackRookSquareBonus =
        {
	         0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,  0 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        10 + ROOK_VALUE, 10 + ROOK_VALUE,	10 + ROOK_VALUE, 10 + ROOK_VALUE,	10 + ROOK_VALUE, 10 + ROOK_VALUE,	10 + ROOK_VALUE, 10 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
          -2 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE, -2 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	3 + ROOK_VALUE,	 3 + ROOK_VALUE,	0 + ROOK_VALUE,	 0 + ROOK_VALUE,	0 + ROOK_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to white queens depending on board location.
    /// </summary>
    private static readonly int[] m_whiteQueenSquareBonus =
        {
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	-2 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	         0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 1 + QUEEN_VALUE,	1 + QUEEN_VALUE, 1 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 1 + QUEEN_VALUE,	 1 + QUEEN_VALUE,	1 + QUEEN_VALUE, 1 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	2 + QUEEN_VALUE, 2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	2 + QUEEN_VALUE, 2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
          -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black queens depending on board location.
    /// </summary>
    private static readonly int[] m_blackQueenSquareBonus =
        {
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	2 + QUEEN_VALUE, 2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	2 + QUEEN_VALUE, 2 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 1 + QUEEN_VALUE,	 1 + QUEEN_VALUE,	1 + QUEEN_VALUE, 1 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 1 + QUEEN_VALUE,	1 + QUEEN_VALUE, 1 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE,	 0 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	0 + QUEEN_VALUE, 0 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	-2 + QUEEN_VALUE,	-2 + QUEEN_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to white king if not in endgame situation.
    /// </summary>
    private static readonly int[] m_whiteKingSquareBonus = 
        {
	          3 + KING_VALUE,	  3 + KING_VALUE,	  8 + KING_VALUE,	-12 + KING_VALUE,  -8 + KING_VALUE,	-12 + KING_VALUE,  10 + KING_VALUE,	  5 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1	
	         -5 + KING_VALUE,	 -5 + KING_VALUE,	-12 + KING_VALUE, -12 + KING_VALUE, -12 + KING_VALUE, -12 + KING_VALUE,  -5 + KING_VALUE,	 -5 + KING_VALUE,	0, 0, 0, 0, 0, 0, 0, 0,
	         -7 + KING_VALUE,	-15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE,  -7 + KING_VALUE,	0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black king if not in endgame situation.
    /// </summary>
    private static readonly int[] m_blackKingSquareBonus = 
        {
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, -20 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	         -7 + KING_VALUE,	-15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE, -15 + KING_VALUE,  -7 + KING_VALUE,	0, 0, 0, 0, 0, 0, 0, 0,
	         -5 + KING_VALUE,	 -5 + KING_VALUE,	-12 + KING_VALUE, -12 + KING_VALUE, -12 + KING_VALUE, -12 + KING_VALUE,  -5 + KING_VALUE,	 -5 + KING_VALUE,	0, 0, 0, 0, 0, 0, 0, 0,
            3 + KING_VALUE,	  3 + KING_VALUE,	  8 + KING_VALUE,	-12 + KING_VALUE,  -8 + KING_VALUE,	-12 + KING_VALUE,  10 + KING_VALUE,	  5 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8	
        };

    /// <summary>
    /// Array of bonus score added to white king if in endgame situation.
    /// </summary>
    private static readonly int[] m_whiteKingEndSquareBonus = 
        {
	        0 + KING_VALUE,	0 + KING_VALUE,	 1 + KING_VALUE,  2 + KING_VALUE,	 2 + KING_VALUE,  1 + KING_VALUE,	0 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        0 + KING_VALUE,	2 + KING_VALUE,	 4 + KING_VALUE,  5 + KING_VALUE,	 5 + KING_VALUE,  4 + KING_VALUE,	2 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        1 + KING_VALUE,	4 + KING_VALUE,	 6 + KING_VALUE,  7 + KING_VALUE,	 7 + KING_VALUE,  6 + KING_VALUE,	4 + KING_VALUE, 1 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        1 + KING_VALUE,	4 + KING_VALUE,	10 + KING_VALUE, 10 + KING_VALUE,	10 + KING_VALUE, 10 + KING_VALUE,	4 + KING_VALUE, 1 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        1 + KING_VALUE,	4 + KING_VALUE,	12 + KING_VALUE, 15 + KING_VALUE,	15 + KING_VALUE, 12 + KING_VALUE,	4 + KING_VALUE, 1 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        0 + KING_VALUE,	7 + KING_VALUE,	10 + KING_VALUE, 12 + KING_VALUE,	12 + KING_VALUE, 10 + KING_VALUE,	7 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        0 + KING_VALUE,	2 + KING_VALUE,	 4 + KING_VALUE,  5 + KING_VALUE,	 5 + KING_VALUE,  4 + KING_VALUE,	2 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        0 + KING_VALUE,	0 + KING_VALUE,	 0 + KING_VALUE,	0 + KING_VALUE,	 0 + KING_VALUE,  0 + KING_VALUE,	0 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// Array of bonus score added to black king if in endgame situation.
    /// </summary>
    private static readonly int[] m_blackKingEndSquareBonus = 
        {
	        0 + KING_VALUE,	0 + KING_VALUE,	 0 + KING_VALUE,	0 + KING_VALUE,	 0 + KING_VALUE,  0 + KING_VALUE,	0 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
	        0 + KING_VALUE,	2 + KING_VALUE,	 4 + KING_VALUE,	5 + KING_VALUE,	 5 + KING_VALUE,  4 + KING_VALUE,	2 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        0 + KING_VALUE,	7 + KING_VALUE,	10 + KING_VALUE, 12 + KING_VALUE,	12 + KING_VALUE, 10 + KING_VALUE,	7 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        1 + KING_VALUE,	4 + KING_VALUE,	12 + KING_VALUE, 15 + KING_VALUE,	15 + KING_VALUE, 12 + KING_VALUE,	4 + KING_VALUE, 1 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        1 + KING_VALUE,	4 + KING_VALUE,	10 + KING_VALUE, 10 + KING_VALUE,	10 + KING_VALUE, 10 + KING_VALUE,	4 + KING_VALUE, 1 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        1 + KING_VALUE,	4 + KING_VALUE,	 6 + KING_VALUE,  7 + KING_VALUE,	 7 + KING_VALUE,  6 + KING_VALUE,	4 + KING_VALUE, 1 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
	        0 + KING_VALUE,	2 + KING_VALUE,	 4 + KING_VALUE,  5 + KING_VALUE,	 5 + KING_VALUE,  4 + KING_VALUE,	2 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0,
          0 + KING_VALUE,	0 + KING_VALUE,	 1 + KING_VALUE,  2 + KING_VALUE,	 2 + KING_VALUE,  1 + KING_VALUE,	0 + KING_VALUE, 0 + KING_VALUE, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
        };

    /// <summary>
    /// Array with zero value to avoid index out of range problems.
    /// </summary>
    private static readonly int[] m_zeroBonus = 
        {
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //A1..H1
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	        0, 0,	0, 0,	0, 0,	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //A8..H8
        };

    /// <summary>
    /// 
    /// </summary>
    private static readonly int[][] m_PieceSquareValueTable =
		{
			m_whitePawnSquareBonus,
			m_blackPawnSquareBonus,
			m_whiteKnightSquareBonus,
			m_blackKnightSquareBonus,
			m_whiteBishopSquareBonus,
			m_blackBishopSquareBonus,
			m_whiteRookSquareBonus,
			m_blackRookSquareBonus,
			m_whiteQueenSquareBonus,
			m_blackQueenSquareBonus,
			m_zeroBonus,
			m_zeroBonus,
		};

    /// <summary>
    /// Evaluates the value of a piece (exluding king) on the board also taking it's location into account.
    /// </summary>
    /// <param name="square">Square to evaluate.</param>
    /// <returns>Value of that square.</returns>
    private int EvaluateBasicPiece(Square square)
    {
      return m_PieceSquareValueTable[(int)m_board[square]][(int)square];
    }

    /// <summary>
    /// Evaluates value of the king depending on location and if endgame or not.
    /// </summary>
    /// <param name="square">Location of king.</param>
    /// <param name="materialValue">Current material value.</param>
    /// <returns>Value of the king.</returns>
    private int EvaluateKingPiece(Square square, int materialValue)
    {
      switch (m_board[square])
      {
        case Piece.WhiteKing:
          return KING_VALUE + ((materialValue > (QUEEN_VALUE + KNIGHT_VALUE)) ? m_whiteKingSquareBonus[(int)square] : m_whiteKingEndSquareBonus[(int)square]);

        case Piece.BlackKing:
          return KING_VALUE + ((materialValue > (QUEEN_VALUE + KNIGHT_VALUE)) ? m_blackKingSquareBonus[(int)square] : m_blackKingEndSquareBonus[(int)square]);

        default:
          return 0;
      }
    }

    /// <summary>
    /// Evaluates the material value of all pieces on the board for each side and adds it to the
    /// member variables m_whiteScore and m_blackScore. The material value is the sum of all
    /// piece values added with a bonus depending on board location.
    /// </summary>
    private void EvaluateMaterial()
    {
      foreach (Square square in m_whiteLocations)
        m_whiteScore += EvaluateBasicPiece(square);

      foreach (Square square in m_blackLocations)
        m_blackScore += EvaluateBasicPiece(square);

      m_whiteScore += EvaluateKingPiece(m_board.WhiteKingLocation, m_whiteScore);
      m_blackScore += EvaluateKingPiece(m_board.BlackKingLocation, m_blackScore);
    }
  }
}
