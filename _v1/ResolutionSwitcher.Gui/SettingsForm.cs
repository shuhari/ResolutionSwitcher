namespace ResolutionSwitcher.Gui
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void SettingsForm_Activated(object sender, EventArgs e)
        {
            CenterToScreen();

            var model = AppModel.Instance;
            model.Update();

            lblRecommended.Text = model.RecommendedMode.ToString();
            lblCurrent.Text = model.CurrentMode.ToString();

            FillCombo(cboResolution, model.Resolutions);
            FillCombo(cboOrientation, model.Orientations);
            FillCombo(cboScale, model.Scales);
            OnModelChanged(model);
        }

        private static void FillCombo(ComboBox cbo, SelectOption[] options)
        {
            cbo.Items.Clear();
            foreach (var option in options) 
            {
                int index = cbo.Items.Add(option);
            }
            if (cbo.Items.Count > 0)
                cbo.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboResolution.SelectedIndex < 0 || cboOrientation.SelectedIndex < 0 ||
                cboScale.SelectedIndex < 0)
                return;
            var sizeValue = GetSelectedValue(cboResolution);
            int width = sizeValue >> 16;
            int height = sizeValue & 0xFFFF;
            var orientation = (DisplayOrientation)GetSelectedValue(cboOrientation);
            var scale = (ScaleFactor)GetSelectedValue(cboScale);
            var mode = new DisplayMode(DisplayModeType.Custom, 0, new Size(width, height), orientation, scale);
            
            var model = AppModel.Instance;
            if (model.Add(mode))
            {
                OnModelChanged(model);
            }
        }

        private int GetSelectedValue(ComboBox cbo)
        {
            var item = (SelectOption)cbo.SelectedItem;
            return item.Value;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lsvModes.SelectedItems.Count > 0)
            {
                var mode = (DisplayMode)lsvModes.SelectedItems[0].Tag;
                var model = AppModel.Instance;
                if (model.Remove(mode))
                {
                    OnModelChanged(model);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lsvModes.Items.Count > 0)
            {
                if (MessageBox.Show("Sure to delete all modes？", 
                    "Confirm clear",
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    var model = AppModel.Instance;
                    model.Clear();
                    OnModelChanged(model);
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            var model = AppModel.Instance;
            if (lsvModes.SelectedIndices.Count > 0)
            {
                int index = lsvModes.SelectedIndices[0];
                if (model.MoveUp(index))
                {
                    OnModelChanged(model);
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            var model = AppModel.Instance;
            if (lsvModes.SelectedIndices.Count > 0)
            {
                int index = lsvModes.SelectedIndices[0];
                if (model.MoveDown(index))
                {
                    OnModelChanged(model);
                }
            }
        }

        private void OnModelChanged(AppModel model)
        {
            model.Save();
            lsvModes.Items.Clear();
            foreach (var mode in model.Modes)
            {
                var item = lsvModes.Items.Add(Names.OfResolution(mode.Resolution));
                item.SubItems.Add(Names.OfOrientation(mode.Orientation));
                item.SubItems.Add(Names.OfScale(mode.Scale));
                item.Tag = mode;
            }
        }
    }
}
