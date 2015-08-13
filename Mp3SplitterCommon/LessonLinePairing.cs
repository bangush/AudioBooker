using System.Collections.Generic;
using Mp3SplitterCommon.xml;

namespace Mp3SplitterCommon
{
	public class LessonLinePairing
	{
		public List<LessonLine> LinesFrom1 { get; set; }
		public List<LessonLine> LinesFrom2 { get; set; }

		public LessonLinePairing()
		{
			LinesFrom1 = new List<LessonLine>();
			LinesFrom2 = new List<LessonLine>();
		}
		public LessonLinePairing(LessonLine l1, LessonLine l2)
			: this()
		{
			LinesFrom1.Add(l1);
			LinesFrom2.Add(l2);
		}
	}
}