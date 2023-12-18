using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "TBD", Scope = "module")]

namespace ResolutionSwitcher.Gui
{
    internal static class Program
    {
        private const string UniqueId = "shuhari.ResolutionSwitcher";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var singleton = new AppSingleton(UniqueId))
            {
                if (singleton.IsRunning)
                    return;
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
        }
    }
}