using System;
using System.Windows.Forms;


namespace Chess.UI
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      Application.Run(new MainForm());

      /*
      MainForm mf = new MainForm();
      mf.Show();
      mf.SetupProfilingTest();
      mf.RunProfilingTest();
       * */
    }
  }
}