namespace QuietLeague;

static class Program
{
  [STAThread]
  static void Main()
  {
    ApplicationConfiguration.Initialize();

    using var context = new QuietLeagueContext();
    Application.Run(context);
  }
}