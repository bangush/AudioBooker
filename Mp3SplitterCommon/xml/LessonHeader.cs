using System.Xml.Serialization;

namespace Mp3SplitterCommon.xml
{
	public class LessonHeader
	{
		[XmlAttribute("wav")]
		public string Wav { get; set; }
	}
}