namespace ResolutionSwitcher.Gui
{
    /// <summary>
    /// Allow only one instance of application
    /// </summary>
    internal class AppSingleton : IDisposable
    {
        public AppSingleton(string uniqueId)
        {
            _mutex = new Mutex(false, uniqueId);
        }

        private Mutex _mutex;

        public bool IsRunning
        {
            get
            {
                if (!_mutex.WaitOne(TimeSpan.FromSeconds(2), false))
                    return true;
                return false;
            }
        }

        public void Dispose()
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex = null;
            }
        }
    }
}
