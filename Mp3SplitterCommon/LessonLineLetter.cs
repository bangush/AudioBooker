using System;
using Mp3SplitterCommon.xml;

namespace Mp3SplitterCommon
{
	public class LessonLineLetter : IComparable
	{
		public char Letter { get; set; }
		public LessonLine LessonLine { get; set; }
		
		public int CompareTo(object obj)
		{
			if (!(obj is LessonLineLetter))
				return 1;
			var lll = (LessonLineLetter) obj;
			if (this.Letter == lll.Letter)
				return 0;
			return 1;
		}
	}
}