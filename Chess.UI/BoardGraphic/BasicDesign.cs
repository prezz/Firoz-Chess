using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Chess.Model;


namespace Chess.UI.BoardGraphic
{
  class BasicDesign : GraphicHandler
  {
    protected Image[] m_pieces;
    protected Image m_board;


    public BasicDesign(PictureBox target)
      : base(target)
    { }


    public override Image BoardImage
    {
      get { return m_board; }
    }


    public override Image PieceImage(Piece piece)
    {
      return m_pieces[(int)piece];
    }


    protected override void InitializeGraphic()
    {
      Assembly asm = Assembly.GetExecutingAssembly();

      m_pieces = new Image[13];
      m_pieces[(int)Piece.WhitePawn] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.WhitePawn.gif"));
      m_pieces[(int)Piece.BlackPawn] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.BlackPawn.gif"));
      m_pieces[(int)Piece.WhiteKnight] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.WhiteKnight.gif"));
      m_pieces[(int)Piece.BlackKnight] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.BlackKnight.gif"));
      m_pieces[(int)Piece.WhiteBishop] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.WhiteBishop.gif"));
      m_pieces[(int)Piece.BlackBishop] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.BlackBishop.gif"));
      m_pieces[(int)Piece.WhiteRook] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.WhiteRook.gif"));
      m_pieces[(int)Piece.BlackRook] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.BlackRook.gif"));
      m_pieces[(int)Piece.WhiteQueen] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.WhiteQueen.gif"));
      m_pieces[(int)Piece.BlackQueen] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.BlackQueen.gif"));
      m_pieces[(int)Piece.WhiteKing] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.WhiteKing.gif"));
      m_pieces[(int)Piece.BlackKing] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.BlackKing.gif"));
      m_pieces[(int)Piece.None] = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.NoPiece.gif"));

      m_board = Image.FromStream(asm.GetManifestResourceStream("Chess.UI.BoardGraphic.BasicDesign.ChessBoard.jpg"));
    }


    protected override int BoardSize
    {
      get { return 680; }
    }


    protected override int EdgeSize
    {
      get { return 20; }
    }


    protected override int SquareSize
    {
      get { return 80; }
    }
  }
}
