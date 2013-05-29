using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Drawing;
using Chess.UI.Properties;
using Chess.UI.BoardGraphic;
using Chess.Model;


namespace Chess.UI
{
  public partial class MainForm : Form
  {
    private Font m_panelFont;
    private Brush m_blackBrush;
    private Brush m_whiteBrush;
    private Graphics m_whiteTimeGraphic;
    private Graphics m_blackTimeGraphic;
    private Graphics m_messagesGraphic;

    private ChessFacade m_chessFacade;
    private GraphicHandler m_graphicHandler;
    private Square m_movingFrom;


    public MainForm()
    {
      InitializeComponent();

      m_panelFont = new Font(FontFamily.GenericSerif, 16, FontStyle.Regular);
      m_whiteBrush = new SolidBrush(Color.White);
      m_blackBrush = new SolidBrush(Color.Black);
      pictureWhiteTime.Image = new Bitmap(pictureWhiteTime.Width - 4, pictureWhiteTime.Height - 4);
      pictureBlackTime.Image = new Bitmap(pictureBlackTime.Width - 4, pictureBlackTime.Height - 4);
      pictureStateMessages.Image = new Bitmap(pictureStateMessages.Width - 4, pictureStateMessages.Height - 4);
      m_whiteTimeGraphic = Graphics.FromImage(pictureWhiteTime.Image);
      m_blackTimeGraphic = Graphics.FromImage(pictureBlackTime.Image);
      m_messagesGraphic = Graphics.FromImage(pictureStateMessages.Image);

      m_chessFacade = new ChessFacade();
      m_graphicHandler = new BasicDesign(boardPicture);

      m_chessFacade.BoardChanged += HandleBoardChangedEvent;
      m_chessFacade.WhitePawnPromoted += HandleWhitePawnPromotedEvent;
      m_chessFacade.BlackPawnPromoted += HandleBlackPawnPromotedEvent;
      m_chessFacade.StatusInfo += HandleStateChangedEvent;
      m_chessFacade.WhiteClockNotifier += HandleWhiteTimeEvent;
      m_chessFacade.BlackClockNotifier += HandleBlackTimeEvent;
      OutputWriter.WriteOutput += HandleWriteOutput;

      m_chessFacade.RaiseEvents();
      m_chessFacade.LoadOpeningBook();
    }

    public void SetupProfilingTest()
    {
      PositionEditor editor = m_chessFacade.PositionEditor;
      editor[Square.E2] = Piece.None;
      editor[Square.E4] = Piece.WhitePawn;
      editor[Square.E7] = Piece.None;
      editor[Square.E5] = Piece.BlackPawn;
      editor[Square.G1] = Piece.None;
      editor[Square.F3] = Piece.WhiteKnight;
      editor[Square.B8] = Piece.None;
      editor[Square.C6] = Piece.BlackKnight;
      m_chessFacade.PositionEditor = editor;

      ClockConfiguration clockConfig = m_chessFacade.ClockConfiguration;
      clockConfig.ClockType = ClockType.None;
      m_chessFacade.ClockConfiguration = clockConfig;

      EngineConfiguration engineConfig = m_chessFacade.EngineConfiguration;
      engineConfig.EngineAutoPlay = false;
      engineConfig.UseBook = false;
      engineConfig.MaxSearchTime = new TimeSpan(0, 5, 0);
      engineConfig.MaxSearchDepth = 5;
      m_chessFacade.EngineConfiguration = engineConfig;
    }

    public void RunProfilingTest()
    {
      m_chessFacade.ForceEngineToMove();
    }

    private void HandleWhiteTimeEvent(object sender, ClockEventArgs e)
    {
      string time;
      if (e.Time.Milliseconds <= 0 || e.ClockType == ClockType.None)
      {
        time = e.Time.ToString();
      }
      else
      {
        TimeSpan timeSpan = new TimeSpan(e.Time.Hours, e.Time.Minutes, e.Time.Seconds + 1);
        time = timeSpan.ToString();
      }

      if (time.Contains("."))
        time = time.Substring(0, time.IndexOf('.'));

      m_whiteTimeGraphic.Clear(Color.White);
      m_whiteTimeGraphic.DrawString(time, m_panelFont, m_blackBrush, (time[0] == '-') ? -3 : 4, 1);
      pictureWhiteTime.Refresh();
    }

