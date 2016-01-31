using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioBooker.classes.Properties;
using Miktemk.Mp3;

namespace AudioBooker.classes {

    public static class AudiobookerMp3Utils
    {
        //public const double CutSecondsBefore = 0.1;
        //public const double CutSecondsAfter = 0.1;

        // TODO: introduce a pause between phrases for watermark
        public static void SaveMusicMp3ToSegmentedFile(string filename, XmlAudiobook meta)
        {
            // save mp3
            var result = new WavComposite(filename);
            foreach (var seg in meta.Segments) {
                var segFilename = UtilsCore.GetFullPathWithoutExtension(filename) + "/" + seg.Filename;
                var lengthSec = ((double)WavUtils.TotalLengthMillis(segFilename)) / 1000;
                result.WritePieceOfSomeFileWav(
                    segFilename,
                    Settings.Default.CutSecondsBefore,
                    lengthSec - Settings.Default.CutSecondsAfter);
            }
            result.Close();
        }

        public static void InterleaveAndSaveIlyaFrankerMp3(
            string filenameOut,
            XmlIlyaFrankAbook xmlIlyaFrankAbook,
            double? speedChange1 = null,
            double? speedChange2 = null,
            bool reverseLanguages = false,
            Action<double> updateProgress = null)
        {
            // save mp3
            var filenameWav = UtilsCore.GetFullPathWithoutExtension(filenameOut) + ".wav"; //force .wav extension
            var result = new WavComposite(filenameWav);
            int totalSentences = xmlIlyaFrankAbook.Paragraphs.SelectMany(x => x.Sentences).Count();
            int i = 0;
            var sndInter = Settings.Default.SoundEffectInter.Replace("{ExePath}", Application.StartupPath);
            var sndOpen = Settings.Default.SoundEffectOpen.Replace("{ExePath}", Application.StartupPath);
            var sndClose = Settings.Default.SoundEffectClose.Replace("{ExePath}", Application.StartupPath);
            if (updateProgress != null)
                updateProgress(0);
            foreach (var para in xmlIlyaFrankAbook.Paragraphs) {
                if (!para.Sentences.Any())
                    continue;
                foreach (var sen in para.Sentences) {
                    // A-B-A-ding
                    var segA = sen.Lang1Segments;
                    var segB = sen.Lang2Segments;
                    //var prefixA = xmlIlyaFrankAbook.Lang1Prefix;
                    //var prefixB = xmlIlyaFrankAbook.Lang2Prefix;
                    var speedChangeA = speedChange1;
                    var speedChangeB = speedChange2;
                    if (reverseLanguages) {
                        segB = sen.Lang1Segments;
                        segA = sen.Lang2Segments;
                        //prefixB = xmlIlyaFrankAbook.Lang1Prefix;
                        //prefixA = xmlIlyaFrankAbook.Lang2Prefix;
                        speedChangeB = speedChange1;
                        speedChangeA = speedChange2;
                    }
                    foreach (var seg in segA) {
                        //result.WritePieceOfSomeFile(mp3A, seg.TimeIn.TotalSeconds, seg.TimeOut.TotalSeconds);
                        result.WriteXmlWavEvent(seg, xmlIlyaFrankAbook.Lang1Prefix, xmlIlyaFrankAbook.Lang2Prefix, speedChangeA);
                    }
                    foreach (var seg in segB) {
                        //result.WritePieceOfSomeFile(mp3B, seg.TimeIn.TotalSeconds, seg.TimeOut.TotalSeconds);
                        result.WriteXmlWavEvent(seg, xmlIlyaFrankAbook.Lang1Prefix, xmlIlyaFrankAbook.Lang2Prefix, speedChangeB);
                    }
                    foreach (var seg in segA) {
                        //result.WritePieceOfSomeFile(mp3A, seg.TimeIn.TotalSeconds, seg.TimeOut.TotalSeconds);
                        result.WriteXmlWavEvent(seg, xmlIlyaFrankAbook.Lang1Prefix, xmlIlyaFrankAbook.Lang2Prefix, speedChangeA);
                    }
                    result.AppendAllOfFile(
                        sndInter,
                        (Settings.Default.SoundEffectFactor != 0) ? (double?)Settings.Default.SoundEffectFactor : null
                    );
                    i++;
                }
                // ding-AAA-ding
                result.AppendAllOfFile(
                    sndOpen,
                    (Settings.Default.SoundEffectFactor != 0) ? (double?)Settings.Default.SoundEffectFactor : null
                );
                foreach (var sen in para.Sentences) {
                    var segA1 = sen.Lang1Segments;
                    //var prefixA = xmlIlyaFrankAbook.Lang1Prefix;
                    //var prefixB = xmlIlyaFrankAbook.Lang2Prefix;
                    var speedChangeA = speedChange1;
                    if (reverseLanguages) {
                        segA1 = sen.Lang2Segments;
                        //prefixB = xmlIlyaFrankAbook.Lang1Prefix;
                        //prefixA = xmlIlyaFrankAbook.Lang2Prefix;
                        speedChangeA = speedChange2;
                    }
                    foreach (var seg in segA1) {
                        //result.WritePieceOfSomeFile(mp3A, seg.TimeIn.TotalSeconds, seg.TimeOut.TotalSeconds);
                        result.WriteXmlWavEvent(seg, xmlIlyaFrankAbook.Lang1Prefix, xmlIlyaFrankAbook.Lang2Prefix, speedChangeA);
                    }
                }
                result.AppendAllOfFile(
                    sndClose,
                    (Settings.Default.SoundEffectFactor != 0) ? (double?)Settings.Default.SoundEffectFactor : null
                );
            }
            result.Close();
        }
    }
}
