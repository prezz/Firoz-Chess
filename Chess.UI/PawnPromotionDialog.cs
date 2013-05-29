using System;
using System.Windows.Forms;
using Chess.Model;


namespace Chess.UI
{
  public partial class PawnPromotionDialog : Form
  {
    private PromotionPiece m_promotionPiece;


    public static PromotionPiece ShowPawnPromotionDialog(IWin32Window owner)
    {
      PawnPromotionDialog promotionDlg = new PawnPromotionDialog();
      promotionDlg.ShowDialog(owner);
      PromotionPiece result = promotionDlg.m_promotionPiece;
      promotionDlg.Dispose();
      return result;
    }


    private PawnPromotionDialog()
    {
      InitializeComponent();
    }


    private void buttonQueen_Click(object sender, EventArgs e)
    {
      m_promotionPiece = PromotionPiece.Queen;
      this.Close();
    }


    private void buttonRook_Click(object sender, EventArgs e)
    {
      m_promotionPiece = PromotionPiece.Rook;
      this.Close();
    }


    private void buttonBishop_Click(object sender, EventArgs e)
    {
      m_promotionPiece = PromotionPiece.Bishop;
      this.Close();
    }


    private void buttonKnight_Click(object sender, EventArgs e)
    {
      m_promotionPiece = PromotionPiece.Knight;
      this.Close();
    }
  }
}