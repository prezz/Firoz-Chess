namespace Chess.UI
{
  partial class ConfigurationForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
      this.m_tabControlConfig = new System.Windows.Forms.TabControl();
      this.m_tabPageClock = new System.Windows.Forms.TabPage();
      this.m_labelType = new System.Windows.Forms.Label();
      this.m_comboBoxClockType = new System.Windows.Forms.ComboBox();
      this.m_labelSeconds = new System.Windows.Forms.Label();
      this.m_labelAnd = new System.Windows.Forms.Label();
      this.m_numericIncrementPlus = new System.Windows.Forms.NumericUpDown();
      this.m_labelStartMinutes = new System.Windows.Forms.Label();
      this.m_numericIncrementStart = new System.Windows.Forms.NumericUpDown();
      this.m_labelMinutes = new System.Windows.Forms.Label();
      this.m_numericConventionalMinutes = new System.Windows.Forms.NumericUpDown();
      this.m_labelMoves = new System.Windows.Forms.Label();
      this.m_numericConventionalMoves = new System.Windows.Forms.NumericUpDown();
      this.m_tabPageEngine = new System.Windows.Forms.TabPage();
      this.m_labelSearchSec = new System.Windows.Forms.Label();
      this.m_labelSearchDepth = new System.Windows.Forms.Label();
      this.m_labelSearchTime = new System.Windows.Forms.Label();
      this.m_numericSearchDepth = new System.Windows.Forms.NumericUpDown();
      this.m_checkBoxEngine = new System.Windows.Forms.CheckBox();
      this.m_numericSearchTime = new System.Windows.Forms.NumericUpDown();
      this.m_buttonOk = new System.Windows.Forms.Button();
      this.m_buttonCancel = new System.Windows.Forms.Button();
      this.m_checkBoxBook = new System.Windows.Forms.CheckBox();
      this.m_tabControlConfig.SuspendLayout();
      this.m_tabPageClock.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericIncrementPlus)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericIncrementStart)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericConventionalMinutes)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericConventionalMoves)).BeginInit();
      this.m_tabPageEngine.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericSearchDepth)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericSearchTime)).BeginInit();
      this.SuspendLayout();
      // 
      // m_tabControlConfig
      // 
      this.m_tabControlConfig.Controls.Add(this.m_tabPageClock);
      this.m_tabControlConfig.Controls.Add(this.m_tabPageEngine);
      resources.ApplyResources(this.m_tabControlConfig, "m_tabControlConfig");
      this.m_tabControlConfig.Name = "m_tabControlConfig";
      this.m_tabControlConfig.SelectedIndex = 0;
      // 
      // m_tabPageClock
      // 
      this.m_tabPageClock.Controls.Add(this.m_labelType);
      this.m_tabPageClock.Controls.Add(this.m_comboBoxClockType);
      this.m_tabPageClock.Controls.Add(this.m_labelSeconds);
      this.m_tabPageClock.Controls.Add(this.m_labelAnd);
      this.m_tabPageClock.Controls.Add(this.m_numericIncrementPlus);
      this.m_tabPageClock.Controls.Add(this.m_labelStartMinutes);
      this.m_tabPageClock.Controls.Add(this.m_numericIncrementStart);
      this.m_tabPageClock.Controls.Add(this.m_labelMinutes);
      this.m_tabPageClock.Controls.Add(this.m_numericConventionalMinutes);
      this.m_tabPageClock.Controls.Add(this.m_labelMoves);
      this.m_tabPageClock.Controls.Add(this.m_numericConventionalMoves);
      resources.ApplyResources(this.m_tabPageClock, "m_tabPageClock");
      this.m_tabPageClock.Name = "m_tabPageClock";
      this.m_tabPageClock.UseVisualStyleBackColor = true;
      // 
      // m_labelType
      // 
      resources.ApplyResources(this.m_labelType, "m_labelType");
      this.m_labelType.Name = "m_labelType";
      // 
      // m_comboBoxClockType
      // 
      this.m_comboBoxClockType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.m_comboBoxClockType.FormattingEnabled = true;
      resources.ApplyResources(this.m_comboBoxClockType, "m_comboBoxClockType");
      this.m_comboBoxClockType.Name = "m_comboBoxClockType";
      this.m_comboBoxClockType.SelectedValueChanged += new System.EventHandler(this.m_comboBoxClockType_SelectedValueChanged);
      // 
      // m_labelSeconds
      // 
      resources.ApplyResources(this.m_labelSeconds, "m_labelSeconds");
      this.m_labelSeconds.Name = "m_labelSeconds";
      // 
      // m_labelAnd
      // 
      resources.ApplyResources(this.m_labelAnd, "m_labelAnd");
      this.m_labelAnd.Name = "m_labelAnd";
      // 
      // m_numericIncrementPlus
      // 
      resources.ApplyResources(this.m_numericIncrementPlus, "m_numericIncrementPlus");
      this.m_numericIncrementPlus.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
      this.m_numericIncrementPlus.Name = "m_numericIncrementPlus";
      // 
      // m_labelStartMinutes
      // 
      resources.ApplyResources(this.m_labelStartMinutes, "m_labelStartMinutes");
      this.m_labelStartMinutes.Name = "m_labelStartMinutes";
      // 
      // m_numericIncrementStart
      // 
      resources.ApplyResources(this.m_numericIncrementStart, "m_numericIncrementStart");
      this.m_numericIncrementStart.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
      this.m_numericIncrementStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.m_numericIncrementStart.Name = "m_numericIncrementStart";
      this.m_numericIncrementStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // m_labelMinutes
      // 
      resources.ApplyResources(this.m_labelMinutes, "m_labelMinutes");
      this.m_labelMinutes.Name = "m_labelMinutes";
      // 
      // m_numericConventionalMinutes
      // 
      resources.ApplyResources(this.m_numericConventionalMinutes, "m_numericConventionalMinutes");
      this.m_numericConventionalMinutes.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
      this.m_numericConventionalMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.m_numericConventionalMinutes.Name = "m_numericConventionalMinutes";
      this.m_numericConventionalMinutes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // m_labelMoves
      // 
      resources.ApplyResources(this.m_labelMoves, "m_labelMoves");
      this.m_labelMoves.Name = "m_labelMoves";
      // 
      // m_numericConventionalMoves
      // 
      resources.ApplyResources(this.m_numericConventionalMoves, "m_numericConventionalMoves");
      this.m_numericConventionalMoves.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
      this.m_numericConventionalMoves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.m_numericConventionalMoves.Name = "m_numericConventionalMoves";
      this.m_numericConventionalMoves.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // m_tabPageEngine
      // 
      this.m_tabPageEngine.Controls.Add(this.m_checkBoxBook);
      this.m_tabPageEngine.Controls.Add(this.m_labelSearchSec);
      this.m_tabPageEngine.Controls.Add(this.m_labelSearchDepth);
      this.m_tabPageEngine.Controls.Add(this.m_labelSearchTime);
      this.m_tabPageEngine.Controls.Add(this.m_numericSearchDepth);
      this.m_tabPageEngine.Controls.Add(this.m_checkBoxEngine);
      this.m_tabPageEngine.Controls.Add(this.m_numericSearchTime);
      resources.ApplyResources(this.m_tabPageEngine, "m_tabPageEngine");
      this.m_tabPageEngine.Name = "m_tabPageEngine";
      this.m_tabPageEngine.UseVisualStyleBackColor = true;
      // 
      // m_labelSearchSec
      // 
      resources.ApplyResources(this.m_labelSearchSec, "m_labelSearchSec");
      this.m_labelSearchSec.Name = "m_labelSearchSec";
      // 
      // m_labelSearchDepth
      // 
      resources.ApplyResources(this.m_labelSearchDepth, "m_labelSearchDepth");
      this.m_labelSearchDepth.Name = "m_labelSearchDepth";
      // 
      // m_labelSearchTime
      // 
      resources.ApplyResources(this.m_labelSearchTime, "m_labelSearchTime");
      this.m_labelSearchTime.Name = "m_labelSearchTime";
      // 
      // m_numericSearchDepth
      // 
      resources.ApplyResources(this.m_numericSearchDepth, "m_numericSearchDepth");
      this.m_numericSearchDepth.Name = "m_numericSearchDepth";
      // 
      // m_checkBoxEngine
      // 
      resources.ApplyResources(this.m_checkBoxEngine, "m_checkBoxEngine");
      this.m_checkBoxEngine.Checked = true;
      this.m_checkBoxEngine.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_checkBoxEngine.Name = "m_checkBoxEngine";
      this.m_checkBoxEngine.UseVisualStyleBackColor = true;
      // 
      // m_numericSearchTime
      // 
      resources.ApplyResources(this.m_numericSearchTime, "m_numericSearchTime");
      this.m_numericSearchTime.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
      this.m_numericSearchTime.Name = "m_numericSearchTime";
      // 
      // m_buttonOk
      // 
      resources.ApplyResources(this.m_buttonOk, "m_buttonOk");
      this.m_buttonOk.Name = "m_buttonOk";
      this.m_buttonOk.UseVisualStyleBackColor = true;
      this.m_buttonOk.Click += new System.EventHandler(this.m_buttonOk_Click);
      // 
      // m_buttonCancel
      // 
      resources.ApplyResources(this.m_buttonCancel, "m_buttonCancel");
      this.m_buttonCancel.Name = "m_buttonCancel";
      this.m_buttonCancel.UseVisualStyleBackColor = true;
      this.m_buttonCancel.Click += new System.EventHandler(this.m_buttonCancel_Click);
      // 
      // m_checkBoxBook
      // 
      resources.ApplyResources(this.m_checkBoxBook, "m_checkBoxBook");
      this.m_checkBoxBook.Checked = true;
      this.m_checkBoxBook.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_checkBoxBook.Name = "m_checkBoxBook";
      this.m_checkBoxBook.UseVisualStyleBackColor = true;
      // 
      // ConfigurationForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.m_buttonOk);
      this.Controls.Add(this.m_buttonCancel);
      this.Controls.Add(this.m_tabControlConfig);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ConfigurationForm";
      this.ShowInTaskbar = false;
      this.m_tabControlConfig.ResumeLayout(false);
      this.m_tabPageClock.ResumeLayout(false);
      this.m_tabPageClock.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericIncrementPlus)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericIncrementStart)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericConventionalMinutes)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericConventionalMoves)).EndInit();
      this.m_tabPageEngine.ResumeLayout(false);
      this.m_tabPageEngine.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericSearchDepth)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_numericSearchTime)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl m_tabControlConfig;
    private System.Windows.Forms.TabPage m_tabPageClock;
    private System.Windows.Forms.CheckBox m_checkBoxEngine;
    private System.Windows.Forms.TabPage m_tabPageEngine;
    private System.Windows.Forms.Label m_labelSearchDepth;
    private System.Windows.Forms.Label m_labelSearchTime;
    private System.Windows.Forms.NumericUpDown m_numericSearchDepth;
    private System.Windows.Forms.NumericUpDown m_numericSearchTime;
    private System.Windows.Forms.Button m_buttonOk;
    private System.Windows.Forms.Button m_buttonCancel;
    private System.Windows.Forms.Label m_labelMinutes;
    private System.Windows.Forms.NumericUpDown m_numericConventionalMinutes;
    private System.Windows.Forms.Label m_labelMoves;
    private System.Windows.Forms.NumericUpDown m_numericConventionalMoves;
    private System.Windows.Forms.NumericUpDown m_numericIncrementStart;
    private System.Windows.Forms.Label m_labelSeconds;
    private System.Windows.Forms.Label m_labelAnd;
    private System.Windows.Forms.NumericUpDown m_numericIncrementPlus;
    private System.Windows.Forms.Label m_labelStartMinutes;
    private System.Windows.Forms.ComboBox m_comboBoxClockType;
    private System.Windows.Forms.Label m_labelType;
    private System.Windows.Forms.Label m_labelSearchSec;
    private System.Windows.Forms.CheckBox m_checkBoxBook;


  }
}