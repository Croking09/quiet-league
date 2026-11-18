# QuietLeague

QuietLeague is a Windows 11 system tray app that turns on Do Not Disturb while a League of Legends match is running. It watches for `League of Legends.exe`, so opening the League client alone does not trigger it.

## Use QuietLeague

1. Download `QuietLeague-win-x64.exe` from [GitHub Releases](https://github.com/Croking09/quiet-league/releases) on a Windows 11 PC with an Intel or AMD x64 processor. The self-contained executable includes .NET.
2. Open the executable and look for the QuietLeague icon in the system tray. Check the hidden icons if you do not see it next to the clock.
3. Start a match or enter the Practice Tool. The tray menu will show **Match in progress** once the game process starts.

If QuietLeague turns on Do Not Disturb automatically, it restores the previous setting when the match ends or when you choose **Exit**. Use **Toggle Do Not Disturb** in the tray menu to change the Windows setting manually. Your manual choice stays in effect when the current match ends or you exit QuietLeague.

## Development

- Windows 11
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git to clone the repository
- An editor such as VS Code

```powershell
git clone https://github.com/Croking09/quiet-league.git
cd quiet-league
dotnet run
```

The project uses C# and Windows Forms. Use the League client and Practice Tool to check the tray status before, during, and after a match.

## Build from source

On Windows 11, install the .NET 10 SDK and run this from the directory containing `QuietLeague.csproj`:

```powershell
dotnet build -c Release
```

To create a standalone executable for Windows x64, run:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -o bin/release-win-x64
```

The executable is written to `bin/release-win-x64/QuietLeague.exe`.

## Start with Windows

1. Put the downloaded or published executable in a permanent folder.
2. Press **Win+R**, enter `shell:startup`, and press **Enter**.
3. Create a shortcut to the executable in the folder that opens. QuietLeague will start the next time you sign in.

To disable automatic startup, delete the shortcut from that folder. [Windows startup apps documentation](https://support.microsoft.com/en-us/windows/experience/startup-boot/configure-startup-applications-in-windows)

## Limitations

QuietLeague uses an undocumented Windows interface for Do Not Disturb. Compatibility may vary across Windows builds. If you force-close the app during a match, you may need to turn off Do Not Disturb manually.
