//#define LogTest

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mp3SplitterCommon;
using Mp3SplitterSimple.Properties;

namespace Mp3SplitterSimple
{
	public class SimpleSplitter
	{
        public SimpleSplitter(string filename)
		{
            var ddd = Path.GetFileNameWithoutExtension(filename);
            var splitDir = Path.Combine(Path.GetDirectoryName(filename), ddd);
            if (!Directory.Exists(splitDir))
                Directory.CreateDirectory(splitDir);

            var totalMillis = Mp3Utils.TotalLengthMillis(filename);

            TimeSpan totalTS = TimeSpan.FromMilliseconds(totalMillis);
            Console.WriteLine("total time: " + totalTS);

            var step = (long)Settings.Default.SplitLength.TotalMilliseconds;
            var i = 0;
            for (long ttt = 0; ttt < totalMillis; ttt += step) {
                i++;

                TimeSpan curTS = TimeSpan.FromMilliseconds(ttt);
                Console.WriteLine("current segment: " + curTS);

                var outName = Path.Combine(splitDir, String.Format("{0}_{1}.mp3", ddd, i.ToString("D4")));
                var result = new Mp3Composite(outName);
                result.WritePieceOfSomeFile(filename, ttt / 1000, (ttt + step) / 1000);
                result.Close();
            }
		}

	}
}
