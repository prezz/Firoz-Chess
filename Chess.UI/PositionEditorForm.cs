using System;
using System.Windows.Forms;
using Chess.UI.BoardGraphic;
using Chess.Model;


namespace Chess.UI
{
  public partial class PositionEditorForm : Form
  {
    private GraphicHandler m_graphicHandler;
    private Piece m_editorPiece;
    private ChessFacade m_chessFacade;
    private PositionEditor m_positionEditor;


    public static void ShowPositionEditorForm(IWin32Window owner, ChessFacade chessFacade)
    {
      PositionEditorForm editorForm = new PositionEditorForm(chessFacade);
      editorForm.ShowDialog(owner);
      editorForm.Dispose();
    }


    private PositionEditorForm(ChessFacade chessFacade)
    {
      InitializeComponent();

      m_chessFacade = chessFacade;
      m_graphicHandler = new BasicDesign(m_pictureBoxBoard);

      m_pictureBoxWhiteKing.Image = m_graphicHandler.PieceImage(Piece.WhiteKing);
      m_pictureBoxWhiteQueen.Image = m_graphicHandler.PieceImage(Piece.WhiteQueen);
      m_pictureBoxWhiteRook.Image = m_graphicHandler.PieceImage(Piece.WhiteRook);
      m_pictureBoxWhiteBishop.Image = m_graphicHandler.PieceImage(Piece.WhiteBishop);
      m_pictureBoxWhiteKnight.Image = m_graphicHandler.PieceImage(Piece.WhiteKnight);
      m_pictureBoxWhitePawn.Image = m_graphicHandler.PieceImage(Piece.WhitePawn);
      m_pictureBoxBlackKing.Image = m_graphicHandler.PieceImage(Piece.BlackKing);
      m_pictureBoxBlackQueen.Image = m_graphicHandler.PieceImage(Piece.BlackQueen);
      m_pictureBoxBlackRook.Image = m_graphicHandler.PieceImage(Piece.BlackRook);
      m_pictureBoxBlackBishop.Image = m_graphicHandler.PieceImage(Piece.BlackBishop);
      m_pictureBoxBlackKnight.Image = m_graphicHandler.PieceImage(Piece.BlackKnight);
      m_pictureBoxBlackPawn.Image = m_graphicHandler.PieceImage(Piece.BlackPawn);

      m_comboEnPassant.Items.Add(Square.None);
      for (Square square = Square.A4; square <= Square.H5; ++square)
        m_comboEnPassant.Items.Add(square);

      SetEditorPiece(Piece.None);
      m_positionEditor = m_chessFacade.PositionEditor;
      m_positionEditor.BoardChanged += HandleBoardChangedEvent;
      UpdateInterface();
    }


    private void HandleBoardChangedEvent(object sender, BoardChangedEventArgs e)
    {
      UpdateInterface();
    }


    private void UpdateInterface()
    {
      //Draw board
      m_graphicHandler.SwipeBoard();
      for (Square square = Square.A1; square <= Square.H8; ++square)
        m_graphicHandler.DrawPiece(square, m_positionEditor[square]);
      m_graphicHandler.Refresh();

      //Update interface controls
      m_btnOk.Enabled = m_positionEditor.ValidatePosition();
      m_labelInvalid.Visible = !m_btnOk.Enabled;

      m_radioSideWhite.Checked = (m_positionEditor.ColorToPlay == PieceColor.White);
      m_radioSideBlack.Checked = (m_positionEditor.ColorToPlay == PieceColor.Black);

      m_checkWhiteShort.Checked = m_positionEditor.WhiteCanCastleShort;
      m_checkWhiteLong.Checked = m_positionEditor.WhiteCanCastleLong;
      m_checkBlackShort.Checked = m_positionEditor.BlackCanCastleShort;
      m_checkBlackLong.Checked = m_positionEditor.BlackCanCastleLong;

      m_comboEnPassant.SelectedItem = m_positionEditor.EnpassantTarget;
    }


