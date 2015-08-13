using System;
using System.Xml.Serialization;

namespace Mp3SplitterPlat.xml
{
	public class LessonLine : IComparable
	{
		[XmlAttribute("lang2")]
		public string Lang2 { get; set; }

		[XmlElement("audioTime")]
		public LessonTimeCodes AudioTime { get; set; }

		public int CompareTo(object obj)
		{
			if (!(obj is LessonLine))
				return 1;
			var lll = (LessonLine) obj;
			if (this.Lang2.AlmostEquals(lll.Lang2))
				return 0;
			return 1;
		}
	}
}