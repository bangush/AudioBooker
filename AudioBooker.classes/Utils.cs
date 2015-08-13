using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3SplitterCommon;

namespace AudioBooker.classes {
    public static class Utils {
        public static string ToTsString(this TimeSpan ts) {
            return ts.ToString("hh\\:mm\\:ss\\.ff");
        }

        private const double SECONDS_IN_TEXT_POINT = 1;
        public static string SimulateLength(TimeSpan timeIn, TimeSpan timeOut) {
            var diff = timeOut.Subtract(timeIn);
            var points = (int)(diff.TotalSeconds / SECONDS_IN_TEXT_POINT);
            if (points < 1)
                points = 1;
            return new String('=', points);
        }

        public static void AddOrUpdate(this KeyValueConfigurationCollection dict, string key, string value) {
            if (dict.AllKeys.Any(x => x == key))
                dict.Remove(key);
            dict.Add(key, value);
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

    }

}
