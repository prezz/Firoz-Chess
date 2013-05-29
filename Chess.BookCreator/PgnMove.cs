using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Chess.Model;


namespace Chess.BookCreator
{
  class PgnMove
  {
    private static Regex r = new Regex(@"(?<Move>\d+\.)?(?<Piece>[KQRBNP])?(?<File>[abcdefgh])?(?<Rank>[12345678])?(?<Capture>x)?(?<Destination>[abcdefgh][12345678])(?<Promotion>=[QRBN])?(?<Notation>[+#])?|(?<Move>\d+\.)?(?<Castling>O-O(?<Long>-O)?)");

    public PieceColor ColorToPlay = PieceColor.None;
    public Piece PieceToPlay = Piece.None;
    public int FromFile = -1;
    public int FromRank = -1;
    public Square DestinationSquare = Square.None;
    public PromotionPiece PromotionPiece = PromotionPiece.Queen;

    public PgnMove(string pgnString)
    {
      Match m = r.Match(pgnString);

      if (m.Groups["Move"].Value == "")
      {
        ColorToPlay = PieceColor.Black;
      }
      else
      {
        ColorToPlay = PieceColor.White;
      }

      if (m.Groups["Castling"].Value == "")
      {
        PieceToPlay = StringToPiece(m.Groups["Piece"].Value, ColorToPlay);
        FromFile = StringToFile(m.Groups["File"].Value);
        FromRank = StringToRank(m.Groups["Rank"].Value);
        DestinationSquare = StringToSquare(m.Groups["Destination"].Value);
        PromotionPiece = StringToPromotion(m.Groups["Promotion"].Value);
      }
      else if (m.Groups["Castling"].Value == "O-O")
      {
        if (ColorToPlay == PieceColor.White)
        {
          DestinationSquare = Square.G1;
          PieceToPlay = Piece.WhiteKing;
          FromFile = 4;
          FromRank = 0;
        }

        if (ColorToPlay == PieceColor.Black)
        {
          DestinationSquare = Square.G8;
          PieceToPlay = Piece.BlackKing;
          FromFile = 4;
          FromRank = 7;
        }
      }
      else if (m.Groups["Castling"].Value == "O-O-O")
      {
        if (ColorToPlay == PieceColor.White)
        {
          DestinationSquare = Square.C1;
          PieceToPlay = Piece.WhiteKing;
          FromFile = 4;
          FromRank = 0;
        }

        if (ColorToPlay == PieceColor.Black)
        {
          DestinationSquare = Square.C8;
          PieceToPlay = Piece.BlackKing;
          FromFile = 4;
          FromRank = 7;
        }
      }
    }

    public PromotionPiece StringToPromotion(string pieceString)
    {
      switch (pieceString)
      {
        case "":
        case "=Q":
          return PromotionPiece.Queen;
        case "=R":
          return PromotionPiece.Rook;
        case "=B":
          return PromotionPiece.Bishop;
        case "=N":
          return PromotionPiece.Knight;
      }

      Debug.Fail("Invalid promotion string");
      Console.WriteLine("Invalid promotion string");
      return PromotionPiece.Queen;
    }

    public int StringToRank(string rankString)
    {
      switch (rankString)
      {
        case "":
          return -1;
        case "1":
          return 0;
        case "2":
          return 1;
        case "3":
          return 2;
        case "4":
          return 3;
        case "5":
          return 4;
        case "6":
          return 5;
        case "7":
          return 6;
        case "8":
          return 7;
      }

      Debug.Fail("Invalid rank string");
      Console.WriteLine("Invalid rank string");
      return -1;
    }

    public int StringToFile(string fileString)
    {
      switch (fileString)
      {
        case "":
          return -1;
        case "a":
          return 0;
        case "b":
          return 1;
        case "c":
          return 2;
        case "d":
          return 3;
        case "e":
          return 4;
        case "f":
          return 5;
        case "g":
          return 6;
        case "h":
          return 7;
      }

      Debug.Fail("Invalid file string");
      Console.WriteLine("Invalid file string");
      return -1;
    }