    private void SetEditorPiece(Piece piece)
    {
      m_pictureBoxWhiteKing.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxWhiteQueen.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxWhiteRook.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxWhiteBishop.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxWhiteKnight.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxWhitePawn.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxBlackKing.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxBlackQueen.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxBlackRook.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxBlackBishop.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxBlackKnight.BorderStyle = BorderStyle.FixedSingle;
      m_pictureBoxBlackPawn.BorderStyle = BorderStyle.FixedSingle;

      m_editorPiece = (m_editorPiece == piece) ? Piece.None : piece;

      switch (m_editorPiece)
      {
        case Piece.WhiteKing:
          m_pictureBoxWhiteKing.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.WhiteQueen:
          m_pictureBoxWhiteQueen.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.WhiteRook:
          m_pictureBoxWhiteRook.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.WhiteBishop:
          m_pictureBoxWhiteBishop.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.WhiteKnight:
          m_pictureBoxWhiteKnight.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.WhitePawn:
          m_pictureBoxWhitePawn.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.BlackKing:
          m_pictureBoxBlackKing.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.BlackQueen:
          m_pictureBoxBlackQueen.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.BlackRook:
          m_pictureBoxBlackRook.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.BlackBishop:
          m_pictureBoxBlackBishop.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.BlackKnight:
          m_pictureBoxBlackKnight.BorderStyle = BorderStyle.Fixed3D;
          break;

        case Piece.BlackPawn:
          m_pictureBoxBlackPawn.BorderStyle = BorderStyle.Fixed3D;
          break;
      }
    }


    private void m_pictureBoxBoard_MouseUp(object sender, MouseEventArgs e)
    {
      switch (e.Button)
      {
        case MouseButtons.Left:
          {
            Square square = m_graphicHandler.CoordinateToSquare(e.X, e.Y);
            m_positionEditor[square] = m_editorPiece;
          }
          break;

        case MouseButtons.Right:
          {
            Square square = m_graphicHandler.CoordinateToSquare(e.X, e.Y);
            Piece piece = m_positionEditor[square];
            SetEditorPiece(piece);
          }
          break;
      }
    }


    private void m_pictureBoxWhiteKing_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.WhiteKing);
    }


    private void m_pictureBoxWhiteQueen_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.WhiteQueen);
    }


    private void m_pictureBoxWhiteRook_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.WhiteRook);
    }


    private void m_pictureBoxWhiteBishop_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.WhiteBishop);
    }


    private void m_pictureBoxWhiteKnight_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.WhiteKnight);
    }


    private void m_pictureBoxWhitePawn_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.WhitePawn);
    }


    private void m_pictureBoxBlackKing_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.BlackKing);
    }


    private void m_pictureBoxBlackQueen_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.BlackQueen);
    }


    private void m_pictureBoxBlackRook_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.BlackRook);
    }


    private void m_pictureBoxBlackBishop_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.BlackBishop);
    }


    private void m_pictureBoxBlackKnight_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.BlackKnight);
    }


    private void m_pictureBoxBlackPawn_Click(object sender, EventArgs e)
    {
      SetEditorPiece(Piece.BlackPawn);
    }


    private void m_radioSideWhite_Click(object sender, EventArgs e)
    {
      m_positionEditor.ColorToPlay = PieceColor.White;
    }


    private void m_radioSideBlack_Click(object sender, EventArgs e)
    {
      m_positionEditor.ColorToPlay = PieceColor.Black;
    }


    private void m_checkWhiteShort_CheckedChanged(object sender, EventArgs e)
    {
      m_positionEditor.WhiteCanCastleShort = m_checkWhiteShort.Checked;
    }


    private void m_checkWhiteLong_CheckedChanged(object sender, EventArgs e)
    {
      m_positionEditor.WhiteCanCastleLong = m_checkWhiteLong.Checked;
    }


    private void m_checkBlackShort_CheckedChanged(object sender, EventArgs e)
    {
      m_positionEditor.BlackCanCastleShort = m_checkBlackShort.Checked;
    }


    private void m_checkBlackLong_CheckedChanged(object sender, EventArgs e)
    {
      m_positionEditor.BlackCanCastleLong = m_checkBlackLong.Checked;
    }


    private void m_comboEnPassant_SelectedIndexChanged(object sender, EventArgs e)
    {
      m_positionEditor.EnpassantTarget = (Square)m_comboEnPassant.SelectedItem;
    }


    private void m_btnOk_Click(object sender, EventArgs e)
    {
      if (m_positionEditor.ValidatePosition())
      {
        m_positionEditor.BoardChanged -= HandleBoardChangedEvent;
        m_chessFacade.PositionEditor = m_positionEditor;
        this.Close();
      }
    }


    private void m_btnReset_Click(object sender, EventArgs e)
    {
      m_positionEditor.ResetPosition();
    }


    private void m_btnClear_Click(object sender, EventArgs e)
    {
      m_positionEditor.ClearBoard();
    }


    private void m_btnCancel_Click(object sender, EventArgs e)
    {
      m_chessFacade.BoardChanged -= HandleBoardChangedEvent;
      this.Close();
    }
  }
}