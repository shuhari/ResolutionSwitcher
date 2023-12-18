using Avalonia.Controls;

namespace ResolutionSwitcher.Gui
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object? sender, WindowClosingEventArgs e)
        {
            if (e.CloseReason == WindowCloseReason.WindowClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }
    }
}