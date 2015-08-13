//#define LANG_WITH_LONGER_LINES_FIRST_COMME_FR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mp3SplitterCommon.xml;

namespace Mp3SplitterCommon
{
	public static class Utils
	{
		public static bool AlmostEquals(this string s1, string s2)
		{
			var y1 = s1.CleanUp();
			var y2 = s2.CleanUp();
			return String.Equals(y1, y2, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool AlmostContains(this string s1, string s2)
		{
			var y1 = s1.CleanUp();
			var y2 = s2.CleanUp();
			return y1.Contains(y2);
		}

		public static string CleanUp(this string s)
		{
			return s
				.Replace(".", "")
				.Replace(",", "")
				.Replace("-", "")
				.Replace(" ", "")
				.Replace("!", "")
				.Replace("\"", "")
				.Replace("'", "")
				.ToLower();
		}

		public static int LevenshteinDistance(this string s1, string s2)
		{
			var s = s1.CleanUp();
			var t = s2.CleanUp();

			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++) {}
			for (int j = 0; j <= m; d[0, j] = j++) {}

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					// Step 6
					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			// Step 7
			return d[n, m];
		}

		public static T MaxValue<T>(this IEnumerable<T> list, Func<T, decimal> func) where T : class
		{
			T maxValue = null;
			var maxW = (decimal)0;
			foreach (var x in list)
			{
				var w = func(x);
				if (maxValue == null || maxW < w)
				{
					maxW = w;
					maxValue = x;
				}
			}
			return maxValue;
		}

		public static IEnumerable<LessonLinePairing> AlignMeta2(IEnumerable<LevenshteinTuple<LessonLine>> chain)
		{
			var result = new List<LessonLinePairing>();
			LessonLinePairing pair = new LessonLinePairing(); // this will be discarted... see note below
			foreach (var c in chain)
			{
				// NOTE: if pair is ever null, it would violate the invariant...
				// NOTE: Original node should be eithe None or Substitute according to the algorithm
				switch (c.Operation)
				{
					case LevenshteinOpType.None:
						pair = new LessonLinePairing(c.Item1, c.Item2);
						result.Add(pair);
						break;
					case LevenshteinOpType.Substitute:
						pair = new LessonLinePairing(c.Item1, c.Item2);
						result.Add(pair);
						break;
					case LevenshteinOpType.Insert:
						pair.LinesFrom2.Add(c.Item2);
						break;
					case LevenshteinOpType.Delete:
						pair.LinesFrom1.Add(c.Item1);
						break;
				}
			}
			return result;
		}

		public static IEnumerable<LessonLinePairing> AlignMeta3(IEnumerable<LevenshteinTuple<LessonLineLetter>> chain)
		{
			var collapsed = new List<LevenshteinTuple<LessonLine>>();
			LevenshteinTuple<LessonLine> cur = null;

			foreach (var c in chain)
			{
				LevenshteinOpType? op = null;

				if (cur == null)
					op = LevenshteinOpType.None;
				else
				{
					if (cur.Item1 != c.Item1.LessonLine)
						op = LevenshteinOpType.None;
#if (LANG_WITH_LONGER_LINES_FIRST_COMME_FR)
					if (cur.Item1 == c.Item1.LessonLine && cur.Item2 != c.Item2.LessonLine)
						op = LevenshteinOpType.Insert;
#else
					if (cur.Item2 == c.Item2.LessonLine && cur.Item1 != c.Item1.LessonLine)
						op = LevenshteinOpType.Delete;
#endif
				}
				
				if (op != null)
				{
					cur = new LevenshteinTuple<LessonLine>
					      	{
								Item1 = c.Item1.LessonLine,
								Item2 = c.Item2.LessonLine,
								Operation = op.Value,
					      	};
					collapsed.Add(cur);
				}
			}

			return AlignMeta2(collapsed);
		}

		public static List<LessonLineLetter> Letterize(this List<LessonLine> lines)
		{
			return lines
				.SelectMany(x => x.Lang2.CleanUp().Select(c => new LessonLineLetter {Letter = c, LessonLine = x}))
				.ToList();
		}
	}
}
