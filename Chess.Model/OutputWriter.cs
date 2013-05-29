using System;
using System.Text;


namespace Chess.Model
{
  #region OutputWriterEventArgs

  /// <summary>
  /// Class containing event data for the "WriteOutput" event raised from the "OutputWriter" class.
  /// </summary>
  public class OutputWriterEventArgs : EventArgs
  {
    /// <summary>
    /// Text from the output writers buffer.
    /// </summary>
    private string m_text;


    /// <summary>
    /// Initializes a new instance of the OutputWriterEventArgs.
    /// </summary>
    /// <param name="text">Text to pass along.</param>
    internal OutputWriterEventArgs(string text)
    {
      m_text = text;
    }

    /// <summary>
    /// Text from the output writers buffer.
    /// </summary>
    public string Text
    {
      get { return m_text; }
    }
  }

  #endregion

  /// <summary>
  /// Class that can be used to write to an output of the users own choice.
  /// </summary>
  sealed public class OutputWriter
  {
    /// <summary>
    /// Lock used to make this class thread safe.
    /// </summary>
    private static object m_lock = new object();


    /// <summary>
    /// Event raised when text is added by the Write method. If updating user interface
    /// controls check out the InvokeRequired, BeginInvoke and EndInvoke methods found
    /// on user interface controls as this event might be raised from threads different
    /// then the thread the controls was created on.
    /// </summary>
    public static event EventHandler<OutputWriterEventArgs> WriteOutput;


    /// <summary>
    /// Private constructer ensuring objects of this class can't be instantiated.
    /// </summary>
    private OutputWriter()
    { }

    /// <summary>
    /// Raise the WriteOutput event such observers can write the text.
    /// </summary>
    /// <param name="text">Text to add.</param>
    public static void Write(string text)
    {
      lock (m_lock)
      {
        if (WriteOutput != null)
          WriteOutput(null, new OutputWriterEventArgs(text));
      }
    }
  }
}
