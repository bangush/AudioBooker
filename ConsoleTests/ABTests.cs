using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBooker.classes;
using Mp3SplitterCommon;

namespace ConsoleTests
{
    public class ABTests
    {
        public static void WriteFrankerBook(string fnameXml, string fnameOut, double? speed1, double? speed2)
        {
            var xml = XmlFactory.LoadFromFile<XmlIlyaFrankAbook>(fnameXml);
            AudiobookerMp3Utils.InterleaveAndSaveIlyaFrankerMp3(fnameOut, xml, speed1, speed2, false);
        }

        public static void oldTests() {
            //var fnameMp3 = "../../../AudioBooker/bin/Debug/pars_vite.mp3";
            var fnameMp3 = @"C:\Users\mtemkine\Desktop\snd\test.mp3";
            var fnameXml = fnameMp3 + ".xml";
            var xml = XmlFactory.LoadFromFile<XmlAudiobook>(fnameXml);

            const double CutSecondsBefore = 0.1;
            const double CutSecondsAfter = 0.1;
            var firstOffset = 0; // 0.79; // xml.Segments.First().TimeIn.TotalSeconds;

            var result = new WavComposite(@"C:\Users\mtemkine\Desktop\snd\test_cut.wav");
            foreach (var seg in xml.Segments) {
                //result.WritePieceOfSomeFileWav(fnameMp3, seg.TimeIn.TotalSeconds + CutSecondsBefore - firstOffset, seg.TimeOut.TotalSeconds - CutSecondsAfter - firstOffset);
                //result.AppendAllOfFile("C:\\Users\\mtemkine\\Desktop\\snd\\guitar1.mp3");
                result.AppendAllOfFileWav("C:\\Users\\mtemkine\\Desktop\\snd\\ilyafrank_inter.mp3");
                //result.AppendAllOfFile("C:\\Users\\mtemkine\\Desktop\\snd\\ilyafrank_res_open.mp3");
                //result.AppendAllOfFile("C:\\Users\\mtemkine\\Desktop\\snd\\ilyafrank_res_close.mp3");
            }
            result.Close();

        }
    }
}
