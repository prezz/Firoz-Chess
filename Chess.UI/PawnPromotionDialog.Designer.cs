namespace Chess.UI
{
  partial class PawnPromotionDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PawnPromotionDialog));
      this.m_buttonQueen = new System.Windows.Forms.Button();
      this.m_buttonRook = new System.Windows.Forms.Button();
      this.m_buttonBishop = new System.Windows.Forms.Button();
      this.m_buttonKnight = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // m_buttonQueen
      // 
      resources.ApplyResources(this.m_buttonQueen, "m_buttonQueen");
      this.m_buttonQueen.Name = "m_buttonQueen";
      this.m_buttonQueen.UseVisualStyleBackColor = true;
      this.m_buttonQueen.Click += new System.EventHandler(this.buttonQueen_Click);
      // 
      // m_buttonRook
      // 
      resources.ApplyResources(this.m_buttonRook, "m_buttonRook");
      this.m_buttonRook.Name = "m_buttonRook";
      this.m_buttonRook.UseVisualStyleBackColor = true;
      this.m_buttonRook.Click += new System.EventHandler(this.buttonRook_Click);
      // 
      // m_buttonBishop
      // 
      resources.ApplyResources(this.m_buttonBishop, "m_buttonBishop");
      this.m_buttonBishop.Name = "m_buttonBishop";
      this.m_buttonBishop.UseVisualStyleBackColor = true;
      this.m_buttonBishop.Click += new System.EventHandler(this.buttonBishop_Click);
      // 
      // m_buttonKnight
      // 
      resources.ApplyResources(this.m_buttonKnight, "m_buttonKnight");
      this.m_buttonKnight.Name = "m_buttonKnight";
      this.m_buttonKnight.UseVisualStyleBackColor = true;
      this.m_buttonKnight.Click += new System.EventHandler(this.buttonKnight_Click);
      // 
      // PawnPromotionDialog
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.m_buttonKnight);
      this.Controls.Add(this.m_buttonBishop);
      this.Controls.Add(this.m_buttonRook);
      this.Controls.Add(this.m_buttonQueen);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PawnPromotionDialog";
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button m_buttonQueen;
    private System.Windows.Forms.Button m_buttonRook;
    private System.Windows.Forms.Button m_buttonBishop;
    private System.Windows.Forms.Button m_buttonKnight;
  }
}