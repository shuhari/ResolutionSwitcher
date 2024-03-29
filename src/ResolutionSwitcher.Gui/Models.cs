﻿using System.Text.RegularExpressions;

namespace ResolutionSwitcher.Gui
{
    public class SelectOption
    {
        public SelectOption(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }
        public int Value { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AppModel
    {
        private static AppModel _instance = null;

        public static AppModel Instance
        {
            get
            {
                _instance = _instance ?? new AppModel();
                return _instance;
            }
        }

        private AppModel()
        {
            Resolutions = DisplayApi.GetResolutions()
                .OrderBy(x => x.Width)
                .ThenBy(x => x.Height)
                .Select(ToOption)
                .ToArray();
            var orientationValues = new[] {
                DisplayOrientation.Default,
                DisplayOrientation.Rotate90,
                DisplayOrientation.Rotate180,
                DisplayOrientation.Rotate270
            };
            Orientations = orientationValues.Select(x => new SelectOption(Names.OfOrientation(x), (int)x)).ToArray();
            var scaleValues = new[]
            {
                ScaleFactor.Scale100,
                ScaleFactor.Scale120,
                ScaleFactor.Scale125,
                ScaleFactor.Scale140,
                ScaleFactor.Scale150,
                ScaleFactor.Scale160,
                ScaleFactor.Scale175,
                ScaleFactor.Scale180,
                ScaleFactor.Scale200,
                ScaleFactor.Scale225,
                ScaleFactor.Scale250,
                ScaleFactor.Scale300,
                ScaleFactor.Scale350,
                ScaleFactor.Scale400,
                ScaleFactor.Scale450,
                ScaleFactor.Scale500,
            };
            Scales = scaleValues.Select(x => new SelectOption(Names.OfScale(x), (int)x)).ToArray();
            RecommendedMode = DisplayApi.GetRecommendedMode();
            CurrentMode = DisplayApi.CurrentMode;
            _modes = new List<DisplayMode>();
        }

        public SelectOption[] Resolutions { get; private set; }

        public SelectOption[] Orientations { get; private set; }

        public SelectOption[] Scales { get; private set; }

        public DisplayMode RecommendedMode { get; private set; }

        public DisplayMode CurrentMode { get; private set; }

        private readonly List<DisplayMode> _modes;

        public IEnumerable<DisplayMode> Modes => _modes.AsReadOnly();

        public event EventHandler Changed;

        public string ConfigPath
        {
            get
            {
                var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                const string companyName = "shuhari.dev";
                const string appName = "ResolutionSwitcher";
                const string fileName = "config.ini";
                return Path.Combine(dirPath, companyName, appName, fileName);
            }
        }

        private static SelectOption ToOption(Size resolution)
        {
            string name = string.Format("{0} * {1}", resolution.Width, resolution.Height);
            int value = (resolution.Width << 16) + (resolution.Height & 0xFFFF);
            return new SelectOption(name, value);
        }

        private void InvokeChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void Update()
        {
            RecommendedMode = DisplayApi.GetRecommendedMode();
            CurrentMode = DisplayApi.CurrentMode;
        }

        public bool Add(DisplayMode mode)
        {
            if (_modes.Contains(mode))
                return false;
            _modes.Add(mode);
            UpdateIndexes();
            InvokeChanged();
            return true;
        }

        public bool Remove(DisplayMode mode)
        {
            if (_modes.Remove(mode))
            {
                UpdateIndexes();
                InvokeChanged();
                return true;
            }
            return false;
        }

        private void UpdateIndexes()
        {
            for (int i=0; i<_modes.Count; i++)
            {
                _modes[i].Index = i;
            }
        }

        public void Clear()
        {
            if (_modes.Count > 0)
            {
                _modes.Clear();
                InvokeChanged();
            }
        }

        public bool MoveUp(int index)
        {
            if (index > 0 && index < _modes.Count)
            {
                var target = _modes[index - 1];
                _modes[index - 1] = _modes[index];
                _modes[index] = target;
                UpdateIndexes();
                InvokeChanged();
                return true;
            }
            return false;
        }

        public bool MoveDown(int index)
        {
            if (index >= 0 && index < _modes.Count - 1)
            {
                var target = _modes[index + 1];
                _modes[index + 1] = _modes[index];
                _modes[index] = target;
                UpdateIndexes();
                InvokeChanged();
                return true;
            }
            return false;
        }

        public void Load()
        {
            var filePath = ConfigPath;
            if (File.Exists(filePath))
            {
                var ini = new IniFile(filePath);

                _modes.Clear();
                int index = 0;
                while (true)
                {
                    string key = string.Format("Mode{0}", index);
                    string value = ini.Read("Modes", key);
                    if (!string.IsNullOrWhiteSpace(value) && TryParseMode(value, out DisplayMode mode))
                    {
                        _modes.Add(mode);
                        index++;
                    }
                    else
                        break;
                }
                UpdateIndexes();
            }
        }

        private bool TryParseMode(string value, out DisplayMode mode)
        {
            mode = null;
            var re = new Regex(@"(\d+),(\d+),(\d+),(\d+)");
            var m = re.Match(value);
            if (!m.Success)
                return false;
            int width = int.Parse(m.Groups[1].Value);
            int height = int.Parse(m.Groups[2].Value);
            int orientation = int.Parse(m.Groups[3].Value);
            int scale = int.Parse(m.Groups[4].Value);
            mode = new DisplayMode(DisplayModeType.Custom, 0, new Size(width, height), (DisplayOrientation)orientation, (ScaleFactor)scale);
            return true;
        }

        private string GetModeString(DisplayMode mode)
        {
            return string.Format("{0},{1},{2},{3}",
                mode.Resolution.Width, mode.Resolution.Height,
                (int)mode.Orientation, (int)mode.Scale);
        }

        public void Save()
        {
            var filePath = ConfigPath;
            var fileDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            var ini = new IniFile(filePath);
            ini.DeleteSection("Modes");
            int index = 0;
            foreach (var mode in _modes)
            {
                string key = string.Format("Mode{0}", index);
                ini.Write("Modes", key, GetModeString(mode));
                index++;
            }
        }
    }
}
