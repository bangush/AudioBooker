using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ConsoleTests
{
    public class TestMix
    {
        public TestMix()
        {
            //WaveStream str1 = new Mp3FileReader("C:\\Users\\mtemkine\\Desktop\\snd\\guitar1.mp3");
            //WaveStream str2 = new Mp3FileReader("C:\\Users\\mtemkine\\Desktop\\snd\\molecules.mp3");
            //WaveMixerStream32 mix = new WaveMixerStream32(new [] {str1, str2}, false);

            var background = new Mp3FileReader("C:\\Users\\mtemkine\\Desktop\\snd\\ferriss.mp3");
            var message = new Mp3FileReader("C:\\Users\\mtemkine\\Desktop\\snd\\guitar1.mp3");

            var mixer = new WaveMixerStream32();
            mixer.AutoStop = true;

            var messageOffset = background.TotalTime;
            var messageOffsetted = new WaveOffsetStream(message, TimeSpan.FromSeconds(1.5), TimeSpan.Zero, message.TotalTime.Subtract(TimeSpan.FromSeconds(1)));

            var background32 = new WaveChannel32(background);
            background32.PadWithZeroes = false;
            background32.Volume = 0.9f;

            var message32 = new WaveChannel32(messageOffsetted);
            message32.PadWithZeroes = false;
            message32.Volume = 0.7f;

            var s1 = new RawSourceWaveStream(background32, new WaveFormat(8000, 16, 1));
            var s2 = new RawSourceWaveStream(message32, new WaveFormat(8000, 16, 1));
            WaveFormat targetFormat = WaveFormat.CreateIeeeFloatWaveFormat(128, 2);
            var ss1 = new WaveFormatConversionStream(targetFormat, background32);
            //var c = new WaveFormatConversionStream(WaveFormat.CreateALawFormat(8000, 1), background32);
            //var stream_background32 = new WaveFormatConversionStream(new WaveFormat(256, 32, 2), background32);
            //var stream_message32 = new WaveFormatConversionStream(new WaveFormat(256, 32, 2), message32);
            mixer.AddInputStream(s1);
            mixer.AddInputStream(s2);

            WaveFileWriter.CreateWaveFile("mycomposed.wav", new Wave32To16Stream(mixer));
        }
    }
}
