namespace ResolutionSwitcher.Gui
{
    internal static class Program
    {
        const string AppUniqueId = "shuhari.ResolutionSwitcher";
        static Mutex mutex = new Mutex(false, AppUniqueId);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                return;
            }

            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}