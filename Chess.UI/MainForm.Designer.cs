namespace Chess.UI
{
  partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.m_menuStrip = new System.Windows.Forms.MenuStrip();
      this.gameMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.newGameMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.quitMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.configMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.positionMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.developmentMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.addToBookMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.testMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.m_toolStrip = new System.Windows.Forms.ToolStrip();
      this.newGameBtn = new System.Windows.Forms.ToolStripButton();
      this.m_toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.prevGameBtn = new System.Windows.Forms.ToolStripButton();
      this.prevMoveBtn = new System.Windows.Forms.ToolStripButton();
      this.nextMoveBtn = new System.Windows.Forms.ToolStripButton();
      this.nextGameBtn = new System.Windows.Forms.ToolStripButton();
      this.m_toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.forceEngineMoveBtn = new System.Windows.Forms.ToolStripButton();
      this.flipViewBtn = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.boardPicture = new System.Windows.Forms.PictureBox();
      this.pictureStateMessages = new System.Windows.Forms.PictureBox();
      this.pictureBlackTime = new System.Windows.Forms.PictureBox();
      this.pictureWhiteTime = new System.Windows.Forms.PictureBox();
      this.outputTextBox = new System.Windows.Forms.TextBox();
      this.m_menuStrip.SuspendLayout();
      this.m_toolStrip.SuspendLayout();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.boardPicture)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureStateMessages)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBlackTime)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureWhiteTime)).BeginInit();
      this.SuspendLayout();
      // 
      // m_menuStrip
      // 
      this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameMenu,
            this.editMenu,
            this.developmentMenu});
      resources.ApplyResources(this.m_menuStrip, "m_menuStrip");
      this.m_menuStrip.Name = "m_menuStrip";
      // 
      // gameMenu
      // 
      this.gameMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameMenu,
            this.aboutMenu,
            this.quitMenu});
      this.gameMenu.Name = "gameMenu";
      resources.ApplyResources(this.gameMenu, "gameMenu");
      // 
      // newGameMenu
      // 
      this.newGameMenu.Name = "newGameMenu";
      resources.ApplyResources(this.newGameMenu, "newGameMenu");
      this.newGameMenu.Click += new System.EventHandler(this.newGameMenu_Click);
      // 
      // aboutMenu
      // 
      this.aboutMenu.Name = "aboutMenu";
      resources.ApplyResources(this.aboutMenu, "aboutMenu");
      this.aboutMenu.Click += new System.EventHandler(this.aboutMenu_Click);
      // 
      // quitMenu
      // 
      this.quitMenu.Name = "quitMenu";
      resources.ApplyResources(this.quitMenu, "quitMenu");
      this.quitMenu.Click += new System.EventHandler(this.quitMenu_Click);
      // 
      // editMenu
      // 
      this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configMenu,
            this.positionMenu});
      this.editMenu.Name = "editMenu";
      resources.ApplyResources(this.editMenu, "editMenu");
      // 
      // configMenu
      // 
      this.configMenu.Name = "configMenu";
      resources.ApplyResources(this.configMenu, "configMenu");
      this.configMenu.Click += new System.EventHandler(this.configurationMenu_Click);
      // 
      // positionMenu
      // 
      this.positionMenu.Name = "positionMenu";
      resources.ApplyResources(this.positionMenu, "positionMenu");
      this.positionMenu.Click += new System.EventHandler(this.positionMenu_Click);
      // 
      // developmentMenu
      // 
      this.developmentMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToBookMenu,
            this.testMenu});
      this.developmentMenu.Name = "developmentMenu";
      resources.ApplyResources(this.developmentMenu, "developmentMenu");
      // 
      // addToBookMenu
      // 
      this.addToBookMenu.Name = "addToBookMenu";
      resources.ApplyResources(this.addToBookMenu, "addToBookMenu");
      this.addToBookMenu.Click += new System.EventHandler(this.addToBookMenu_Click);
      // 
      // testMenu
      // 
      this.testMenu.Name = "testMenu";
      resources.ApplyResources(this.testMenu, "testMenu");
      this.testMenu.Click += new System.EventHandler(this.benchmarkMenu_Click);
      // 
      // m_toolStrip
      // 
      this.m_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameBtn,
            this.m_toolStripSeparator1,
            this.prevGameBtn,
            this.prevMoveBtn,
            this.nextMoveBtn,
            this.nextGameBtn,
            this.m_toolStripSeparator2,
            this.forceEngineMoveBtn,
            this.flipViewBtn,
            this.toolStripSeparator1});
      resources.ApplyResources(this.m_toolStrip, "m_toolStrip");
      this.m_toolStrip.Name = "m_toolStrip";
      // 
      // newGameBtn
      // 
      this.newGameBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.newGameBtn.Image = global::Chess.UI.Properties.Resources.NewGame;
      resources.ApplyResources(this.newGameBtn, "newGameBtn");
      this.newGameBtn.Name = "newGameBtn";
      this.newGameBtn.Click += new System.EventHandler(this.newGameBtn_Click);
      // 
      // m_toolStripSeparator1
      // 
      this.m_toolStripSeparator1.Name = "m_toolStripSeparator1";
      resources.ApplyResources(this.m_toolStripSeparator1, "m_toolStripSeparator1");
      // 
      // prevGameBtn
      // 
      this.prevGameBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.prevGameBtn.Image = global::Chess.UI.Properties.Resources.PrevGame;
      resources.ApplyResources(this.prevGameBtn, "prevGameBtn");
      this.prevGameBtn.Name = "prevGameBtn";
      this.prevGameBtn.Click += new System.EventHandler(this.prevGameBtn_Click);
      // 
      // prevMoveBtn
      // 
      this.prevMoveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.prevMoveBtn.Image = global::Chess.UI.Properties.Resources.PrevMove;
      resources.ApplyResources(this.prevMoveBtn, "prevMoveBtn");
      this.prevMoveBtn.Name = "prevMoveBtn";
      this.prevMoveBtn.Click += new System.EventHandler(this.prevMoveBtn_Click);
      // 
      // nextMoveBtn
      // 
      this.nextMoveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.nextMoveBtn.Image = global::Chess.UI.Properties.Resources.NextMove;
      resources.ApplyResources(this.nextMoveBtn, "nextMoveBtn");
      this.nextMoveBtn.Name = "nextMoveBtn";
      this.nextMoveBtn.Click += new System.EventHandler(this.nextMoveBtn_Click);
      // 
      // nextGameBtn
      // 
      this.nextGameBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.nextGameBtn.Image = global::Chess.UI.Properties.Resources.NextGame;
      resources.ApplyResources(this.nextGameBtn, "nextGameBtn");
      this.nextGameBtn.Name = "nextGameBtn";
      this.nextGameBtn.Click += new System.EventHandler(this.nextGameBtn_Click);
      // 
      // m_toolStripSeparator2
      // 
      this.m_toolStripSeparator2.Name = "m_toolStripSeparator2";
      resources.ApplyResources(this.m_toolStripSeparator2, "m_toolStripSeparator2");
      // 
      // forceEngineMoveBtn
      // 
      this.forceEngineMoveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.forceEngineMoveBtn.Image = global::Chess.UI.Properties.Resources.EngineMove;
      resources.ApplyResources(this.forceEngineMoveBtn, "forceEngineMoveBtn");
      this.forceEngineMoveBtn.Name = "forceEngineMoveBtn";
      this.forceEngineMoveBtn.Click += new System.EventHandler(this.forceEngineMoveBtn_Click);
      // 
      // flipViewBtn
      // 
      this.flipViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.flipViewBtn.Image = global::Chess.UI.Properties.Resources.FlipView;
      resources.ApplyResources(this.flipViewBtn, "flipViewBtn");
      this.flipViewBtn.Name = "flipViewBtn";
      this.flipViewBtn.Click += new System.EventHandler(this.flipViewBtn_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
      // 
      // splitContainer
      // 
      this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.splitContainer, "splitContainer");
      this.splitContainer.Name = "splitContainer";
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.BackColor = System.Drawing.Color.Black;
      this.splitContainer.Panel1.Controls.Add(this.boardPicture);
      this.splitContainer.Panel1.Resize += new System.EventHandler(this.splitContainer_Panel1_Resize);
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.pictureStateMessages);
      this.splitContainer.Panel2.Controls.Add(this.pictureBlackTime);
      this.splitContainer.Panel2.Controls.Add(this.pictureWhiteTime);
      this.splitContainer.Panel2.Controls.Add(this.outputTextBox);
      this.splitContainer.Panel2.Resize += new System.EventHandler(this.splitContainer_Panel2_Resize);
      // 
      // boardPicture
      // 
      this.boardPicture.BackColor = System.Drawing.SystemColors.Control;
      resources.ApplyResources(this.boardPicture, "boardPicture");
      this.boardPicture.Name = "boardPicture";
      this.boardPicture.TabStop = false;
      this.boardPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.boardPicture_MouseDown);
      this.boardPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.boardPicture_MouseUp);
      // 
      // pictureStateMessages
      // 
      this.pictureStateMessages.BackColor = System.Drawing.Color.White;
      this.pictureStateMessages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.pictureStateMessages, "pictureStateMessages");
      this.pictureStateMessages.Name = "pictureStateMessages";
      this.pictureStateMessages.TabStop = false;
      // 
      // pictureBlackTime
      // 
      resources.ApplyResources(this.pictureBlackTime, "pictureBlackTime");
      this.pictureBlackTime.BackColor = System.Drawing.Color.Black;
      this.pictureBlackTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureBlackTime.Name = "pictureBlackTime";
      this.pictureBlackTime.TabStop = false;
      // 
      // pictureWhiteTime
      // 
      this.pictureWhiteTime.BackColor = System.Drawing.Color.White;
      this.pictureWhiteTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.pictureWhiteTime, "pictureWhiteTime");
      this.pictureWhiteTime.Name = "pictureWhiteTime";
      this.pictureWhiteTime.TabStop = false;
      // 
      // outputTextBox
      // 
      resources.ApplyResources(this.outputTextBox, "outputTextBox");
      this.outputTextBox.Name = "outputTextBox";
      this.outputTextBox.ReadOnly = true;
      // 
      // MainForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer);
      this.Controls.Add(this.m_toolStrip);
      this.Controls.Add(this.m_menuStrip);
      this.MainMenuStrip = this.m_menuStrip;
      this.Name = "MainForm";
      this.m_menuStrip.ResumeLayout(false);
      this.m_menuStrip.PerformLayout();
      this.m_toolStrip.ResumeLayout(false);
      this.m_toolStrip.PerformLayout();
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.Panel2.PerformLayout();
      this.splitContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.boardPicture)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureStateMessages)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBlackTime)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureWhiteTime)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip m_menuStrip;
    private System.Windows.Forms.ToolStripMenuItem gameMenu;
    private System.Windows.Forms.ToolStripMenuItem newGameMenu;
    private System.Windows.Forms.ToolStripMenuItem quitMenu;
    private System.Windows.Forms.ToolStrip m_toolStrip;
    private System.Windows.Forms.ToolStripButton newGameBtn;
    private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton prevGameBtn;
    private System.Windows.Forms.ToolStripButton prevMoveBtn;
    private System.Windows.Forms.ToolStripButton nextMoveBtn;
    private System.Windows.Forms.ToolStripButton nextGameBtn;
    private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton flipViewBtn;
    private System.Windows.Forms.ToolStripMenuItem developmentMenu;
    private System.Windows.Forms.ToolStripMenuItem addToBookMenu;
    private System.Windows.Forms.ToolStripMenuItem testMenu;
    private System.Windows.Forms.ToolStripMenuItem editMenu;
    private System.Windows.Forms.ToolStripMenuItem configMenu;
    private System.Windows.Forms.ToolStripMenuItem positionMenu;
    private System.Windows.Forms.ToolStripButton forceEngineMoveBtn;
    private System.Windows.Forms.ToolStripMenuItem aboutMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.PictureBox boardPicture;
    private System.Windows.Forms.TextBox outputTextBox;
    private System.Windows.Forms.PictureBox pictureBlackTime;
    private System.Windows.Forms.PictureBox pictureWhiteTime;
    private System.Windows.Forms.PictureBox pictureStateMessages;
  }
}

