using System.Diagnostics;

namespace ResolutionSwitcher.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.ResizeRedraw = true;

            _borderPen = new Pen(Color.Green, 4);
            _recBorderPen = new Pen(Color.Blue, 4);
            _textFont = new Font("Tahoma", 16);
            _textBrush = new SolidBrush(Color.White);
            _textFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

        }

        private SettingsForm _settingsForm = null;
        private Pen _borderPen;
        private Pen _recBorderPen;
        private Brush _textBrush;
        private StringFormat _textFormat;
        private Font _textFont;

        private void MainForm_Load(object sender, EventArgs e)
        {
            var model = AppModel.Instance;
            model.Changed += Model_Changed;
            model.Load();
            RebuildMenu();

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

            var recMode = AppModel.Instance.RecommendedMode;
            var modes = AppModel.Instance.Modes.ToArray();
            DrawMode(g, DisplayModeType.Recommended, 0, recMode);
            for (int i=0; i<modes.Length; i++)
            {
                DrawMode(g, DisplayModeType.Custom, i, modes[i]);
            }
        }

        private Rectangle GetModeBounds(DisplayModeType type, int index)
        {
            const int cellHeight = 80;
            const int margin = 8;
            int row, col;
            if (type == DisplayModeType.Recommended)
            {
                row = 0;
                col = 1;
            }
            else
            {
                row = index / 3 + 1;
                col = index % 3;
            }
            int cellWidth = ClientRectangle.Width / 3;
            var rc = new Rectangle(cellWidth * col, cellHeight * row, cellWidth, cellHeight);
            rc.Inflate(-margin, -margin);
            return rc;
        }

        private void DrawMode(Graphics g, DisplayModeType type, int index, DisplayMode mode)
        {
            const int cornerRadius = 8;
            Rectangle rc = GetModeBounds(type, index);
            string text = string.Format("{0}. {1}", index + 1, mode);
            var pen = type == DisplayModeType.Recommended ? _recBorderPen : _borderPen;
            g.DrawRoundedRectangle(pen, rc, cornerRadius);
            g.DrawString(text, _textFont, _textBrush, rc, _textFormat);
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

        private void MainForm_Click(object sender, EventArgs e)
        {
            var pt = PointToClient(Cursor.Position);
            var mode = GetTargetMode(pt);
            if (mode != null)
            {
                DisplayApi.SetCurrentMode(mode);
            }
        }

        private DisplayMode GetTargetMode(Point pt)
        {
            var model = AppModel.Instance;
            var rc = GetModeBounds(DisplayModeType.Recommended, 0);
            if (rc.Contains(pt))
                return model.RecommendedMode;
            var modes = model.Modes.ToArray();
            for (int i=0; i<modes.Length; i++)
            {
                rc = GetModeBounds(DisplayModeType.Custom, i);
                if (rc.Contains(pt))
                    return modes[i];
            }
            return null;
        }
    }
}