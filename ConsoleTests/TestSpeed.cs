using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3SplitterCommon;
using NAudio.Wave;

namespace ConsoleTests
{
    public class TestSpeed
    {
        public TestSpeed()
        {
            //WaveStream str1 = new Mp3FileReader("C:\\Users\\mtemkine\\Desktop\\snd\\guitar1.mp3");
            //WaveStream str2 = new Mp3FileReader("C:\\Users\\mtemkine\\Desktop\\snd\\molecules.mp3");
            //WaveMixerStream32 mix = new WaveMixerStream32(new [] {str1, str2}, false);

            var mp3Filename = @"C:\WS\jmerde\trunk\_VisualStudio\AudioBooker\IlyaFranker\Content\snd\ilyafrank_open.mp3";

            WavComposite writer = new WavComposite("../../wawa.wav");

            writer.AppendAllOfFile(mp3Filename, null);
            writer.AppendAllOfFile(mp3Filename, 2);
            writer.AppendAllOfFile(mp3Filename, 0.5);
            writer.Close();

            //format = readerWave.WaveFormat;
            //writer = new WaveFileWriter(destFilename, format);
            //var wave = new Mp3FileReader(mp3Filename);

            using (var reader = new Mp3FileReader(mp3Filename))
            using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
            using (var downsampledStream = new WaveFormatConversionStream(new WaveFormat(
                (int)(pcmStream.WaveFormat.SampleRate * 1.5),
                reader.WaveFormat.BitsPerSample,
                reader.WaveFormat.Channels), pcmStream))
            {

                WaveFileWriter.CreateWaveFile("../../xello.wav", downsampledStream);
            }

        }
        /*
        private void SpeedUp(WaveStream www, double PlaybackRate)
        {
            WaveFileWriter writer = new WaveFileWriter("xello.wav", format);
            var provider = www.ToSampleProvider();
            WaveProvider32 wp = new WaveProvider32();
            //WaveProvider WaveOutProvider;
            WaveOut wo = new WaveOut();
            
            if (WaveOutProvider != null) {
                if (Math.Abs(PlaybackRate - 1) > double.Epsilon) {
                    //resample audio if playback speed changed
                    var newRate = Convert.ToInt32(_waveProvider.WaveFormat.SampleRate / PlaybackRate);
                    var wf = new WaveFormat(newRate, 16, _waveProvider.WaveFormat.Channels);
                    var resampleInputMemoryStream = new MemoryStream(data) { Position = 0 };

                    WaveStream ws = new RawSourceWaveStream(resampleInputMemoryStream, _waveProvider.WaveFormat);
                    var wfcs = new WaveFormatConversionStream(wf, ws) { Position = 0 };
                    var b = new byte[ws.WaveFormat.AverageBytesPerSecond];

                    int bo = wfcs.Read(b, 0, ws.WaveFormat.AverageBytesPerSecond);
                    while (bo > 0) {
                        WaveOutProvider.AddSamples(b, 0, bo);
                        bo = wfcs.Read(b, 0, ws.WaveFormat.AverageBytesPerSecond);
                    }
                    wfcs.Dispose();
                    ws.Dispose();

                }
                else {
                    WaveOutProvider.AddSamples(data, 0, data.Length);
                }

            }
        }*/
    }
}
