using System;
using Chess.Model.Logic;


namespace Chess.Model.Engine
{
  /// <summary>
  /// Part of the partial class BoardEvaluator.
  /// This part is responsible for evaluating pawn structures.
  /// </summary>
  partial class BoardEvaluator
  {
    #region FilePawnInfo

    /// <summary>
    /// Structure holding pawn info for a file (column)
    /// </summary>
    struct FilePawnInfo
    {
      /// <summary>
      /// Number of pawns on file.
      /// </summary>
      public int PawnsOnFileCount;

      /// <summary>
      /// Rank of most advanced pawn.
      /// </summary>
      public int MostAdvancedPawn;

      /// <summary>
      /// Rank of most backward pawn.
      /// </summary>
      public int MostBackwardPawn;
    }

    #endregion

    /// <summary>
    /// Array holding pawn info for white for each file.
    /// </summary>
    private FilePawnInfo[] m_whitePawnInfo;

    /// <summary>
    /// Array holding pawn info for black for each file.
    /// </summary>
    private FilePawnInfo[] m_blackPawnInfo;


    /// <summary>
    /// Kind of a constructor for this partial part of BoardEvaluator.
    /// </summary>
    private void Pawns()
    {
      m_whitePawnInfo = new FilePawnInfo[8];
      m_blackPawnInfo = new FilePawnInfo[8];
    }

    /// <summary>
    /// Analyzes the pawn structure filling m_whitePawnInfo and m_blackPawnInfo with info.
    /// </summary>
    private void AnalyzePawnStructure()
    {
      for (int file = 0; file < 8; ++file)
      {
        m_whitePawnInfo[file].PawnsOnFileCount = 0;
        m_whitePawnInfo[file].MostAdvancedPawn = 0;
        m_whitePawnInfo[file].MostBackwardPawn = 7;

        m_blackPawnInfo[file].PawnsOnFileCount = 0;
        m_blackPawnInfo[file].MostAdvancedPawn = 7;
        m_blackPawnInfo[file].MostBackwardPawn = 0;

        for (Square square = (Square)(file + 16); square <= Square.H7; square += 16)
        {
          switch (m_board[square])
          {
            case Piece.WhitePawn:
              {
                int rank = Board.Rank(square);

                ++m_whitePawnInfo[file].PawnsOnFileCount;
                m_whitePawnInfo[file].MostAdvancedPawn = rank;
                if (m_whitePawnInfo[file].MostBackwardPawn > rank)
                  m_whitePawnInfo[file].MostBackwardPawn = rank;
              }
              break;

            case Piece.BlackPawn:
              {
                int rank = Board.Rank(square);

                ++m_blackPawnInfo[file].PawnsOnFileCount;
                m_blackPawnInfo[file].MostBackwardPawn = rank;
                if (m_blackPawnInfo[file].MostAdvancedPawn > rank)
                  m_blackPawnInfo[file].MostAdvancedPawn = rank;
              }
              break;
          }
        }
      }
    }

    /// <summary>
    /// Gives a penelty if more then one pawn exists on a file.
    /// </summary>
    private void EvaluateDoubledPawns()
    {
      foreach (FilePawnInfo whitePawnInfo in m_whitePawnInfo)
        if (whitePawnInfo.PawnsOnFileCount > 1)
          m_whiteScore -= (8 * (whitePawnInfo.PawnsOnFileCount - 1));

      foreach (FilePawnInfo blackPawnInfo in m_blackPawnInfo)
        if (blackPawnInfo.PawnsOnFileCount > 1)
          m_blackScore -= (8 * (blackPawnInfo.PawnsOnFileCount - 1));
    }

