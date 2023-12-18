using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ResolutionSwitcher.Gui;

public partial class DumpDialog : Window
{
    public DumpDialog()
    {
        InitializeComponent();
    }

    public string? Details
    {
        get { return txtDetails.Text; }
        set { txtDetails.Text = value; }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}