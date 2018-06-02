using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.ShellExtensions;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace EffectSome
{
    class FileMetadata
    {
        public static double GetBPM(string fileName)
        {
            ShellFile file = ShellFile.FromFilePath(fileName);
            return Convert.ToDouble(file.Properties.System.Music.BeatsPerMinute.Value);
        }
        public static void SetBPM(string fileName, double value)
        {
            ShellFile file = ShellFile.FromFilePath(fileName);
            bool set = false;
            while (set == false)
            {
                try
                {
                    file.Properties.System.Music.BeatsPerMinute.Value = value.ToString();
                    set = true;
                }
                catch { }
            }
        }
        public static void SetBPM(string fileName, string value)
        {
            ShellFile file = ShellFile.FromFilePath(fileName);
            file.Properties.System.Music.BeatsPerMinute.Value = value;
        }
        public static double GetSongLength(string fileName)
        {
            ShellFile file = ShellFile.FromFilePath(fileName);
            return Convert.ToDouble(file.Properties.System.Media.Duration.Value) / 10000000;
        }
    }
}
