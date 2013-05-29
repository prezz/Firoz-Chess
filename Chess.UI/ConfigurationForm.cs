using System;
using System.Windows.Forms;
using Chess.Model;


namespace Chess.UI
{
  #region JointConfiguration

  public struct JointConfiguration
  {
    private EngineConfiguration m_engineConfiguration;
    private ClockConfiguration m_clockConfiguration;


    public JointConfiguration(EngineConfiguration engineConfiguration, ClockConfiguration clockConfiguration)
    {
      m_engineConfiguration = engineConfiguration;
      m_clockConfiguration = clockConfiguration;
    }

    public EngineConfiguration EngineConfiguration
    {
      get { return m_engineConfiguration; }
    }

    public ClockConfiguration ClockConfiguration
    {
      get { return m_clockConfiguration; }
    }
  }

  #endregion


  public partial class ConfigurationForm : Form
  {
    private EngineConfiguration m_engineConfiguration;
    private ClockConfiguration m_clockConfiguration;


    public static JointConfiguration ShowConfigurationForm(IWin32Window owner, JointConfiguration currentConfig)
    {
      ConfigurationForm configForm = new ConfigurationForm(currentConfig);
      configForm.ShowDialog(owner);
      JointConfiguration result = new JointConfiguration(configForm.m_engineConfiguration, configForm.m_clockConfiguration);
      configForm.Dispose();
      return result;
    }


    private ConfigurationForm(JointConfiguration currentConfig)
    {
      InitializeComponent();

      m_comboBoxClockType.Items.Add(ClockType.Conventional);
      m_comboBoxClockType.Items.Add(ClockType.Incremental);
      m_comboBoxClockType.Items.Add(ClockType.None);

      m_engineConfiguration = currentConfig.EngineConfiguration;
      m_clockConfiguration = currentConfig.ClockConfiguration;

      m_checkBoxEngine.Checked = m_engineConfiguration.EngineAutoPlay;
      m_checkBoxBook.Checked = m_engineConfiguration.UseBook;
      m_numericSearchTime.Value = (decimal)m_engineConfiguration.MaxSearchTime.TotalSeconds;
      m_numericSearchDepth.Value = m_engineConfiguration.MaxSearchDepth;

      m_comboBoxClockType.SelectedItem = m_clockConfiguration.ClockType;
      m_numericConventionalMoves.Value = m_clockConfiguration.ConventionalMoves;
      m_numericConventionalMinutes.Value = (decimal)m_clockConfiguration.ConventionalTime.TotalMinutes;

      m_numericIncrementStart.Value = (decimal)m_clockConfiguration.IncrementStartTime.TotalMinutes;
      m_numericIncrementPlus.Value = (decimal)m_clockConfiguration.IncrementPlusTime.TotalSeconds;
    }


    private void m_buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    private void m_buttonOk_Click(object sender, EventArgs e)
    {
      m_engineConfiguration.EngineAutoPlay = m_checkBoxEngine.Checked;
      m_engineConfiguration.UseBook = m_checkBoxBook.Checked;
      m_engineConfiguration.MaxSearchTime = new TimeSpan(0, 0, (int)m_numericSearchTime.Value);
      m_engineConfiguration.MaxSearchDepth = (int)m_numericSearchDepth.Value;

      m_clockConfiguration.ClockType = (ClockType)m_comboBoxClockType.SelectedItem;

      m_clockConfiguration.ConventionalMoves = (int)m_numericConventionalMoves.Value;
      m_clockConfiguration.ConventionalTime = new TimeSpan(0, (int)m_numericConventionalMinutes.Value, 0);

      m_clockConfiguration.IncrementStartTime = new TimeSpan(0, (int)m_numericIncrementStart.Value, 0);
      m_clockConfiguration.IncrementPlusTime = new TimeSpan(0, 0, (int)m_numericIncrementPlus.Value);

      this.Close();
    }


    private void m_comboBoxClockType_SelectedValueChanged(object sender, EventArgs e)
    {
      switch ((ClockType)m_comboBoxClockType.SelectedItem)
      {
        case ClockType.Conventional:
          m_numericConventionalMoves.Enabled = true;
          m_numericConventionalMinutes.Enabled = true;
          m_numericIncrementStart.Enabled = false;
          m_numericIncrementPlus.Enabled = false;
          break;

        case ClockType.Incremental:
          m_numericConventionalMoves.Enabled = false;
          m_numericConventionalMinutes.Enabled = false;
          m_numericIncrementStart.Enabled = true;
          m_numericIncrementPlus.Enabled = true;
          break;

        case ClockType.None:
          m_numericConventionalMoves.Enabled = false;
          m_numericConventionalMinutes.Enabled = false;
          m_numericIncrementStart.Enabled = false;
          m_numericIncrementPlus.Enabled = false;
          break;
      }
    }
  }
}