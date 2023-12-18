namespace ResolutionSwitcher.Models
{
    public class MonitorInfo
    {
        public string Name { get; internal set; } = string.Empty;

        public string DisplayName { get; internal set; } = string.Empty;

        public string DeviceId { get; internal set; } = string.Empty;

        public string DeviceKey { get; internal set; } = string.Empty;

        public MonitorResolution[] Resolutions { get; internal set; } = new MonitorResolution[0];
    }
}
