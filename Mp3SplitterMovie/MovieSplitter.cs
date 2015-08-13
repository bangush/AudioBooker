//#define TMP_BREAKS

using System;
using Mp3SplitterCommon;

namespace Mp3SplitterMovie
{
	internal class MovieSplitter : AbstractSplitter
	{
		public MovieSplitter()
		{
			SimpleLog.LogIntro(XmlFilename);

			var xxx = XmlFactory.LoadFromFile<Mp3SplitterCommon.xml.Lesson>(XmlFilename);
			Mp3Composite result = null;
			var tmp = 0;
			var fileOutCount = 1;
			foreach (var line in xxx.Lines)
			{
				tmp++;
#if (TMP_BREAKS)
				if (!(tmp == 599 || tmp == 1477))
					continue;
#endif

				if (tmp % LINES_PER_MP3 == 0 || result == null)
				{
					if (result != null)
						result.Close();
					result = new Mp3Composite(String.Format(OutName2Lang, fileOutCount.ToString("D3")));
					fileOutCount++;
				}

				var time1 = formula(line.AudioTime.In) - 500;
				var time2 = formula(line.AudioTime.Out) + 1300;
				SimpleLog.Log("{0} - {1}:   {2}", time1, time2, line.Lang1);
				result.WritePieceOfSomeFile(Mp3Name2, time1 / 1000.0, time2 / 1000.0);
				result.WritePieceOfSomeFile(Mp3Name1, time1 / 1000.0, time2 / 1000.0);
				
#if (TMP_BREAKS)
				if (tmp > 2000)
					break;
#endif
			}
			if (result != null)
				result.Close();
		}

	}
}