    private void HandleBlackTimeEvent(object sender, ClockEventArgs e)
    {
      string time;
      if (e.Time.Milliseconds <= 0 || e.ClockType == ClockType.None)
      {
        time = e.Time.ToString();
      }
      else
      {
        TimeSpan timeSpan = new TimeSpan(e.Time.Hours, e.Time.Minutes, e.Time.Seconds + 1);
        time = timeSpan.ToString();
      }

      if (time.Contains("."))
        time = time.Substring(0, time.IndexOf('.'));

      m_blackTimeGraphic.Clear(Color.Black);
      m_blackTimeGraphic.DrawString(time, m_panelFont, m_whiteBrush, (time[0] == '-') ? -3 : 4, 1);
      pictureBlackTime.Refresh();
    }

    private void HandleBoardChangedEvent(object sender, BoardChangedEventArgs e)
    {
      m_graphicHandler.SwipeBoard();
      for (Square square = Square.A1; square <= Square.H8; ++square)
        m_graphicHandler.DrawPiece(square, m_chessFacade.PieceAt(square));
      m_graphicHandler.HighlightSquare(e.From);
      m_graphicHandler.HighlightSquare(e.To);
      m_graphicHandler.Refresh();

      prevGameBtn.Enabled = (m_chessFacade.PreviousGameCount > 0);
      prevMoveBtn.Enabled = (m_chessFacade.UndoCount > 0);
      nextMoveBtn.Enabled = (m_chessFacade.RedoCount > 0);
      nextGameBtn.Enabled = (m_chessFacade.SubsequentGameCount > 0);
    }

    private void HandleWhitePawnPromotedEvent(object sender, PromotionEventArgs e)
    {
      e.PromotionPiece = PawnPromotionDialog.ShowPawnPromotionDialog(this);
    }

