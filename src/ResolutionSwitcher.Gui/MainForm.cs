using System.Diagnostics;

namespace ResolutionSwitcher.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ResizeRedraw = true;
            
            _settingsForm = new SettingsForm();
            _settingsForm.Owner = this;
        }

        private SettingsForm _settingsForm;

        private void MainForm_Load(object sender, EventArgs e)
        {
            var model = AppModel.Instance;
            model.Changed += Model_Changed;
            model.Load();
            OnModesChanged();

            // Hide on start up; call Hide() right once has no effect
            BeginInvoke(new MethodInvoker(delegate
            {
                Hide();
            }));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hide to tray instead of close
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            _settingsForm.Hide();
            CenterToScreen();
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            // Double click to show/hide main form
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
            // Show settings form
            Hide();
            _settingsForm.Show();
            _settingsForm.Activate();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            // Quit application
            Application.Exit();
        }

        private void Model_Changed(object sender, EventArgs e)
        {
            OnModesChanged();
        }

        /// <summary>
        /// Rebuild dynamic UI items on modes changed
        /// </summary>
        private void OnModesChanged()
        {
            var model = AppModel.Instance;

            // Rebuild menus
            var modeMenus = trayMenu.Items.OfType<ToolStripItem>()
                .Where(x => x.Tag is DisplayMode)
                .ToArray();
            foreach (var menuItems in modeMenus)
            {
                trayMenu.Items.Remove(menuItems);
            }

            int menuIndex = 0;
            AddModeMenu(model.RecommendedMode, ref menuIndex);
            AddSeparator(ref menuIndex);
            foreach (var mode in model.Modes)
            {
                AddModeMenu(mode, ref menuIndex);
            }
            AddSeparator(ref menuIndex);

            // Rebuild buttons
            var modeButtons = Controls.OfType<Button>()
                .Where(x => x.Tag is DisplayMode)
                .ToArray();
            foreach (var modeBtn in modeButtons)
            {
                Controls.Remove(modeBtn);
            }

            AddModeButton(model.RecommendedMode);
            foreach (var mode in AppModel.Instance.Modes)
            {
                AddModeButton(mode);
            }
            AdjustButtonBounds();
        }

        private void AddModeButton(DisplayMode mode)
        {
            var btn = new ModeButton { Data = mode };
            btn.Click += OnModeBtnClick;
            Controls.Add(btn);
        }

        private void AdjustButtonBounds()
        {
            // Set button bounds on resize
            var buttons = Controls.OfType<Button>()
                .Where(x => x.Tag is DisplayMode)
                .ToArray();
            foreach (var btn in buttons)
            {
                var mode = (DisplayMode)btn.Tag;
                var bounds = GetModeBounds(mode);
                btn.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }
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

        private Rectangle GetModeBounds(DisplayMode mode)
        {
            const int cellHeight = 96;
            const int margin = 8;
            int row, col;
            if (mode.Type == DisplayModeType.Recommended)
            {
                row = 0;
                col = 1;
            }
            else
            {
                row = mode.Index / 3 + 1;
                col = mode.Index % 3;
            }
            int cellWidth = ClientRectangle.Width / 3;
            var rc = new Rectangle(cellWidth * col, cellHeight * row, cellWidth, cellHeight);
            rc.Inflate(-margin, -margin);
            return rc;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 0 to set recommended mode
            // 1-9 to set defined mode
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                var model = AppModel.Instance;
                DisplayMode mode = null;
                if (e.KeyChar == '0')
                    mode = model.RecommendedMode;
                else
                {
                    var modes = model.Modes.ToArray();
                    int index = e.KeyChar - '1';
                    if (index >= 0 && index < modes.Length)
                        mode = modes[index];
                }
                if (mode != null)
                {
                    DisplayApi.CurrentMode = mode;
                }
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            AdjustButtonBounds();
        }

        private void OnModeMenu(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is DisplayMode mode)
            {
                DisplayApi.CurrentMode = mode;
            }
        }

        private void OnModeBtnClick(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is DisplayMode mode)
            {
                DisplayApi.CurrentMode = mode;
            }
        }
    }
}