using System;
using System.Configuration;
using System.IO;

namespace Mp3SplitterCommon
{
	public static class SimpleLog
	{
		public static string LogFilename()
		{
			return DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
		}
		public static void Reset()
		{
			File.WriteAllText(ConfigurationManager.AppSettings["LogFolder"] + "\\" + LogFilename(), "");
		}
		public static void Log(string s, params object[] arg)
		{
			var sss = String.Format(s, arg);
			Console.WriteLine(sss);
			File.AppendAllText(ConfigurationManager.AppSettings["LogFolder"] + "\\" + LogFilename(), sss + "\n");
		}
		public static void LogIntro(string name = null)
		{
			Log("\n\nExecution " + ((name != null) ? " de " + name : "") + "a la date de " + DateTime.Now.ToString() + "\n==================\n");
		}
	}
}
