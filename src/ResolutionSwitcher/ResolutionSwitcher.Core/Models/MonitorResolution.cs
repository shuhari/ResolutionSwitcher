namespace ResolutionSwitcher.Models
{
    public class MonitorResolution
    {
        public int Width { get; internal set; }

        public int Height { get; internal set; }
        
        public int BitsPerPixel { get; internal set; }

        public int DisplayFrequency { get; internal set; }

        public int DisplayFlags { get; internal set; }

        public string MenuName =>
            string.Format("{0} * {1}, {2} bit Colors, {3}Hz", Width, Height, BitsPerPixel, DisplayFrequency);
    }
}
