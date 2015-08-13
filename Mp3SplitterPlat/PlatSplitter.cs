//#define LogTest

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mp3SplitterCommon
{
	public class PlatSplitter
	{
		public PlatSplitter()
		{
			SimpleLog.Reset();
			SimpleLog.LogIntro();

			for (int i = 1; i <= 144; i++)
			{
				// NOTE: don't forget to change the LANG_WITH_LONGER_LINES_FIRST_COMME_FR flag in Utils.cs

				//SimpleLog.Log("===============================\nFile: fr_it_" + i.ToString("D3") + ".mp3");
				//var xml1 = @"C:\Users\mtemkine.TFO\Desktop\hall\fr\f_" + i.ToString("D3") + ".xml";
				//var xml2 = @"C:\Users\mtemkine.TFO\Desktop\hall\it\i_" + i.ToString("D3") + ".xml";
				//var outName = @"C:\Users\mtemkine.TFO\Desktop\hall\fr_it\fr_it_" + i.ToString("D3") + ".mp3";

				//SimpleLog.Log("===============================\nFile: it_fr_" + i.ToString("D3") + ".mp3");
				//var xml1 = @"C:\Users\mtemkine.TFO\Desktop\hall\it\i_" + i.ToString("D3") + ".xml";
				//var xml2 = @"C:\Users\mtemkine.TFO\Desktop\hall\fr\f_" + i.ToString("D3") + ".xml";
				//var outName = @"C:\Users\mtemkine.TFO\Desktop\hall\it_fr\it_fr_" + i.ToString("D3") + ".mp3";

				//SimpleLog.Log("===============================\nFile: fr_de_" + i.ToString("D3") + ".mp3");
				//var xml1 = @"C:\Users\mtemkine.TFO\Desktop\hall\fr\f_" + i.ToString("D3") + ".xml";
				//var xml2 = @"C:\Users\mtemkine.TFO\Desktop\hall\de\d_" + i.ToString("D3") + ".xml";
				//var outName = @"C:\Users\mtemkine.TFO\Desktop\hall\fr_de\fr_de_" + i.ToString("D3") + ".mp3";

				SimpleLog.Log("===============================\nFile: de_fr_" + i.ToString("D3") + ".mp3");
				var xml1 = @"C:\Users\mtemkine.TFO\Desktop\hall\de\d_" + i.ToString("D3") + ".xml";
				var xml2 = @"C:\Users\mtemkine.TFO\Desktop\hall\fr\f_" + i.ToString("D3") + ".xml";
				var outName = @"C:\Users\mtemkine.TFO\Desktop\hall\de_fr\de_fr_" + i.ToString("D3") + ".mp3";

				RunSplits(xml1, xml2, outName);
			}

			//Console.ReadKey();
		}

		private void RunSplits(string xml1, string xml2, string outName)
		{
			var splitDir = Path.GetDirectoryName(outName);
			if (!Directory.Exists(splitDir))
				Directory.CreateDirectory(splitDir);

			var l1 = XmlFactory.LoadFromFile<xml.Lesson>(xml1);
			var l2 = XmlFactory.LoadFromFile<xml.Lesson>(xml2);
			var mp3Name1 = Path.Combine(Path.GetDirectoryName(xml1), l1.Mp3Name);
			var mp3Name2 = Path.Combine(Path.GetDirectoryName(xml2), l2.Mp3Name);

			//var lev = new Levenshtein<LessonLine>(l1.Lines, l2.Lines);
			//IEnumerable<LessonLinePairing> pairings = Utils.AlignMeta2(lev.GetChain());

			var lev = new Levenshtein<LessonLineLetter>(l1.Lines.Letterize(), l2.Lines.Letterize());
			IEnumerable<LessonLinePairing> pairings = Utils.AlignMeta3(lev.GetChain());

#if(LogTest)
			SimpleLog.Log("" + outName);
			foreach (var pair in pairings)
			{
				SimpleLog.Log("  -------");
				foreach (var lll in pair.LinesFrom1)
					SimpleLog.Log("  lang1={0}", lll.Lang2);
				foreach (var lll in pair.LinesFrom2)
					SimpleLog.Log("  lang2={0}", lll.Lang2);
			}
#else
			var result = new Mp3Composite(outName);
			foreach (var pair in pairings)
			{
				foreach (var aaa in pair.LinesFrom1.Select(x => x.AudioTime))
					result.WritePieceOfSomeFile(mp3Name1, Constants.TIME_FACTOR * aaa.In / 1000.0, Constants.TIME_FACTOR * aaa.Out / 1000.0);
				foreach (var aaa in pair.LinesFrom2.Select(x => x.AudioTime))
					result.WritePieceOfSomeFile(mp3Name2, Constants.TIME_FACTOR * aaa.In / 1000.0, Constants.TIME_FACTOR * aaa.Out / 1000.0);
			}
			result.Close();
#endif
		}
	}
}
