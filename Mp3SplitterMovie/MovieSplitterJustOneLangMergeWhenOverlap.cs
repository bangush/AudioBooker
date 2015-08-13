//#define TMP_BREAKS

using System;
using Mp3SplitterCommon;

namespace Mp3SplitterMovie
{
	internal class MovieSplitterJustOneLangMergeWhenOverlap : AbstractSplitter
	{
#if (TMP_BREAKS)
		new const int LINES_PER_MP3 = 400000;
#endif

		public MovieSplitterJustOneLangMergeWhenOverlap()
		{
			SimpleLog.LogIntro(XmlFilename);

			var xxx = XmlFactory.LoadFromFile<Mp3SplitterCommon.xml.Lesson>(XmlFilename);
			Mp3Composite result = null;
			var i = 0;
			var fileOutCount = 1;
			double prevTime2 = -500;
			foreach (var line in xxx.Lines)
			{
				i++;

#if (TMP_BREAKS)
				// 1 = Pero sólo hemos hablado de Rusia y España
				// 14 = Pero sólo hemos hablado de Rusia y España
				// 423 = Tus ojos,
				// 471 = Mi hermana Dasha
				// 753 = Mi hermana quería invitar a unos amigos de su novio.
				// 977 = La despedida seria mucho mas dificil.
				if (!(i == 423 || i == 753 || i == 977))
					continue;
#endif

				if (i % LINES_PER_MP3 == 0 || result == null)
				{
					if (result != null)
						result.Close();
					result = new Mp3Composite(String.Format(OutName1Lang, fileOutCount.ToString("D3")));
					fileOutCount++;
				}

				var time1 = formula(line.AudioTime.In) - 500;
				var time2 = formula(line.AudioTime.Out) + 1300;
				if (time1 < prevTime2)
					time1 = prevTime2;
				SimpleLog.Log("{0} - {1}:   {2}", time1, time2, line.Lang1);
				result.WritePieceOfSomeFile(Mp3Name1, time1 / 1000.0, time2 / 1000.0);
				prevTime2 = time2;
			}
			if (result != null)
				result.Close();
		}
	}
}