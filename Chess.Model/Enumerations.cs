using System;


namespace Chess.Model
{
  //Preprocessor directive disabling XML comment warnings from this file.
#pragma warning disable 1591

  /// <summary>
  /// Enumeration of the different chess pieces.
  /// </summary>
  public enum Piece
  {
    WhitePawn,
    BlackPawn,
    WhiteKnight,
    BlackKnight,
    WhiteBishop,
    BlackBishop,
    WhiteRook,
    BlackRook,
    WhiteQueen,
    BlackQueen,
    WhiteKing,
    BlackKing,
    None,
  }

  /// <summary>
  /// Enumeration of the chess board squares. They are not linary numbered but numbered to fulfill
  /// the 0x88 board scheme which allows us to check if a square is valid if the statement
  /// (((int)square & 0xFFFFFF88) == 0) evaluetes to true 
  /// </summary>
  public enum Square
  {
    A1 = 0, B1, C1, D1, E1, F1, G1, H1,
    A2 = 16, B2, C2, D2, E2, F2, G2, H2,
    A3 = 32, B3, C3, D3, E3, F3, G3, H3,
    A4 = 48, B4, C4, D4, E4, F4, G4, H4,
    A5 = 64, B5, C5, D5, E5, F5, G5, H5,
    A6 = 80, B6, C6, D6, E6, F6, G6, H6,
    A7 = 96, B7, C7, D7, E7, F7, G7, H7,
    A8 = 112, B8, C8, D8, E8, F8, G8, H8,
    None = 128,
  }

  /// <summary>
  /// Enumeration of the piece colors. This enum is also used to identify a player.
  /// </summary>
  public enum PieceColor
  {
    Black,
    White,
    None,
  }

  /// <summary>
  /// Enumeration of the pieces a pawn can be promoted to.
  /// </summary>
  public enum PromotionPiece
  {
    Knight,
    Bishop,
    Rook,
    Queen,
  }

  /// <summary>
  /// Enumeration of the game states that needs to be signaled in chess.
  /// </summary>
  public enum GameStatus
  {
    WhiteCheckmateVictory,
    BlackCheckmateVictory,
    WhiteTimeVictory,
    BlackTimeVictory,
    StalemateDraw,
    MoveRepetitionDraw,
    FiftyMovesDraw,
    InsufficientPiecesDraw,
    WhiteIsCheck,
    BlackIsCheck,
    Normal,
  }

  /// <summary>
  /// Enumeration of the chess games clock types.
  /// </summary>
  public enum ClockType
  {
    Conventional,
    Incremental,
    None,
  }
}