using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace Mp3SplitterPlat
{
	public class Mp3Composite
	{
		private FileStream writer;
		public Mp3Composite(string filename)
		{
			writer = File.Create(filename);
		}

		public void Close()
		{
			writer.Dispose();
		} 

		public void WritePieceOfSomeFile(string srcFilename, double secondIn, double secondOut)
		{
			using (var reader = new Mp3FileReader(srcFilename))
			{
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
	}














	// --------------- junk

	//public Program()
	//    {
	//        var mp3Path = @"C:\Users\mtemkine.TFO\Desktop\hall\i_002.mp3";
	//        var splitLength = 2.5; // seconds

	//        var mp3Dir = Path.GetDirectoryName(mp3Path);
	//        var mp3File = Path.GetFileName(mp3Path);
	//        var splitDir = Path.Combine(mp3Dir, Path.GetFileNameWithoutExtension(mp3Path));
	//        Directory.CreateDirectory(splitDir);

	//        int splitI = 0;
	//        int secsOffset = 0;

	//        using (var reader = new Mp3FileReader(mp3Path))
	//        {
	//            FileStream writer = null;
	//            Action createWriter = new Action(() =>
	//            {
	//                writer = File.Create(Path.Combine(splitDir, Path.ChangeExtension(mp3File, (++splitI).ToString("D4") + ".mp3")));
	//            });

	//            Mp3Frame frame;
	//            while ((frame = reader.ReadNextFrame()) != null)
	//            {
	//                if (writer == null) createWriter();

	//                if ((int)reader.CurrentTime.TotalSeconds - secsOffset >= splitLength)
	//                {
	//                    // time for a new file
	//                    writer.Dispose();
	//                    createWriter();
	//                    secsOffset = (int)reader.CurrentTime.TotalSeconds;
	//                }

	//                writer.Write(frame.RawData, 0, frame.RawData.Length);
	//            }

	//            if (writer != null) writer.Dispose();
	//        }
	//    }
}
