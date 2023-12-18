using System.Runtime.InteropServices;
using ResolutionSwitcher.Models;
using static ResolutionSwitcher.Runtime.Interop.WinApi;

namespace ResolutionSwitcher.Runtime.Interop
{
    public static class DisplayService
    {
        public static MonitorInfo[] EnumMonitors()
        {
            var monitors = new List<MonitorInfo>();

            uint deviceNum = 0;
            var dd = new DISPLAY_DEVICE();
            dd.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));
            while (EnumDisplayDevices(null, deviceNum, ref dd, 0))
            {
                deviceNum++;
                if (!dd.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                    continue;
                var ddMonitor = new DISPLAY_DEVICE();
                ddMonitor.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));
                // Assume 1 device has only 1 monitor
                uint monitorNum = 0;
                if (!EnumDisplayDevices(dd.DeviceName, monitorNum, ref ddMonitor, 0))
                    continue;

                var resolutions = new List<MonitorResolution>();
                int modeNum = 0;
                var devMode = new DEVMODE();
                while (EnumDisplaySettings(dd.DeviceName, modeNum, ref devMode))
                {
                    modeNum++;
                    if (devMode.dmDisplayFixedOutput != 0)
                        continue;
                    var resolution = CreateMonitorResolution(devMode);
                    resolutions.Add(resolution);
                }
                // monitorNum++;

                var monitor = CreateMonitorInfo(dd, resolutions);
                monitors.Add(monitor);

            }

            return monitors.ToArray();
        }

        static MonitorInfo CreateMonitorInfo(DISPLAY_DEVICE dd, 
            IEnumerable<MonitorResolution> resolutions)
        {
            return new MonitorInfo
            {
                Name = dd.DeviceName,
                DisplayName = dd.DeviceString,
                DeviceId = dd.DeviceID,
                DeviceKey = dd.DeviceKey,
                Resolutions = resolutions.ToArray(),
            };
        }

        static MonitorResolution CreateMonitorResolution(DEVMODE dm)
        {
            return new MonitorResolution
            {
                Width = dm.dmPelsWidth,
                Height = dm.dmPelsHeight,
                BitsPerPixel = dm.dmBitsPerPel,
                DisplayFrequency = dm.dmDisplayFrequency,
                DisplayFlags = dm.dmDisplayFlags,
            };
        }

        public static string[] Dump()
        {
            var lines = new List<string>();

            DISPLAY_DEVICE dd = new DISPLAY_DEVICE();
            dd.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));

            uint deviceNum = 0;
            dd.StateFlags = DisplayDeviceStateFlags.AttachedToDesktop;
            while (EnumDisplayDevices(null, deviceNum, ref dd, 0))
            {
                DumpDevice(lines, dd, 0);
                DISPLAY_DEVICE newdd = new DISPLAY_DEVICE();
                newdd.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));
                uint monitorNum = 0;
                while (EnumDisplayDevices(dd.DeviceName, monitorNum, ref newdd, 0))
                {
                    DumpDevice(lines, newdd, 4);
                    int modeNum = 0;
                    DEVMODE devMode = new DEVMODE();
                    while (EnumDisplaySettings(dd.DeviceName, modeNum, ref devMode))
                    {
                        DumpResolution(lines, modeNum, devMode, 4);
                        modeNum++;
                    }
                    monitorNum++;
                }
                deviceNum++;
            }

            return lines.ToArray();
        }

        static void DumpDevice(List<string> lines, DISPLAY_DEVICE dd, int indent)
        {
            var prefix = new string(' ', indent);
            lines.Add($"{prefix}Device Name: {dd.DeviceName}");
            lines.Add($"{prefix}Device String: {dd.DeviceString}");
            lines.Add($"{prefix}State Flags: {dd.StateFlags}");
            lines.Add($"{prefix}DeviceID: {dd.DeviceID}");
            lines.Add($"{prefix}DeviceKey: {dd.DeviceKey}");
        }
        
        static void DumpResolution(List<string> lines, int modeNum, DEVMODE dm, int indent)
        {
            var prefix = new string(' ', indent);
            var line = string.Format("{0}Mode {1}: {2} x {3}, BitsPerPel: {4}, Frequency: {5}, Orientation: {6}, Flags: {7}",
                prefix, modeNum, 
                dm.dmPelsWidth, dm.dmPelsHeight,
                dm.dmBitsPerPel, dm.dmDisplayFrequency,
                dm.dmDisplayOrientation, dm.dmDisplayFlags);
            lines.Add(line);
        }
    }
}