    public Piece StringToPiece(string pieceString, PieceColor color)
    {
      if (color == PieceColor.White)
      {
        switch (pieceString)
        {
          case "K":
            return Piece.WhiteKing;
          case "Q":
            return Piece.WhiteQueen;
          case "R":
            return Piece.WhiteRook;
          case "B":
            return Piece.WhiteBishop;
          case "N":
            return Piece.WhiteKnight;
          case "P":
          case "":
            return Piece.WhitePawn;
        }
      }

      if (color == PieceColor.Black)
      {
        switch (pieceString)
        {
          case "K":
            return Piece.BlackKing;
          case "Q":
            return Piece.BlackQueen;
          case "R":
            return Piece.BlackRook;
          case "B":
            return Piece.BlackBishop;
          case "N":
            return Piece.BlackKnight;
          case "P":
          case "":
            return Piece.BlackPawn;
        }
      }

      Debug.Fail("Invalid piece string");
      Console.WriteLine("Invalid piece string");
      return Piece.None;
    }

    public Square StringToSquare(string squareString)
    {
      switch (squareString)
      {
        case "a1":
          return Square.A1;
        case "a2":
          return Square.A2;
        case "a3":
          return Square.A3;
        case "a4":
          return Square.A4;
        case "a5":
          return Square.A5;
        case "a6":
          return Square.A6;
        case "a7":
          return Square.A7;
        case "a8":
          return Square.A8;

        case "b1":
          return Square.B1;
        case "b2":
          return Square.B2;
        case "b3":
          return Square.B3;
        case "b4":
          return Square.B4;
        case "b5":
          return Square.B5;
        case "b6":
          return Square.B6;
        case "b7":
          return Square.B7;
        case "b8":
          return Square.B8;

        case "c1":
          return Square.C1;
        case "c2":
          return Square.C2;
        case "c3":
          return Square.C3;
        case "c4":
          return Square.C4;
        case "c5":
          return Square.C5;
        case "c6":
          return Square.C6;
        case "c7":
          return Square.C7;
        case "c8":
          return Square.C8;

        case "d1":
          return Square.D1;
        case "d2":
          return Square.D2;
        case "d3":
          return Square.D3;
        case "d4":
          return Square.D4;
        case "d5":
          return Square.D5;
        case "d6":
          return Square.D6;
        case "d7":
          return Square.D7;
        case "d8":
          return Square.D8;

        case "e1":
          return Square.E1;
        case "e2":
          return Square.E2;
        case "e3":
          return Square.E3;
        case "e4":
          return Square.E4;
        case "e5":
          return Square.E5;
        case "e6":
          return Square.E6;
        case "e7":
          return Square.E7;
        case "e8":
          return Square.E8;

        case "f1":
          return Square.F1;
        case "f2":
          return Square.F2;
        case "f3":
          return Square.F3;
        case "f4":
          return Square.F4;
        case "f5":
          return Square.F5;
        case "f6":
          return Square.F6;
        case "f7":
          return Square.F7;
        case "f8":
          return Square.F8;

        case "g1":
          return Square.G1;
        case "g2":
          return Square.G2;
        case "g3":
          return Square.G3;
        case "g4":
          return Square.G4;
        case "g5":
          return Square.G5;
        case "g6":
          return Square.G6;
        case "g7":
          return Square.G7;
        case "g8":
          return Square.G8;

        case "h1":
          return Square.H1;
        case "h2":
          return Square.H2;
        case "h3":
          return Square.H3;
        case "h4":
          return Square.H4;
        case "h5":
          return Square.H5;
        case "h6":
          return Square.H6;
        case "h7":
          return Square.H7;
        case "h8":
          return Square.H8;

        default:
          Debug.Fail("Invalid square string");
          Console.WriteLine("Invalid square string");
          return Square.None;
      }
    }
  }
}
