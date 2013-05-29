using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Chess.Model;


namespace Chess.BookCreator
{
  class Program
  {
    static PromotionPiece promotion;

    static void Main(string[] args)
    {
      ChessFacade chess = new ChessFacade();

      chess.WhitePawnPromoted += new EventHandler<PromotionEventArgs>(chess_WhitePawnPromoted);
      chess.BlackPawnPromoted += new EventHandler<PromotionEventArgs>(chess_BlackPawnPromoted);
      chess.StatusInfo += new EventHandler<GameStatusEventArgs>(chess_StatusChanged);

      EngineConfiguration engConfig = chess.EngineConfiguration;
      engConfig.EngineAutoPlay = false;
      chess.EngineConfiguration = engConfig;

      ClockConfiguration clockConfig = chess.ClockConfiguration;
      clockConfig.ClockType = ClockType.None;
      chess.ClockConfiguration = clockConfig;

      StreamReader file;
      if (args.Length == 1)
        file = File.OpenText(args[0]);
      else
        file = File.OpenText(@"..\..\input\book_1.00.pgn");

      int lineCounter = 0;
      string line;
      StringBuilder gameString = new StringBuilder();
      while ((line = file.ReadLine()) != null)
      {
        ++lineCounter;
        if (line.Length > 0 && line[0] != '[')
        {
          gameString.Append(line);
          if (line[line.Length - 1] != '.')
            gameString.Append(" ");
        }
        else if (gameString.Length > 0)
        {
          bool breaking = false;
          string[] lineBlocks = gameString.ToString().Split(new char[] { ' ' });

          PieceColor winner = PieceColor.None;
          if (lineBlocks[lineBlocks.Length - 2] == "1-0")
          {
            winner = PieceColor.White;
          }
          else if (lineBlocks[lineBlocks.Length - 2] == "0-1")
          {
            winner = PieceColor.Black;
          }
          else if (lineBlocks[lineBlocks.Length - 2] == "1/2-1/2" || lineBlocks[lineBlocks.Length - 2] == "*")
          {
            winner = PieceColor.None;
          }
          else
          {
            Console.WriteLine("Unknown result");
          }

          int itLength = Math.Min(lineBlocks.Length, 20);
          for (int i = 0; i < itLength; ++i)
          {
            string lineBlock = lineBlocks[i];
            if (lineBlock != "" && lineBlock != "*" && lineBlock != "1-0" && lineBlock != "0-1" && lineBlock != "1/2-1/2")
            {
              PgnMove pgnMove = new PgnMove(lineBlock);

              for (Square square = Square.A1; square <= Square.H8; ++square)
              {
                if ((pgnMove.PieceToPlay == chess.PieceAt(square)) &&
                  (pgnMove.FromFile == -1 || pgnMove.FromFile == ChessFacade.File(square)) &&
                  (pgnMove.FromRank == -1 || pgnMove.FromRank == ChessFacade.Rank(square)))
                {
                  promotion = pgnMove.PromotionPiece;
                  if (chess.PerformMove(square, pgnMove.DestinationSquare))
                  {
                    //Console.Write(square + "-" + pgnMove.DestinationSquare + " | ");
                    if (winner == PieceColor.None)
                      chess.AddToOpeningBook(1);
                    else if (winner == pgnMove.ColorToPlay)
                      chess.AddToOpeningBook(2);
                    //else
                    //	chess.AddToOpeningBook(1);
                    break;
                  }
                }

                if (square == Square.H8)
                {
                  Console.WriteLine("No move performed: " + lineCounter);
                  Console.ReadLine();
                  breaking = true;
                  break;
                }
              }
            }

            if (breaking)
              break;
          }

          //Console.WriteLine("new game");
          gameString = new StringBuilder();
          while (chess.UndoCount > 0)
            chess.UndoMove();
        }

        if (lineCounter % 1000 == 0)
          Console.WriteLine(lineCounter.ToString());
      }

      chess.SaveOpeningBook();
      Console.WriteLine("Done");
      Console.ReadLine();
    }

    static void chess_StatusChanged(object sender, GameStatusEventArgs e)
    {
      if (e.GameStatus != GameStatus.Normal && e.GameStatus != GameStatus.WhiteIsCheck && e.GameStatus != GameStatus.BlackIsCheck)
        Console.WriteLine(e.GameStatus);
    }

    static void chess_BlackPawnPromoted(object sender, PromotionEventArgs e)
    {
      e.PromotionPiece = promotion;
    }

    static void chess_WhitePawnPromoted(object sender, PromotionEventArgs e)
    {
      e.PromotionPiece = promotion;
    }
  }
}
