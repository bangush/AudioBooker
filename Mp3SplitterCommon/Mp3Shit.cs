using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio.Wave;
//using NAudio.Wave;

namespace Mp3SplitterCommon
{
	public class Mp3Composite
	{
		private FileStream writer;
        private WaveFormat format;

		public Mp3Composite(string filename)
		{
			writer = File.Create(filename);
            format = null;
		}

		public void Close()
		{
			writer.Dispose();
		}

		public void WritePieceOfSomeFile(string srcFilename, double secondIn, double secondOut)
		{
			using (var reader = new Mp3FileReader(srcFilename))
			{
                if (format == null)
                    format = reader.WaveFormat;
				Mp3Frame frame;
				while ((frame = reader.ReadNextFrame()) != null)
				{
					if (reader.CurrentTime.TotalSeconds >= secondIn)
					{
						writer.Write(frame.RawData, 0, frame.RawData.Length);
					}
					if (reader.CurrentTime.TotalSeconds >= secondOut)
						break;
				}
			}
		}

		public void AppendAllOfFile(string srcFilename)
		{
			using (var reader = new Mp3FileReader(srcFilename))
			{
				Mp3Frame frame;
				while ((frame = reader.ReadNextFrame()) != null)
				{
					writer.Write(frame.RawData, 0, frame.RawData.Length);
				}
			}
		}
		
	}

    // misc static methods
    public static class Mp3Utils
    {
        public static long TotalLengthMillis(string srcFilename) {
            long millis = 0;
            using (var reader = new Mp3FileReader(srcFilename)) {
                Mp3Frame frame;
                // read this shit to the end
                while ((frame = reader.ReadNextFrame()) != null) {}
                millis = (long)reader.CurrentTime.TotalMilliseconds;
            }
            return millis;
        }

        public static void PlayPieceOfAFile(string srcFilename, double secondIn, double secondOut) {
            Thread ttt = new Thread(() => PlayPieceOfAFile_t(srcFilename, secondIn, secondOut));
            ttt.Start();
        }

        private static void PlayPieceOfAFile_t(string srcFilename, double secondIn, double secondOut) {
            IWavePlayer waveOutDevice = new WaveOut();
            using (var reader = new Mp3FileReader(srcFilename)) {
                waveOutDevice.Init(reader);
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null) {
                    if (reader.CurrentTime.TotalSeconds >= secondIn) {
                        break;
                        //writer.Write(frame.RawData, 0, frame.RawData.Length);
                    }
                    if (reader.CurrentTime.TotalSeconds >= secondOut)
                        break;
                }
                waveOutDevice.Play();
                int millis = (int)(1000 * (secondOut - secondIn));
                Thread.Sleep(millis);
            }
        }
    }

}
