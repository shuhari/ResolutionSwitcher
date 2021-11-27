using System.Runtime.InteropServices;
using System.Text;

namespace ResolutionSwitcher.Gui
{
    public class IniFile   // revision 11
    {
        public string Path { get; private set; }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath)
        {
            Path = new FileInfo(IniPath).FullName;
        }

        public string Read(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        public void DeleteKey(string Section, string Key)
        {
            Write(Section, Key, null);
        }

        public void DeleteSection(string Section)
        {
            Write(Section, null, null);
        }

        public bool KeyExists(string Section, string Key)
        {
            return Read(Section, Key).Length > 0;
        }
    }
}
