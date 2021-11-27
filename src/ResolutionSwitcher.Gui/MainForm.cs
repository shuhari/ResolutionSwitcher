using System.Diagnostics;

namespace ResolutionSwitcher.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private SettingsForm _settingsForm = null;

        private void MainForm_Load(object sender, EventArgs e)
        {
            var model = AppModel.Instance;
            model.Changed += Model_Changed;
            model.Load();

            // Hide on start up
            BeginInvoke(new MethodInvoker(delegate
            {
                Hide();
            }));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (_settingsForm != null)
            {
                _settingsForm.Hide();
            }
            CenterToScreen();
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            if (Visible)
            {
                Hide();
            }
            else
            {
                ShowInTaskbar = true;
                Show();
                Activate();
            }
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            _settingsForm = _settingsForm ?? new SettingsForm();
            _settingsForm.Show();
            _settingsForm.Activate();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Model_Changed(object sender, EventArgs e)
        {
            RebuildMenu();
        }

        private void RebuildMenu()
        {
            var toRemove = trayMenu.Items.OfType<ToolStripItem>()
                .Where(x => x.Tag is DisplayMode)
                .ToArray();
            foreach (var removeItem in toRemove)
            {
                trayMenu.Items.Remove(removeItem);
            }

            int index = 0;
            var model = AppModel.Instance;
            model.Save();
            AddModeMenu(model.RecommendedMode, ref index);
            AddSeparator(ref index);
            foreach (var mode in model.Modes)
            {
                AddModeMenu(mode, ref index);
            }
            AddSeparator(ref index);
        }

        private void AddModeMenu(DisplayMode mode, ref int index)
        {
            var item = new ToolStripMenuItem { Text = mode.ToString(), Tag = mode };
            item.Click += OnModeMenu;
            trayMenu.Items.Insert(index, item);
            index++;
        }

        private void AddSeparator(ref int index)
        {
            var item = new ToolStripSeparator() { Tag = AppModel.Instance.RecommendedMode };
            trayMenu.Items.Insert(index, item);
            index++;
        }

        private void OnModeMenu(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is DisplayMode mode)
            {
                DisplayApi.SetCurrentMode(mode);
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.Black, ClientRectangle);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                int index = e.KeyChar - '0';
                var modes = AppModel.Instance.Modes.ToArray();
                if (index >= 0 && index <= modes.Length)
                {
                    DisplayMode mode = (index == 0) ? AppModel.Instance.RecommendedMode :
                        modes[index - 1];
                    DisplayApi.SetCurrentMode(mode);
                }
            }
        }
    }
}