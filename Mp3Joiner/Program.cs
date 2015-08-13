using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mp3SplitterCommon;

namespace Mp3Joiner
{
	class Mp3Joiner
	{
		static void Main(string[] args)
		{
			var firstArg = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			if (args.Length > 0)
				firstArg = args.FirstOrDefault();

			// a directory was gragged in
			if (Directory.Exists(firstArg)) {
				new Mp3Joiner(firstArg);
				return;
			}

			// otherwise a bunch of mp3s were gragged in
			var outFile = Path.GetDirectoryName(firstArg)
				+ Path.DirectorySeparatorChar
				+ Path.GetFileNameWithoutExtension(firstArg)
				+ ".concat"
				+ Path.GetExtension(firstArg);
			new Mp3Joiner(outFile, args);
		}

		public Mp3Joiner(string dirpath)
		{
			var files = Directory.GetFiles(dirpath, "*.mp3");
			DoStitch(dirpath + ".mp3", files);
		}

		public Mp3Joiner(string outFile, string[] files) {
			DoStitch(outFile, files);
		}

		private void DoStitch(string outFile, string[] files)
		{
			SimpleLog.Reset();
			SimpleLog.LogIntro();
			SimpleLog.Log("Generating {0}", outFile);
			var result = new Mp3Composite(outFile);
			foreach (var fff in files) {
				SimpleLog.Log(fff);
				result.AppendAllOfFile(fff);
			}
			result.Close();
		}

		
	}
}
