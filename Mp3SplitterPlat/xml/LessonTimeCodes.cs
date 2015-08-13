using System.Xml.Serialization;

namespace Mp3SplitterPlat.xml
{
	public class LessonTimeCodes
	{
		[XmlAttribute("in")]
		public int In { get; set; }

		[XmlAttribute("out")]
		public int Out { get; set; }

		public int Length
		{
			get { return Out - In; }
		}
		public double LengthD
		{
			get { return (double)Out - In; }
		}
	}
}