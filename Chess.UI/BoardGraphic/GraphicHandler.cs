using System;
using System.Drawing;
using System.Windows.Forms;
using Chess.Model;


namespace Chess.UI.BoardGraphic
{
  abstract class GraphicHandler
  {
    protected static bool m_viewFromBlack = false;

    protected PictureBox m_pictureBox;
    protected Graphics m_picturePainter;
    protected Pen m_highlightPen;
    protected Font m_squareFont;
    protected SolidBrush m_fontBrush;


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Chess.UI.BoardGraphic.GraphicHandler..ctor(Chess.Core.ChessInterface,System.Windows.Forms.PictureBox)")]
    protected GraphicHandler(PictureBox pictureBox)  //Target picture box must be completly filled with board picture for correct behavour
    {
      InitializeGraphic();

      m_pictureBox = pictureBox;
      m_pictureBox.Image = new Bitmap(BoardImage, BoardSize, BoardSize);
      m_picturePainter = Graphics.FromImage(m_pictureBox.Image);

      //TODO: Make following color and fontstyles part of subclass graphic configuration.
      m_highlightPen = new Pen(Color.Blue, 3.0f);
      m_squareFont = new Font("Arial", (int)Math.Round((float)EdgeSize * 0.66f), FontStyle.Bold);
      m_fontBrush = new SolidBrush(Color.White);
    }


    public bool ViewFromBlack
    {
      get { return m_viewFromBlack; }
      set { m_viewFromBlack = value; }
    }


    public void SwipeBoard()
    {
      //Draw empty board image.
      m_picturePainter.DrawImage(BoardImage, 0, 0, BoardSize, BoardSize);

      //Draw square names in board side.
      for (int x = 0; x < 8; ++x)
      {
        char c = (char)((m_viewFromBlack) ? ('H' - x) : ('A' + x));
        m_picturePainter.DrawString(c.ToString(), m_squareFont, m_fontBrush, EdgeSize + (SquareSize * x) + (SquareSize * 0.39f), BoardSize - EdgeSize);
      }

      for (int y = 0; y < 8; ++y)
      {
        int c = (m_viewFromBlack) ? (y + 1) : (8 - y);
        m_picturePainter.DrawString(c.ToString(), m_squareFont, m_fontBrush, EdgeSize * 0.15f, EdgeSize + (SquareSize * y) + (SquareSize * 0.39f));
      }
    }


    public void DrawPiece(Square square, Piece piece)
    {
      if (square != Square.None && piece != Piece.None)
      {
        Image image = PieceImage(piece);

        switch (m_viewFromBlack)
        {
          case true:
            m_picturePainter.DrawImage(image, ((7 - ChessFacade.File(square)) * SquareSize) + EdgeSize, (ChessFacade.Rank(square) * SquareSize) + EdgeSize, SquareSize, SquareSize);
            break;

          case false:
            m_picturePainter.DrawImage(image, (ChessFacade.File(square) * SquareSize) + EdgeSize, ((7 - ChessFacade.Rank(square)) * SquareSize) + EdgeSize, SquareSize, SquareSize);
            break;
        }
      }
    }


    public void HighlightSquare(Square square)
    {
      if (square != Square.None)
      {
        switch (m_viewFromBlack)
        {
          case true:
            m_picturePainter.DrawRectangle(m_highlightPen, ((7 - ChessFacade.File(square)) * SquareSize) + EdgeSize, (ChessFacade.Rank(square) * SquareSize) + EdgeSize, SquareSize, SquareSize);
            break;

          case false:
            m_picturePainter.DrawRectangle(m_highlightPen, (ChessFacade.File(square) * SquareSize) + EdgeSize, ((7 - ChessFacade.Rank(square)) * SquareSize) + EdgeSize, SquareSize, SquareSize);
            break;
        }
      }
    }


    public void Refresh()
    {
      m_pictureBox.Refresh();
    }


    public Square CoordinateToSquare(int x, int y)
    {
      if (x > FactoredEdgeWidth && x < (FactoredSquareWidth * 8) + FactoredEdgeWidth &&
          y > FactoredEdgeHeight && y < (FactoredSquareHeight * 8) + FactoredEdgeHeight)
      {
        int file = (x - FactoredEdgeWidth) / FactoredSquareWidth;
        int rank = (y - FactoredEdgeHeight) / FactoredSquareHeight;

        if (m_viewFromBlack)
          file = 7 - file;
        else
          rank = 7 - rank;

        return ChessFacade.Position(file, rank);
      }

      return Square.None;
    }


    public abstract Image BoardImage { get; }
    public abstract Image PieceImage(Piece piece);
    protected abstract void InitializeGraphic();
    protected abstract int BoardSize { get; }
    protected abstract int EdgeSize { get; }
    protected abstract int SquareSize { get; }


    private int FactoredEdgeWidth
    {
      get { return (int)Math.Round((float)EdgeSize * ((float)m_pictureBox.Width / (float)BoardSize)); }
    }


    private int FactoredEdgeHeight
    {
      get { return (int)Math.Round((float)EdgeSize * ((float)m_pictureBox.Height / (float)BoardSize)); }
    }


    private int FactoredSquareWidth
    {
      get { return (int)Math.Round((float)SquareSize * ((float)m_pictureBox.Width / (float)BoardSize)); }
    }


    private int FactoredSquareHeight
    {
      get { return (int)Math.Round((float)SquareSize * ((float)m_pictureBox.Height / (float)BoardSize)); }
    }
  }
}
