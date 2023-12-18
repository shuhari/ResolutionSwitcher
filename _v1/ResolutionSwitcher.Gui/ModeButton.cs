namespace ResolutionSwitcher.Gui
{
    public class ModeButton : Button
    {
        static ModeButton()
        {
        }

        public ModeButton()
        {
            Cursor = Cursors.Hand;
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 16);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 2;
            FlatAppearance.MouseOverBackColor = Color.DarkOrange;
        }

        public DisplayMode Data
        {
            get { return Tag as DisplayMode; }
            set
            {
                Tag = value;
                Text = value.ToString();
                FlatAppearance.BorderColor = (value.Type == DisplayModeType.Recommended) ?
                    Color.Green : Color.Yellow;
            }
        }
    }
}
