using System.Xml.Serialization;

namespace Mp3SplitterPlat.xml
{
	public class LessonHeader
	{
		[XmlAttribute("wav")]
		public string Wav { get; set; }
	}
}