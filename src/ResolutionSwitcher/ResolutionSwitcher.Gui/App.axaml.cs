using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ResolutionSwitcher.Gui.Utils;
using ResolutionSwitcher.Models;
using ResolutionSwitcher.Runtime.Interop;

namespace ResolutionSwitcher.Gui
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private IClassicDesktopStyleApplicationLifetime GetLifeTime()
        {
            return (IClassicDesktopStyleApplicationLifetime)ApplicationLifetime!;
        }

        private MainWindow GetMainWindow()
        {
            return (MainWindow)GetLifeTime().MainWindow!;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var mainWin = new MainWindow();
            var lifetime = GetLifeTime();
            lifetime.MainWindow = mainWin;
            lifetime.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            base.OnFrameworkInitializationCompleted();

            RebuildMenu();
            mainWin.Hide();
        }

        private DateTime? _trayTimeClick = null;

        private bool IsDoubleClick()
        {
            // Avalonia UI TrayIcon has no doubleClick event.
            // Idea from: https://github.com/AvaloniaUI/Avalonia/issues/10269
            if (_trayTimeClick == null)
            {
                _trayTimeClick = DateTime.Now;
                return false;
            }

            var delta = (DateTime.Now - _trayTimeClick).Value;
            if (delta.TotalMilliseconds > WinApi.GetDoubleClickTime())
            {
                _trayTimeClick = DateTime.Now;
                return false;
            }

            _trayTimeClick = null;
            return true;
        }

        private void TrayIcon_Clicked(object? sender, EventArgs e)
        {
            if (IsDoubleClick()) 
            {
                var mainWin = GetMainWindow();
                mainWin.Show();
                mainWin.Activate();
            }
        }

        private void mnuCustomize_Click(object sender, EventArgs e)
        {
            var mainWin = GetMainWindow();
            mainWin.Show();
            mainWin.Activate();
        }

        private void mnuDump_Click(object sender, EventArgs e)
        {
            var lines = DisplayService.Dump();
            var text = string.Join(Environment.NewLine, lines);
            var dlg = new DumpDialog { Details = text };

            dlg.Show();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            GetLifeTime().Shutdown();
        }

        private void RebuildMenu()
        {
            var menu = TrayIcon.GetIcons(this)![0].Menu!;
            foreach (var item in menu.Items.Where(IsResolutionMenu).ToArray())
            {
                menu.Items.Remove(item);
            }

            var monitors = DisplayService.EnumMonitors();
            if (monitors.Length == 1)
            {
                AddResolutionMenus(menu, monitors[0].Resolutions);
            }
            else
            {
                int menuIndex = 0;
                foreach (var monitor in monitors)
                {
                    var item = menu.InsertItem(menuIndex, monitor.DisplayName, monitor);
                    item.Menu = new NativeMenu();
                    AddResolutionMenus(item.Menu, monitor.Resolutions);
                    menuIndex++;
                }
            }
        }

        private bool IsResolutionMenu(NativeMenuItemBase item)
        {
            if (item is NativeMenuItem mi &&
                (mi.CommandParameter is MonitorInfo || mi.CommandParameter is MonitorResolution))
                return true;
            return false;
        }

        private void AddResolutionMenus(NativeMenu parent, MonitorResolution[] resolutions)
        {
            int menuIndex = 0;
            foreach (var resolution in resolutions) 
            {
                var menu = parent.InsertItem(menuIndex, resolution.MenuName, resolution);
                menuIndex++;
            }
        }
    }
}
