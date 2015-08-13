using System;
using System.Collections.Generic;
using Mp3SplitterCommon.xml;

namespace Mp3SplitterCommon
{
	public class LevenshteinTuple<TT>
	{
		public int W { get; set; }
		public int Index1 { get; set; }
		public int Index2 { get; set; }
		public TT Item1 { get; set; }
		public TT Item2 { get; set; }
		public LevenshteinTuple<TT> Parent { get; set; }
		public LevenshteinOpType Operation { get; set; }
	}
	public enum LevenshteinOpType
	{
		Insert, Delete, Substitute, None
	}

	public class Levenshtein<T> where T : IComparable
	{
		private IEnumerable<LevenshteinTuple<T>> chain;

		public Levenshtein(List<T> list1, List<T> list2)
		{
			var matrix = ComputeMatrix(list1, list2);
			chain = ReverseLevenChain(matrix, list1.Count, list2.Count);
		}

		private IEnumerable<LevenshteinTuple<T>> ReverseLevenChain(LevenshteinTuple<T>[,] matrix, int n, int m)
		{
			var last = matrix[n, m];
			var result = new List<LevenshteinTuple<T>>();
			while (last.Parent != null) // this eliminates the parent
			{
				result.Add(last);
				last = last.Parent;
			}
			result.Reverse();
			return result;
		}

		private LevenshteinTuple<T>[,] ComputeMatrix(List<T> list1, List<T> list2)
		{
			int n = list1.Count;
			int m = list2.Count;
			var d = new LevenshteinTuple<T>[n + 1, m + 1];

			// Step 1
			if (n == 0)
				return d;
			if (m == 0)
				return d;

			// Step 2
			d[0, 0] = new LevenshteinTuple<T> { W = 0, Parent = null, Operation = LevenshteinOpType.None };
			var prev = d[0, 0];
			for (int i = 1; i <= n; i++)
			{
				d[i, 0] = new LevenshteinTuple<T> { W = i, Parent = prev, Operation = LevenshteinOpType.Insert };
				d[i, 0].Item1 = list1[i - 1];
				d[i, 0].Item2 = list2[0];
				prev = d[i, 0];
			}
			prev = d[0, 0];
			for (int j = 1; j <= m; j++)
			{
				d[0, j] = new LevenshteinTuple<T> { W = j, Parent = prev, Operation = LevenshteinOpType.Insert };
				d[0, j].Item1 = list1[0];
				d[0, j].Item2 = list2[j - 1];
				prev = d[0, j];
			}

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					if (list1[i - 1].CompareTo(list2[j - 1]) == 0)
					{
						var prevvv = d[i - 1, j - 1];
						d[i, j] = new LevenshteinTuple<T> { W = prevvv.W, Parent = prevvv, Operation = LevenshteinOpType.None };
					}
					else
					{
						d[i, j] =
							new[]
								{
									new LevenshteinTuple<T>
										{W = d[i - 1, j].W + 1, Parent = d[i - 1, j], Operation = LevenshteinOpType.Delete},
									new LevenshteinTuple<T>
										{W = d[i, j-1].W + 1, Parent = d[i, j-1], Operation = LevenshteinOpType.Insert},
									new LevenshteinTuple<T>
										{W = d[i - 1, j-1].W + 1, Parent = d[i - 1, j-1], Operation = LevenshteinOpType.Substitute},
								}.MaxValue(x => -x.W);
					}
					d[i, j].Item1 = list1[i - 1];
					d[i, j].Item2 = list2[j - 1];
				}
			}

			// some of my own shit
			for (int i = 0; i <= n; i++)
			{
				for (int j = 0; j <= m; j++)
				{
					d[i, j].Index1 = i;
					d[i, j].Index2 = j;
				}
			}

			return d;
		}

		public IEnumerable<LevenshteinTuple<T>> GetChain()
		{
			return chain;
		}
	}
}