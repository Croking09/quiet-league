using System.Runtime.InteropServices;

namespace QuietLeague;

internal static class WindowsDoNotDisturb
{
  internal const string Unrestricted = "Microsoft.QuietHoursProfile.Unrestricted";
  internal const string PriorityOnly = "Microsoft.QuietHoursProfile.PriorityOnly";

  [ComImport]
  [Guid("6BFF4732-81EC-4FFB-AE67-B6C1BC29631F")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  private interface IQuietHoursSettings
  {
    void GetUserSelectedProfile(out IntPtr profile);
    void SetUserSelectedProfile([MarshalAs(UnmanagedType.LPWStr)] string profile);
  }

  [DllImport("ole32.dll", ExactSpelling = true)]
  private static extern int CoCreateInstance(
      ref Guid classId,
      IntPtr outer,
      uint classContext,
      ref Guid interfaceId,
      [MarshalAs(UnmanagedType.Interface)] out IQuietHoursSettings settings);

  internal static string GetProfile()
  {
    var settings = OpenSettings();

    try
    {
      settings.GetUserSelectedProfile(out IntPtr profile);

      try
      {
        return Marshal.PtrToStringUni(profile)
            ?? throw new InvalidOperationException("Windows returned an empty profile.");
      }
      finally
      {
        Marshal.FreeCoTaskMem(profile);
      }
    }
    finally
    {
      Marshal.ReleaseComObject(settings);
    }
  }

  internal static void SetProfile(string profile)
  {
    var settings = OpenSettings();

    try
    {
      settings.SetUserSelectedProfile(profile);
    }
    finally
    {
      Marshal.ReleaseComObject(settings);
    }
  }

  private static IQuietHoursSettings OpenSettings()
  {
    const uint localServer = 4;
    var classId = new Guid("F53321FA-34F8-4B7F-B9A3-361877CB94CF");
    var interfaceId = typeof(IQuietHoursSettings).GUID;

    int result = CoCreateInstance(
        ref classId, IntPtr.Zero, localServer, ref interfaceId, out var settings);

    Marshal.ThrowExceptionForHR(result);
    return settings;
  }
}