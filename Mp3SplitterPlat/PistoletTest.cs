using System.IO;

namespace Mp3SplitterPlat
{
	public class PistoletTest
	{
		public PistoletTest()
		{
			for (int i = 1; i <= 6; i++)
			{
				var xml = @"C:\Users\mtemkine.TFO\Desktop\hall\it\pistol" + i.ToString("D2") + ".xml";
				var outName = @"C:\Users\mtemkine.TFO\Desktop\hall\pistolet\pisol" + i.ToString("D2") + ".mp3";
				RunSplits(xml, outName);
			}
		}

		private void RunSplits(string xml, string outName)
		{
			var splitDir = Path.GetDirectoryName(outName);
			if (!Directory.Exists(splitDir))
				Directory.CreateDirectory(splitDir);

			var l1 = XmlFactory.LoadFromFile<xml.Lesson>(xml);
			var mp3Name1 = Path.Combine(Path.GetDirectoryName(xml), l1.Mp3Name);

			var result = new Mp3Composite(outName);
			foreach (var line in l1.Lines)
			{
				var aaa = line.AudioTime;
				result.WritePieceOfSomeFile(mp3Name1, Constants.TIME_FACTOR*aaa.In/1000.0, Constants.TIME_FACTOR*aaa.Out/1000.0);
			}
			result.Close();
		}
	}
}