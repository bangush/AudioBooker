using System.Configuration;
using Mp3SplitterCommon;

namespace Mp3SplitterMovie
{
	internal class AbstractSplitter
	{
		protected const int LINES_PER_MP3 = 40;
		protected string XmlFilename = ConfigurationManager.AppSettings["XmlFilename"];
		protected string Mp3Name1 = ConfigurationManager.AppSettings["Mp3Name1"];
		protected string Mp3Name2 = ConfigurationManager.AppSettings["Mp3Name2"];
		protected string OutName2Lang = ConfigurationManager.AppSettings["OutName2Lang"];
		protected string OutName1Lang = ConfigurationManager.AppSettings["OutName1Lang"];

		protected double formula(double xtime)
		{
			// il postino
			//return xtime - 1000;

			// room in rome
			//return (xtime - 146494)*0.95995840995812641998403513471231 + 146494 -6744;

			// dolce vita
			return UtilsTime.Formula_linearOffset(xtime,
			                                      186120, 181000, // 1: Leontina, che cos'è? - Guarda, è Gesù! Ma dove vanno?
			                                      9584080, 9579000); // 1477: Ma che ora è? - Sono le 5:15. - Alle 9 devo essere in Tribunale!
		}
	}
}