    private void HandleBlackPawnPromotedEvent(object sender, PromotionEventArgs e)
    {
      e.PromotionPiece = PawnPromotionDialog.ShowPawnPromotionDialog(this);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
    private void HandleStateChangedEvent(object sender, GameStatusEventArgs e)
    {
      string message = "";
      m_messagesGraphic.Clear(Color.White);

      switch (e.GameStatus)
      {
        case GameStatus.WhiteCheckmateVictory:
          message = Resources.WHITE_WIN + "\n" + Resources.BLACK_MATE;
          break;

        case GameStatus.BlackCheckmateVictory:
          message = Resources.BLACK_WIN + "\n" + Resources.WHITE_MATE;
          break;

        case GameStatus.WhiteTimeVictory:
          message = Resources.WHITE_WIN + "\n" + Resources.BLACK_TIME;
          break;

        case GameStatus.BlackTimeVictory:
          message = Resources.BLACK_WIN + "\n" + Resources.WHITE_TIME;
          break;

        case GameStatus.StalemateDraw:
          message = Resources.DRAW + "\n" + Resources.STALEMATE;
          break;

        case GameStatus.InsufficientPiecesDraw:
          message = Resources.DRAW + "\n" + Resources.INSUFFICIENT_PIECES;
          break;

        case GameStatus.MoveRepetitionDraw:
          message = Resources.DRAW + "\n" + Resources.MOVE_REPETITION;
          break;

        case GameStatus.FiftyMovesDraw:
          message = Resources.DRAW + "\n" + Resources.FIFTY_MOVE_RULE;
          break;

        case GameStatus.WhiteIsCheck:
          message = Resources.CHECK + "\n" + Resources.WHITE_CHECK;
          break;

        case GameStatus.BlackIsCheck:
          message = Resources.CHECK + "\n" + Resources.BLACK_CHECK;
          break;
      }

      m_messagesGraphic.DrawString(message, m_panelFont, m_blackBrush, 5, 5);
      pictureStateMessages.Refresh();
    }

    private void HandleWriteOutput(object sender, OutputWriterEventArgs e)
    {
      if (outputTextBox.InvokeRequired)
      {
        outputTextBox.BeginInvoke(new EventHandler<OutputWriterEventArgs>(HandleWriteOutput), new object[] { sender, e });
      }
      else
      {
        outputTextBox.AppendText(e.Text);
        outputTextBox.AppendText("\r\n");
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      m_chessFacade.BoardChanged -= HandleBoardChangedEvent;
      m_chessFacade.WhitePawnPromoted -= HandleWhitePawnPromotedEvent;
      m_chessFacade.BlackPawnPromoted -= HandleBlackPawnPromotedEvent;
      m_chessFacade.StatusInfo -= HandleStateChangedEvent;
      m_chessFacade.WhiteClockNotifier -= HandleWhiteTimeEvent;
      m_chessFacade.BlackClockNotifier -= HandleBlackTimeEvent;

      OutputWriter.WriteOutput -= HandleWriteOutput;

      m_chessFacade.Quit();
      base.OnClosing(e);
    }

    private void boardPicture_MouseDown(object sender, MouseEventArgs e)
    {
      m_movingFrom = m_graphicHandler.CoordinateToSquare(e.X, e.Y);

      m_graphicHandler.SwipeBoard();
      for (Square square = Square.A1; square <= Square.H8; ++square)
        m_graphicHandler.DrawPiece(square, m_chessFacade.PieceAt(square));

      Square[] squares = m_chessFacade.GetValidSquaresForPiece(m_movingFrom);
      foreach (Square square in squares)
        m_graphicHandler.HighlightSquare(square);

      m_graphicHandler.Refresh();
    }

    private void boardPicture_MouseUp(object sender, MouseEventArgs e)
    {
      Square movingTo = m_graphicHandler.CoordinateToSquare(e.X, e.Y);
      if (!m_chessFacade.PerformMove(m_movingFrom, movingTo))
      {
        m_graphicHandler.SwipeBoard();
        for (Square square = Square.A1; square <= Square.H8; ++square)
          m_graphicHandler.DrawPiece(square, m_chessFacade.PieceAt(square));
        m_graphicHandler.Refresh();
      }
    }

    private void newGameMenu_Click(object sender, EventArgs e)
    {
      m_chessFacade.NewGame();
    }

    private void quitMenu_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void newGameBtn_Click(object sender, EventArgs e)
    {
      m_chessFacade.NewGame();
    }

    private void prevGameBtn_Click(object sender, EventArgs e)
    {
      m_chessFacade.PreviousGame();
    }

    private void prevMoveBtn_Click(object sender, EventArgs e)
    {
      m_chessFacade.UndoMove();
    }

    private void nextMoveBtn_Click(object sender, EventArgs e)
    {
      m_chessFacade.RedoMove();
    }

    private void nextGameBtn_Click(object sender, EventArgs e)
    {
      m_chessFacade.SubsequentGame();
    }

    private void positionMenu_Click(object sender, EventArgs e)
    {
      PositionEditorForm.ShowPositionEditorForm(this, m_chessFacade);
    }

    private void forceEngineMoveBtn_Click(object sender, EventArgs e)
    {
      m_chessFacade.ForceEngineToMove();
    }

    private void flipViewBtn_Click(object sender, EventArgs e)
    {
      m_graphicHandler.ViewFromBlack = !m_graphicHandler.ViewFromBlack;
      m_graphicHandler.SwipeBoard();
      for (Square square = Square.A1; square <= Square.H8; ++square)
        m_graphicHandler.DrawPiece(square, m_chessFacade.PieceAt(square));
      m_graphicHandler.Refresh();
    }

    private void addToBookMenu_Click(object sender, EventArgs e)
    {
      m_chessFacade.AddToOpeningBook(1);
      m_chessFacade.SaveOpeningBook();
    }

    private void benchmarkMenu_Click(object sender, EventArgs e)
    {
      SetupProfilingTest();
    }

    private void configurationMenu_Click(object sender, EventArgs e)
    {
      JointConfiguration jointConfig = new JointConfiguration(m_chessFacade.EngineConfiguration, m_chessFacade.ClockConfiguration);
      jointConfig = ConfigurationForm.ShowConfigurationForm(this, jointConfig);
      m_chessFacade.EngineConfiguration = jointConfig.EngineConfiguration;
      m_chessFacade.ClockConfiguration = jointConfig.ClockConfiguration;
    }

    private void aboutMenu_Click(object sender, EventArgs e)
    {
      AboutForm.ShowAboutForm(this);
    }

    private void splitContainer_Panel1_Resize(object sender, EventArgs e)
    {
      int dimension = Math.Min(splitContainer.Panel1.Width, splitContainer.Panel1.Height);

      boardPicture.Width = boardPicture.Height = dimension - 24;
      boardPicture.Left = ((splitContainer.Panel1.Width - boardPicture.Width) / 2) - 2;
      boardPicture.Top = ((splitContainer.Panel1.Height - boardPicture.Height) / 2) - 2;
    }

    private void splitContainer_Panel2_Resize(object sender, EventArgs e)
    {
      pictureWhiteTime.Width = (splitContainer.Panel2.Width / 2) - 12;
      pictureBlackTime.Width = (splitContainer.Panel2.Width / 2) - 12;

      pictureWhiteTime.Height = pictureWhiteTime.Width / 3;
      pictureBlackTime.Height = pictureBlackTime.Width / 3;

      pictureBlackTime.Left = pictureWhiteTime.Right + 7;

      pictureStateMessages.Top = pictureWhiteTime.Bottom + 10;
      pictureStateMessages.Width = pictureWhiteTime.Width + pictureBlackTime.Width + 7;
      pictureStateMessages.Height = pictureStateMessages.Width / 3;
    }
  }
}