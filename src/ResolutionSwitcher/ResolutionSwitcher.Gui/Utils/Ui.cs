using Avalonia.Controls;

namespace ResolutionSwitcher.Gui.Utils
{
    internal static class Ui
    {
        public static NativeMenuItem InsertItem(this NativeMenu menu, int index,
            string header, object? parameter = null)
        {
            var menuItem = new NativeMenuItem
            {
                Header = header,
                CommandParameter = parameter,
            };
            menu.Items.Insert(index, menuItem);
            return menuItem;
        }

        
    }
}
