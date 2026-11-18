using System.Diagnostics;

namespace QuietLeague;

internal sealed class NotificationController
{
  private bool _wasMatchRunning;
  private string? _profileToRestore;

  internal bool CheckMatch()
  {
    bool matchRunning = IsMatchRunning();

    if (matchRunning && !_wasMatchRunning)
    {
      EnableDoNotDisturb();
    }
    else if (!matchRunning)
    {
      RestoreNotificationProfile();
    }

    _wasMatchRunning = matchRunning;
    return matchRunning;
  }

  private static bool IsMatchRunning()
  {
    Process[] gameProcesses = Process.GetProcessesByName("League of Legends");

    try
    {
      return gameProcesses.Length > 0;
    }
    finally
    {
      foreach (Process process in gameProcesses)
      {
        process.Dispose();
      }
    }
  }

  private void EnableDoNotDisturb()
  {
    string currentProfile = WindowsDoNotDisturb.GetProfile();

    if (currentProfile == WindowsDoNotDisturb.Unrestricted)
    {
      WindowsDoNotDisturb.SetProfile(WindowsDoNotDisturb.PriorityOnly);
      _profileToRestore = currentProfile;
    }
  }

  internal void ToggleDoNotDisturb()
  {
    bool matchRunning = IsMatchRunning();
    string currentProfile = WindowsDoNotDisturb.GetProfile();
    string newProfile = currentProfile == WindowsDoNotDisturb.Unrestricted
        ? WindowsDoNotDisturb.PriorityOnly
        : WindowsDoNotDisturb.Unrestricted;

    WindowsDoNotDisturb.SetProfile(newProfile);

    // Keep the manual choice through the end of the current match.
    _profileToRestore = null;
    _wasMatchRunning = matchRunning;
  }

  internal void RestoreNotificationProfile()
  {
    if (_profileToRestore is not string previousProfile)
    {
      return;
    }

    if (WindowsDoNotDisturb.GetProfile() == WindowsDoNotDisturb.PriorityOnly)
    {
      WindowsDoNotDisturb.SetProfile(previousProfile);
    }

    _profileToRestore = null;
  }
}
