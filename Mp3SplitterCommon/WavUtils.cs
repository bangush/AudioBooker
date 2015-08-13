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
    public class WavComposite
    {
        //private FileStream writer;
        private string destFilename;
        private WaveFileWriter writer;
        private WaveFormat format;

        public WavComposite(string filename)
		{
            destFilename = filename;
		}

		public void Close()
		{
			writer.Dispose();
		}

        public void WritePieceOfSomeFileWav(
            string srcFilename,
            double secondIn,
            double secondOut,
            double? speedChange = null)
        {
            using (var reader = new WaveFileReader(srcFilename)) {
                WritePieceOfSomeFile_stream(reader, secondIn, secondOut, speedChange);
            }
        }
        public void WritePieceOfSomeFileMp3(
            string srcFilename,
            double secondIn,
            double secondOut,
            double? speedChange = null)
		{
            using (var reader = new Mp3FileReader(srcFilename)) {
                WritePieceOfSomeFile_stream(reader, secondIn, secondOut, speedChange);
            }
		}
        public void AppendAllOfFileMp3(string srcFilename, double? speedChange = null) {
            //using (var readerMp3 = new Mp3FileReader(srcFilename))
            //using (var PCM = WaveFormatConversionStream.CreatePcmStream(readerMp3))
            //using (var readerWave2 = new WaveChannel32(PCM))
            //using (var downsampledStream = new WaveFormatConversionStream(new WaveFormat(8000, readerWave2.WaveFormat.BitsPerSample, readerWave2.WaveFormat.Channels), readerWave2))
            //using (var readerWave = new WaveFormatConversionStream(format, downsampledStream))
            using (var readerMp3 = new Mp3FileReader(srcFilename)) {
                AppendAllOfFile_stream(readerMp3, speedChange);
            }
        }
        public void AppendAllOfFileWav(string srcFilename, double? speedChange = null) {
            using (var readerWav = new WaveFileReader(srcFilename)) {
                AppendAllOfFile_stream(readerWav, speedChange);
            }
        }
        /// <summary>
        /// Automatically detects how to decode the file based on extension
        /// </summary>
        public void AppendAllOfFile(string srcFilename, double? speedChange=null) {
            if (".mp3".Equals(Path.GetExtension(srcFilename), StringComparison.OrdinalIgnoreCase))
                AppendAllOfFileMp3(srcFilename, speedChange);
            else
                AppendAllOfFileWav(srcFilename, speedChange);
        }

        #region privates

        // TODO: make this shit more presice
        private void WritePieceOfSomeFile_stream(WaveStream readerWave, double secondIn, double secondOut, double? speedChange)
        {
            InitWriterIfNull(readerWave);
            speedChange = speedChange ?? 1;
            using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(readerWave))
            using (var downsampledStream = new WaveFormatConversionStream(new WaveFormat(
                (int)(format.SampleRate / speedChange),
                format.BitsPerSample,
                format.Channels), pcmStream))
            {
                var sss = pcmStream;

                const int readThisManyBytes = 4000; // 16384;
                byte[] buffer = new byte[readThisManyBytes];
                int bufferL = 0;

                while (sss.Position < sss.Length) {
                    bufferL = sss.Read(buffer, 0, readThisManyBytes);
                    if (sss.CurrentTime.TotalSeconds >= secondIn) {
                        writer.Write(buffer, 0, bufferL);
                    }
                    if (sss.CurrentTime.TotalSeconds >= secondOut)
                        break;
                    writer.Write(buffer, 0, bufferL);
                }
                //while (readerWave.Position < readerWave.Length) {
                //    bufferL = readerWave.Read(buffer, 0, readThisManyBytes);
                //    //TODO: volume chart trimming
                //    //for (int i = 0; i < read / 4; i++) {
                //    //    chart1.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                //    //}
                //    //Console.WriteLine("{0} {1}", readerWave.CurrentTime, (readerWave.CurrentTime.TotalSeconds >= secondIn) ? "xxxxxxxxxx" : "");
                //    if (readerWave.CurrentTime.TotalSeconds >= secondIn) {
                //        //writer.Write(frame.RawData, 0, frame.RawData.Length);
                //        writer.Write(buffer, 0, bufferL);
                //    }
                //    if (readerWave.CurrentTime.TotalSeconds >= secondOut)
                //        break;
                //}
            }
        }

        private void AppendAllOfFile_stream(WaveStream readerWave, double? speedChange)
        {
            InitWriterIfNull(readerWave);
            speedChange = speedChange ?? 1;
            using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(readerWave))
            using (var downsampledStream = new WaveFormatConversionStream(new WaveFormat(
                (int)(format.SampleRate / speedChange),
                format.BitsPerSample,
                format.Channels), pcmStream)) 
            {
                const int readThisManyBytes = 16384;
                byte[] buffer = new byte[readThisManyBytes];
                int bufferL = 0;
                //ByteBufferCompressor bbbb = new ByteBufferCompressor(readThisManyBytes, speedChange);
                while (downsampledStream.Position < downsampledStream.Length) {
                    bufferL = downsampledStream.Read(buffer, 0, readThisManyBytes);
                    if (bufferL == 0)
                        break;
                    writer.Write(buffer, 0, bufferL);
                    //var buffer2L = bbbb.Transform(buffer, bufferL);
                    //writer.Write(bbbb.GetNewBuffer(), 0, buffer2L);
                }
            }
        }

        private void InitWriterIfNull(WaveStream readerWave) {
            if (writer == null) {
                format = readerWave.WaveFormat;
                writer = new WaveFileWriter(destFilename, format);
            }
        }

        #endregion

    }

    // misc static methods
    public static class WavUtils {
        public static long TotalLengthMillis(string srcFilename) {
            long millis = 0;
            using (var reader = new WaveFileReader(srcFilename)) {
                millis = (long)reader.TotalTime.TotalMilliseconds;
            }
            return millis;
        }

        public static void PlayAllOfFile(string srcFilename, Action donePlaying = null) {
            Thread ttt = new Thread(() => {
                IWavePlayer waveOutDevice = new WaveOut();
                using (var reader = new WaveFileReader(srcFilename)) {
                    waveOutDevice.Init(reader);
                    waveOutDevice.Play();
                    Thread.Sleep(reader.TotalTime);
                }
                if (donePlaying != null)
                    donePlaying();
            });
            ttt.Start();
        }

        // TODO: so far we never had to play pieces of wav files
        /*public static void PlayPieceOfAFile(string srcFilename, double secondIn, double secondOut) {
            Thread ttt = new Thread(() => PlayPieceOfAFile_t(srcFilename, secondIn, secondOut));
            ttt.Start();
        }

        private static void PlayPieceOfAFile_t(string srcFilename, double secondIn, double secondOut) {
            IWavePlayer waveOutDevice = new WaveOut();
            using (var reader = new WaveFileReader(srcFilename)) {
                waveOutDevice.Init(reader);
                //Mp3Frame frame;
                //while ((frame = reader.ReadNextFrame()) != null) {
                //    if (reader.CurrentTime.TotalSeconds >= secondIn) {
                //        break;
                //        //writer.Write(frame.RawData, 0, frame.RawData.Length);
                //    }
                //    if (reader.CurrentTime.TotalSeconds >= secondOut)
                //        break;
                //}
                waveOutDevice.Play();
                int millis = (int)(1000 * (secondOut - secondIn));
                Thread.Sleep(millis);
            }
        }*/
    }

}
