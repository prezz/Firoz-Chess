namespace Chess.UI
{
  partial class AboutForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
      this.m_btnOk = new System.Windows.Forms.Button();
      this.m_textAbout = new System.Windows.Forms.TextBox();
      this.m_textAssemblies = new System.Windows.Forms.TextBox();
      this.m_labelAssemblies = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // m_btnOk
      // 
      resources.ApplyResources(this.m_btnOk, "m_btnOk");
      this.m_btnOk.Name = "m_btnOk";
      this.m_btnOk.UseVisualStyleBackColor = true;
      this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
      // 
      // m_textAbout
      // 
      this.m_textAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
      resources.ApplyResources(this.m_textAbout, "m_textAbout");
      this.m_textAbout.Name = "m_textAbout";
      this.m_textAbout.ReadOnly = true;
      // 
      // m_textAssemblies
      // 
      resources.ApplyResources(this.m_textAssemblies, "m_textAssemblies");
      this.m_textAssemblies.Name = "m_textAssemblies";
      this.m_textAssemblies.ReadOnly = true;
      // 
      // m_labelAssemblies
      // 
      resources.ApplyResources(this.m_labelAssemblies, "m_labelAssemblies");
      this.m_labelAssemblies.Name = "m_labelAssemblies";
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // AboutForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.m_labelAssemblies);
      this.Controls.Add(this.m_textAssemblies);
      this.Controls.Add(this.m_textAbout);
      this.Controls.Add(this.m_btnOk);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button m_btnOk;
    private System.Windows.Forms.TextBox m_textAbout;
    private System.Windows.Forms.TextBox m_textAssemblies;
    private System.Windows.Forms.Label m_labelAssemblies;
    private System.Windows.Forms.Label label1;
  }
}