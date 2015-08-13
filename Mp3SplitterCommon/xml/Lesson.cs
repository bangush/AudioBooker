using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Mp3SplitterCommon.xml
{
	[XmlRoot(ElementName = "lesson")]
	public class Lesson
	{
		[XmlElement("header")]
		public LessonHeader Header { get; set; }

		[XmlArray("lines")]
		[XmlArrayItem("line")]
		public List<LessonLine> Lines { get; set; }

		public string Mp3Name
		{
			get
			{
				if (Header.Wav == null)
					return "";
				return Header.Wav.Replace(".wav", ".mp3");
			}
		}
	}
}
