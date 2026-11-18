namespace QuietLeague;

internal sealed class QuietLeagueContext : ApplicationContext
{
  private readonly ToolStripMenuItem _statusItem = new() { Enabled = false };
  private readonly ContextMenuStrip _menu = new();
  private readonly NotifyIcon _trayIcon = new();
  private readonly Icon _icon = new(typeof(QuietLeagueContext), "QuietLeague.ico");
  private readonly System.Windows.Forms.Timer _pollTimer = new() { Interval = 1000 };

  private readonly NotificationController _notifications = new();

  public QuietLeagueContext()
  {
    _menu.Items.Add(_statusItem);
    _menu.Items.Add("Toggle Do Not Disturb", null, (_, _) => ToggleDoNotDisturb());
    _menu.Items.Add("Exit", null, (_, _) => ExitQuietLeague());

    _trayIcon.Icon = _icon;
    _trayIcon.ContextMenuStrip = _menu;

    CheckMatch();
    _trayIcon.Visible = true;

    _pollTimer.Tick += (_, _) => CheckMatch();
    _pollTimer.Start();
  }

  private void CheckMatch()
  {
    try
    {
      bool matchRunning = _notifications.CheckMatch();
      SetStatus(matchRunning ? "Match in progress" : "Waiting for a match");
    }
    catch (Exception error)
    {
      SetStatus($"QuietLeague error (0x{error.HResult:X8})");
    }
  }

  private void ToggleDoNotDisturb()
  {
    try
    {
      _notifications.ToggleDoNotDisturb();
    }
    catch (Exception error)
    {
      MessageBox.Show(
          $"Could not toggle Do Not Disturb (0x{error.HResult:X8}).\n{error.Message}",
          "QuietLeague",
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
    }
  }

  private void SetStatus(string status)
  {
    _statusItem.Text = status;
    _trayIcon.Text = $"QuietLeague · {status}";
  }

  private void ExitQuietLeague()
  {
    try
    {
      _notifications.RestoreNotificationProfile();
    }
    catch (Exception error)
    {
      MessageBox.Show(
          $"Could not restore your notification settings (0x{error.HResult:X8}).\n" +
          $"{error.Message}\n\nQuietLeague is still running.",
          "QuietLeague",
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
      return;
    }

    ExitThread();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      _pollTimer.Dispose();
      _trayIcon.Dispose();
      _icon.Dispose();
      _menu.Dispose();
    }

    base.Dispose(disposing);
  }
}
