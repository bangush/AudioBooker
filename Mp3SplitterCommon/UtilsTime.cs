namespace Mp3SplitterCommon
{
	public static class UtilsTime
	{
		// Usage: measure times respectively for xml and audio
		// preferable pick 2 lines far far from each other
		public static double Formula_linearOffset(double xtime, double xmlT1, double auT1, double xmlT2, double auT2)
		{
			//return (xtime - 146494) * 0.95995840995812641998403513471231 + 146494 - 6744;
			return (xtime - xmlT1) * (auT2 - auT1) / (xmlT2 - xmlT1) + auT1;
		}
	}
}