namespace Chess.UI
{
  partial class PositionEditorForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionEditorForm));
      this.m_pictureBoxBoard = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxBlackKing = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxWhiteKing = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxWhiteQueen = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxWhiteRook = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxWhiteBishop = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxWhiteKnight = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxWhitePawn = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxBlackQueen = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxBlackRook = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxBlackBishop = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxBlackKnight = new System.Windows.Forms.PictureBox();
      this.m_pictureBoxBlackPawn = new System.Windows.Forms.PictureBox();
      this.m_btnOk = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.m_radioSideBlack = new System.Windows.Forms.RadioButton();
      this.m_radioSideWhite = new System.Windows.Forms.RadioButton();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.m_checkBlackLong = new System.Windows.Forms.CheckBox();
      this.m_checkBlackShort = new System.Windows.Forms.CheckBox();
      this.m_checkWhiteLong = new System.Windows.Forms.CheckBox();
      this.m_checkWhiteShort = new System.Windows.Forms.CheckBox();
      this.m_comboEnPassant = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.m_btnReset = new System.Windows.Forms.Button();
      this.m_btnClear = new System.Windows.Forms.Button();
      this.m_btnCancel = new System.Windows.Forms.Button();
      this.m_labelInvalid = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBoard)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackKing)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteKing)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteQueen)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteRook)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteBishop)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteKnight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhitePawn)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackQueen)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackRook)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackBishop)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackKnight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackPawn)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_pictureBoxBoard
      // 
      resources.ApplyResources(this.m_pictureBoxBoard, "m_pictureBoxBoard");
      this.m_pictureBoxBoard.Name = "m_pictureBoxBoard";
      this.m_pictureBoxBoard.TabStop = false;
      this.m_pictureBoxBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_pictureBoxBoard_MouseUp);
      // 
      // m_pictureBoxBlackKing
      // 
      this.m_pictureBoxBlackKing.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxBlackKing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxBlackKing, "m_pictureBoxBlackKing");
      this.m_pictureBoxBlackKing.Name = "m_pictureBoxBlackKing";
      this.m_pictureBoxBlackKing.TabStop = false;
      this.m_pictureBoxBlackKing.Click += new System.EventHandler(this.m_pictureBoxBlackKing_Click);
      // 
      // m_pictureBoxWhiteKing
      // 
      this.m_pictureBoxWhiteKing.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxWhiteKing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxWhiteKing, "m_pictureBoxWhiteKing");
      this.m_pictureBoxWhiteKing.Name = "m_pictureBoxWhiteKing";
      this.m_pictureBoxWhiteKing.TabStop = false;
      this.m_pictureBoxWhiteKing.Click += new System.EventHandler(this.m_pictureBoxWhiteKing_Click);
      // 
      // m_pictureBoxWhiteQueen
      // 
      this.m_pictureBoxWhiteQueen.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxWhiteQueen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxWhiteQueen, "m_pictureBoxWhiteQueen");
      this.m_pictureBoxWhiteQueen.Name = "m_pictureBoxWhiteQueen";
      this.m_pictureBoxWhiteQueen.TabStop = false;
      this.m_pictureBoxWhiteQueen.Click += new System.EventHandler(this.m_pictureBoxWhiteQueen_Click);
      // 
      // m_pictureBoxWhiteRook
      // 
      this.m_pictureBoxWhiteRook.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxWhiteRook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxWhiteRook, "m_pictureBoxWhiteRook");
      this.m_pictureBoxWhiteRook.Name = "m_pictureBoxWhiteRook";
      this.m_pictureBoxWhiteRook.TabStop = false;
      this.m_pictureBoxWhiteRook.Click += new System.EventHandler(this.m_pictureBoxWhiteRook_Click);
      // 
      // m_pictureBoxWhiteBishop
      // 
      this.m_pictureBoxWhiteBishop.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxWhiteBishop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxWhiteBishop, "m_pictureBoxWhiteBishop");
      this.m_pictureBoxWhiteBishop.Name = "m_pictureBoxWhiteBishop";
      this.m_pictureBoxWhiteBishop.TabStop = false;
      this.m_pictureBoxWhiteBishop.Click += new System.EventHandler(this.m_pictureBoxWhiteBishop_Click);
      // 
      // m_pictureBoxWhiteKnight
      // 
      this.m_pictureBoxWhiteKnight.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxWhiteKnight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxWhiteKnight, "m_pictureBoxWhiteKnight");
      this.m_pictureBoxWhiteKnight.Name = "m_pictureBoxWhiteKnight";
      this.m_pictureBoxWhiteKnight.TabStop = false;
      this.m_pictureBoxWhiteKnight.Click += new System.EventHandler(this.m_pictureBoxWhiteKnight_Click);
      // 
      // m_pictureBoxWhitePawn
      // 
      this.m_pictureBoxWhitePawn.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxWhitePawn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxWhitePawn, "m_pictureBoxWhitePawn");
      this.m_pictureBoxWhitePawn.Name = "m_pictureBoxWhitePawn";
      this.m_pictureBoxWhitePawn.TabStop = false;
      this.m_pictureBoxWhitePawn.Click += new System.EventHandler(this.m_pictureBoxWhitePawn_Click);
      // 
      // m_pictureBoxBlackQueen
      // 
      this.m_pictureBoxBlackQueen.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxBlackQueen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxBlackQueen, "m_pictureBoxBlackQueen");
      this.m_pictureBoxBlackQueen.Name = "m_pictureBoxBlackQueen";
      this.m_pictureBoxBlackQueen.TabStop = false;
      this.m_pictureBoxBlackQueen.Click += new System.EventHandler(this.m_pictureBoxBlackQueen_Click);
      // 
      // m_pictureBoxBlackRook
      // 
      this.m_pictureBoxBlackRook.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxBlackRook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxBlackRook, "m_pictureBoxBlackRook");
      this.m_pictureBoxBlackRook.Name = "m_pictureBoxBlackRook";
      this.m_pictureBoxBlackRook.TabStop = false;
      this.m_pictureBoxBlackRook.Click += new System.EventHandler(this.m_pictureBoxBlackRook_Click);
      // 
      // m_pictureBoxBlackBishop
      // 
      this.m_pictureBoxBlackBishop.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxBlackBishop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxBlackBishop, "m_pictureBoxBlackBishop");
      this.m_pictureBoxBlackBishop.Name = "m_pictureBoxBlackBishop";
      this.m_pictureBoxBlackBishop.TabStop = false;
      this.m_pictureBoxBlackBishop.Click += new System.EventHandler(this.m_pictureBoxBlackBishop_Click);
      // 
      // m_pictureBoxBlackKnight
      // 
      this.m_pictureBoxBlackKnight.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxBlackKnight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxBlackKnight, "m_pictureBoxBlackKnight");
      this.m_pictureBoxBlackKnight.Name = "m_pictureBoxBlackKnight";
      this.m_pictureBoxBlackKnight.TabStop = false;
      this.m_pictureBoxBlackKnight.Click += new System.EventHandler(this.m_pictureBoxBlackKnight_Click);
      // 
      // m_pictureBoxBlackPawn
      // 
      this.m_pictureBoxBlackPawn.BackColor = System.Drawing.SystemColors.Control;
      this.m_pictureBoxBlackPawn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      resources.ApplyResources(this.m_pictureBoxBlackPawn, "m_pictureBoxBlackPawn");
      this.m_pictureBoxBlackPawn.Name = "m_pictureBoxBlackPawn";
      this.m_pictureBoxBlackPawn.TabStop = false;
      this.m_pictureBoxBlackPawn.Click += new System.EventHandler(this.m_pictureBoxBlackPawn_Click);
      // 
      // m_btnOk
      // 
      resources.ApplyResources(this.m_btnOk, "m_btnOk");
      this.m_btnOk.Name = "m_btnOk";
      this.m_btnOk.UseVisualStyleBackColor = true;
      this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.m_radioSideBlack);
      this.groupBox1.Controls.Add(this.m_radioSideWhite);
      resources.ApplyResources(this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      // 
      // m_radioSideBlack
      // 
      resources.ApplyResources(this.m_radioSideBlack, "m_radioSideBlack");
      this.m_radioSideBlack.Name = "m_radioSideBlack";
      this.m_radioSideBlack.UseVisualStyleBackColor = true;
      this.m_radioSideBlack.Click += new System.EventHandler(this.m_radioSideBlack_Click);
      // 
      // m_radioSideWhite
      // 
      resources.ApplyResources(this.m_radioSideWhite, "m_radioSideWhite");
      this.m_radioSideWhite.Name = "m_radioSideWhite";
      this.m_radioSideWhite.UseVisualStyleBackColor = true;
      this.m_radioSideWhite.Click += new System.EventHandler(this.m_radioSideWhite_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.m_checkBlackLong);
      this.groupBox2.Controls.Add(this.m_checkBlackShort);
      this.groupBox2.Controls.Add(this.m_checkWhiteLong);
      this.groupBox2.Controls.Add(this.m_checkWhiteShort);
      resources.ApplyResources(this.groupBox2, "groupBox2");
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      // 
      // m_checkBlackLong
      // 
      resources.ApplyResources(this.m_checkBlackLong, "m_checkBlackLong");
      this.m_checkBlackLong.Name = "m_checkBlackLong";
      this.m_checkBlackLong.UseVisualStyleBackColor = true;
      this.m_checkBlackLong.CheckedChanged += new System.EventHandler(this.m_checkBlackLong_CheckedChanged);
      // 
      // m_checkBlackShort
      // 
      resources.ApplyResources(this.m_checkBlackShort, "m_checkBlackShort");
      this.m_checkBlackShort.Name = "m_checkBlackShort";
      this.m_checkBlackShort.UseVisualStyleBackColor = true;
      this.m_checkBlackShort.CheckedChanged += new System.EventHandler(this.m_checkBlackShort_CheckedChanged);
      // 
      // m_checkWhiteLong
      // 
      resources.ApplyResources(this.m_checkWhiteLong, "m_checkWhiteLong");
      this.m_checkWhiteLong.Name = "m_checkWhiteLong";
      this.m_checkWhiteLong.UseVisualStyleBackColor = true;
      this.m_checkWhiteLong.CheckedChanged += new System.EventHandler(this.m_checkWhiteLong_CheckedChanged);
      // 
      // m_checkWhiteShort
      // 
      resources.ApplyResources(this.m_checkWhiteShort, "m_checkWhiteShort");
      this.m_checkWhiteShort.Name = "m_checkWhiteShort";
      this.m_checkWhiteShort.UseVisualStyleBackColor = true;
      this.m_checkWhiteShort.CheckedChanged += new System.EventHandler(this.m_checkWhiteShort_CheckedChanged);
      // 
      // m_comboEnPassant
      // 
      this.m_comboEnPassant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.m_comboEnPassant.FormattingEnabled = true;
      resources.ApplyResources(this.m_comboEnPassant, "m_comboEnPassant");
      this.m_comboEnPassant.Name = "m_comboEnPassant";
      this.m_comboEnPassant.SelectedIndexChanged += new System.EventHandler(this.m_comboEnPassant_SelectedIndexChanged);
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // m_btnReset
      // 
      resources.ApplyResources(this.m_btnReset, "m_btnReset");
      this.m_btnReset.Name = "m_btnReset";
      this.m_btnReset.UseVisualStyleBackColor = true;
      this.m_btnReset.Click += new System.EventHandler(this.m_btnReset_Click);
      // 
      // m_btnClear
      // 
      resources.ApplyResources(this.m_btnClear, "m_btnClear");
      this.m_btnClear.Name = "m_btnClear";
      this.m_btnClear.UseVisualStyleBackColor = true;
      this.m_btnClear.Click += new System.EventHandler(this.m_btnClear_Click);
      // 
      // m_btnCancel
      // 
      resources.ApplyResources(this.m_btnCancel, "m_btnCancel");
      this.m_btnCancel.Name = "m_btnCancel";
      this.m_btnCancel.UseVisualStyleBackColor = true;
      this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
      // 
      // m_labelInvalid
      // 
      resources.ApplyResources(this.m_labelInvalid, "m_labelInvalid");
      this.m_labelInvalid.Name = "m_labelInvalid";
      // 
      // PositionEditorForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.m_labelInvalid);
      this.Controls.Add(this.m_btnCancel);
      this.Controls.Add(this.m_btnClear);
      this.Controls.Add(this.m_btnReset);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.m_comboEnPassant);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.m_btnOk);
      this.Controls.Add(this.m_pictureBoxBlackPawn);
      this.Controls.Add(this.m_pictureBoxBlackKnight);
      this.Controls.Add(this.m_pictureBoxBlackBishop);
      this.Controls.Add(this.m_pictureBoxBlackRook);
      this.Controls.Add(this.m_pictureBoxBlackQueen);
      this.Controls.Add(this.m_pictureBoxWhitePawn);
      this.Controls.Add(this.m_pictureBoxWhiteKnight);
      this.Controls.Add(this.m_pictureBoxWhiteBishop);
      this.Controls.Add(this.m_pictureBoxWhiteRook);
      this.Controls.Add(this.m_pictureBoxWhiteQueen);
      this.Controls.Add(this.m_pictureBoxWhiteKing);
      this.Controls.Add(this.m_pictureBoxBlackKing);
      this.Controls.Add(this.m_pictureBoxBoard);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "PositionEditorForm";
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBoard)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackKing)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteKing)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteQueen)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteRook)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteBishop)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhiteKnight)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxWhitePawn)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackQueen)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackRook)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackBishop)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackKnight)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxBlackPawn)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox m_pictureBoxBoard;
    private System.Windows.Forms.PictureBox m_pictureBoxBlackKing;
    private System.Windows.Forms.PictureBox m_pictureBoxWhiteKing;
    private System.Windows.Forms.PictureBox m_pictureBoxWhiteQueen;
    private System.Windows.Forms.PictureBox m_pictureBoxWhiteRook;
    private System.Windows.Forms.PictureBox m_pictureBoxWhiteBishop;
    private System.Windows.Forms.PictureBox m_pictureBoxWhiteKnight;
    private System.Windows.Forms.PictureBox m_pictureBoxWhitePawn;
    private System.Windows.Forms.PictureBox m_pictureBoxBlackQueen;
    private System.Windows.Forms.PictureBox m_pictureBoxBlackRook;
    private System.Windows.Forms.PictureBox m_pictureBoxBlackBishop;
    private System.Windows.Forms.PictureBox m_pictureBoxBlackKnight;
    private System.Windows.Forms.PictureBox m_pictureBoxBlackPawn;
    private System.Windows.Forms.Button m_btnOk;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton m_radioSideBlack;
    private System.Windows.Forms.RadioButton m_radioSideWhite;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox m_checkBlackLong;
    private System.Windows.Forms.CheckBox m_checkBlackShort;
    private System.Windows.Forms.CheckBox m_checkWhiteLong;
    private System.Windows.Forms.CheckBox m_checkWhiteShort;
    private System.Windows.Forms.ComboBox m_comboEnPassant;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button m_btnReset;
    private System.Windows.Forms.Button m_btnClear;
    private System.Windows.Forms.Button m_btnCancel;
    private System.Windows.Forms.Label m_labelInvalid;
  }
}