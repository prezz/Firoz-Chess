using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace Chess.UI
{
  public partial class AboutForm : Form
  {
    public static void ShowAboutForm(IWin32Window owner)
    {
      AboutForm aboutForm = new AboutForm();
      aboutForm.ShowDialog(owner);
      aboutForm.Dispose();
    }


    private AboutForm()
    {
      InitializeComponent();

      m_textAbout.Clear();
      m_textAssemblies.Clear();

      object[] attrs;
      Assembly assemblyUI = Assembly.GetExecutingAssembly();

      attrs = assemblyUI.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
      AssemblyProductAttribute prodAttrs = (AssemblyProductAttribute)attrs[0];
      m_textAbout.AppendText(prodAttrs.Product + "\r\n");

      attrs = assemblyUI.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
      AssemblyInformationalVersionAttribute versAttrs = (AssemblyInformationalVersionAttribute)attrs[0];
      m_textAbout.AppendText("Version " + versAttrs.InformationalVersion + "\r\n");

      attrs = assemblyUI.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
      AssemblyCompanyAttribute compAttrs = (AssemblyCompanyAttribute)attrs[0];
      m_textAbout.AppendText("By " + compAttrs.Company + "\r\n");

      attrs = assemblyUI.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
      AssemblyCopyrightAttribute copyAttrs = (AssemblyCopyrightAttribute)attrs[0];
      m_textAbout.AppendText(copyAttrs.Copyright + "\r\n");

      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach (Assembly assembly in assemblies)
      {
        if (assembly.GetName().Name.StartsWith("Chess."))
        {
          AssemblyName name = assembly.GetName();
          m_textAssemblies.AppendText(name.Name + "\r\n");
          m_textAssemblies.AppendText(" - Version: " + name.Version.ToString() + "\r\n");
          m_textAssemblies.AppendText(" - Build on: " + BuildTime(name.Version) + "\r\n\r\n");
        }
      }
    }


    private void m_btnOk_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    private static string BuildTime(Version version)
    {
      DateTime dt = new DateTime(2000, 1, 1);
      dt += new TimeSpan(version.Build, 0, 0, version.Revision * 2);

      if (TimeZone.IsDaylightSavingTime(dt, TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)))
        dt = dt.AddHours(1);

      return dt.ToLongDateString() + " at " + dt.ToLongTimeString();
    }
  }
}