using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBooker.classes.Properties;
using System.Diagnostics;
using Miktemk.Mp3;

namespace AudioBooker.classes
{
    public static class UtilsCore
    {
        public const string ExplorerExecutable = "explorer";
        private const double SECONDS_IN_TEXT_POINT = 4;

        private static string[]
            EXT_image = Settings.Default.FileExtensionsImage.Split('|');

        public static string ToBarString(this TimeSpan ts) {
            var points = (int)(ts.TotalSeconds / SECONDS_IN_TEXT_POINT);
            return (points >= 1)
                ? new String('=', points)
                : "|";
        }
        public static string SimulateLength(TimeSpan timeIn, TimeSpan timeOut) {
            var diff = timeOut.Subtract(timeIn);
            return diff.ToBarString();
        }

        public static string GetFullPathWithoutExtension(string fname) {
            var dir = Path.GetDirectoryName(fname);
            if (String.IsNullOrEmpty(fname))
                return Path.GetDirectoryName(fname);
            return dir + "/" + Path.GetFileNameWithoutExtension(fname);
        }

        public static void WriteXmlWavEvent(
            this WavComposite writeTo,
            XmlWavEvent segment,
            string prefixLang1,
            string prefixLang2,
            double? speedChange)
        {
            switch (segment.Type) {
                case WavEventType.WavRecording1:
                    writeTo.AppendAllOfFile(prefixLang1 + "/" + segment.Filename, speedChange);
                    break;
                case WavEventType.WavRecording2:
                    writeTo.AppendAllOfFile(prefixLang2 + "/" + segment.Filename, speedChange);
                    break;
                case WavEventType.Mp3Segment:
                    writeTo.WritePieceOfSomeFileMp3(prefixLang1 + "/" + segment.Filename, segment.TimeIn.TotalSeconds, segment.TimeOut.TotalSeconds, speedChange);
                    writeTo.AppendAllOfFile(prefixLang1 + "/" + segment.Filename, speedChange);
                    break;
            }
        }

        public static bool IsFilenameImage(string filename)
        {
            return IsFilenameOneOfThese(filename, EXT_image);
        }

        private static bool IsFilenameOneOfThese(string filename, string[] extensions)
        {
            if (String.IsNullOrEmpty(filename))
                return false;
            var ext = Path.GetExtension(filename).ToLower();
            return extensions.Any(e => String.Equals(e, ext, StringComparison.OrdinalIgnoreCase));
        }

        public static void OpenWinExplorerAndSelectThisFile(string filename)
        {
            string args = string.Format("/e, /select, \"{0}\"", filename);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = ExplorerExecutable;
            info.Arguments = args;
            Process.Start(info);
        }
    }

}