    /// <summary>
    /// Gives a penelty to isolated pawns which is pawns with no frindly pawns existing on the
    /// file to the left or right.
    /// </summary>
    private void EvaluateIsolatedPawns()
    {
      //Evaluate for white
      if (m_whitePawnInfo[0].PawnsOnFileCount > 0 && m_whitePawnInfo[1].PawnsOnFileCount == 0)
      {
        m_whiteScore -= 10;
      }
      for (int i = 1; i < 7; ++i)
      {
        if (m_whitePawnInfo[i].PawnsOnFileCount > 0 &&
            (m_whitePawnInfo[i - 1].PawnsOnFileCount == 0) || (m_whitePawnInfo[i + 1].PawnsOnFileCount == 0))
        {
          m_whiteScore -= 10;
        }
      }
      if (m_whitePawnInfo[7].PawnsOnFileCount > 0 && m_whitePawnInfo[6].PawnsOnFileCount == 0)
      {
        m_whiteScore -= 10;
      }

      //Evaluate for black
      if (m_blackPawnInfo[0].PawnsOnFileCount > 0 && m_blackPawnInfo[1].PawnsOnFileCount == 0)
      {
        m_blackScore -= 10;
      }
      for (int i = 1; i < 7; ++i)
      {
        if (m_blackPawnInfo[i].PawnsOnFileCount > 0 &&
            (m_blackPawnInfo[i - 1].PawnsOnFileCount == 0) || (m_blackPawnInfo[i + 1].PawnsOnFileCount == 0))
        {
          m_blackScore -= 10;
        }
      }
      if (m_blackPawnInfo[7].PawnsOnFileCount > 0 && m_blackPawnInfo[6].PawnsOnFileCount == 0)
      {
        m_blackScore -= 10;
      }
    }

    /// <summary>
    /// Gives a bonus if a pawn has passed all of the opponents pawn existing
    /// one the left, right and same file as the pawn.
    /// </summary>
    private void EvaluatePassedPawns()
    {
      //TODO: En-passant?

      //Evaluate for white
      if (m_whitePawnInfo[0].MostAdvancedPawn > m_blackPawnInfo[0].MostBackwardPawn &&
          m_whitePawnInfo[0].MostAdvancedPawn >= m_blackPawnInfo[1].MostBackwardPawn)
      {
        m_whiteScore += 15;
      }
      for (int i = 1; i < 7; ++i)
      {
        if (m_whitePawnInfo[i].MostAdvancedPawn >= m_blackPawnInfo[i - 1].MostBackwardPawn &&
            m_whitePawnInfo[i].MostAdvancedPawn > m_blackPawnInfo[i].MostBackwardPawn &&
            m_whitePawnInfo[i].MostAdvancedPawn >= m_blackPawnInfo[i + 1].MostBackwardPawn)
        {
          m_whiteScore += 15;
        }
      }
      if (m_whitePawnInfo[7].MostAdvancedPawn >= m_blackPawnInfo[6].MostBackwardPawn &&
          m_whitePawnInfo[7].MostAdvancedPawn > m_blackPawnInfo[7].MostBackwardPawn)
      {
        m_whiteScore += 15;
      }

      //Evaluate for black
      if (m_blackPawnInfo[0].MostAdvancedPawn < m_whitePawnInfo[0].MostBackwardPawn &&
          m_blackPawnInfo[0].MostAdvancedPawn <= m_whitePawnInfo[1].MostBackwardPawn)
      {
        m_blackScore += 15;
      }
      for (int i = 1; i < 7; ++i)
      {
        if (m_blackPawnInfo[i].MostAdvancedPawn <= m_whitePawnInfo[i - 1].MostBackwardPawn &&
            m_blackPawnInfo[i].MostAdvancedPawn < m_whitePawnInfo[i].MostBackwardPawn &&
            m_blackPawnInfo[i].MostAdvancedPawn <= m_whitePawnInfo[i + 1].MostBackwardPawn)
        {
          m_blackScore += 15;
        }
      }
      if (m_blackPawnInfo[7].MostAdvancedPawn <= m_whitePawnInfo[6].MostBackwardPawn &&
          m_blackPawnInfo[7].MostAdvancedPawn < m_whitePawnInfo[7].MostBackwardPawn)
      {
        m_blackScore += 15;
      }
    }

    /// <summary>
    /// Evaluates pawn structur.
    /// </summary>
    private void EvaluatePawns()
    {
      AnalyzePawnStructure();

      EvaluateDoubledPawns();
      EvaluateIsolatedPawns();
      EvaluatePassedPawns();
    }
  }
}
