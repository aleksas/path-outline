using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.gscoder.graph.outline
{
	public class CurveCollection : List<Curve>
	{
		public CurveCollection()
		{ }

		public CurveCollection(IEnumerable<Curve> curves) :
			base(new List<Curve>(curves))
		{
		}

		public static CurveCollection FromVectorSets(IEnumerable<IEnumerable<Vector>> vectorsSets)
		{
			var curves = new CurveCollection();
			foreach (var vectors in vectorsSets)
			{
				curves.Add(new Curve(vectors));
			}
			return curves;
		}

		public IEnumerable<Curve> GetOutlines(bool firstOnly = false)
		{
			var initialCurveIndex = NorthWestCurveIndex();
			return GetOutlines(NorthWestCurveIndex(), firstOnly);
		}

		public IEnumerable<Curve> GetOutlines(int initialCurveIndex, bool firstOnly = false)
		{
			var used = new bool[this.Count];
			var exclude = new bool[this.Count];

			var outline = (Curve)null;
			var path = (IList<int>)null;

			var curveIndex = initialCurveIndex;

			do
			{
				outline = GetOutline(curveIndex, out path, ref used, ref exclude);

				if (firstOnly)
				{
					yield return outline;
					break;
				}
				else
				{
					if (outline != null)
					{
						MarkInnnerCurvesAsUsed(path, ref used, exclude);
						yield return outline;
					}
					curveIndex = NorthWestCurveIndex(used, exclude);
				}

			} while (outline != null);
		}

		public void MarkInnnerCurvesAsUsed(IList<int> path, ref bool[] used, bool[] exclude)
		{
			var check = new Stack<int>(path);

			do
			{
				var c = check.Pop();

				for (int i = 0; i < this.Count; i++)
				{
					if (used[i] || exclude[i]) continue;

					if (this[c][0] == this[i][0] ||
						this[c][this[c].Count - 1] == this[i][0] ||
						this[c][0] == this[i][this[i].Count - 1] ||
						this[c][this[c].Count - 1] == this[i][this[i].Count - 1])
					{
						used[i] = true;
						check.Push(i);
					}
				}
			} while (check.Count > 0);
		}

		public Curve GetOutline(int initialCurveIndex, out IList<int> path, ref bool[] used, ref bool[] exclude)
		{
			if (initialCurveIndex == -1)
			{
				path = null;
				return null;
			}

			var usedNotReversed = (bool[])used.Clone();
			var usedReversed = (bool[])used.Clone();

			var excludeNotReversed = (bool[])exclude.Clone();
			var excludeReversed = (bool[])exclude.Clone();

			IList<int> pathNotReversed, pathReversed;
			Curve outline, outlineReversed;

			GetOutlineCandidates(initialCurveIndex, out pathNotReversed, out pathReversed, out outline, out outlineReversed, ref usedNotReversed, ref usedReversed, ref excludeNotReversed, ref excludeReversed);

			bool preferNotReversed = outline != null;
			bool preferReversed = outlineReversed != null;

			if (outline != null && outlineReversed != null)
			{ 
				var outlineBounds = Bounds.FromVectors(outline);
				var outlineReversedBounds = Bounds.FromVectors(outlineReversed);

				if (outlineBounds != outlineReversedBounds)
				{
					preferNotReversed = outlineBounds.IsOutsideOf(outlineReversedBounds);
					preferReversed = outlineReversedBounds.IsOutsideOf(outlineBounds);
				}
				else
				{
					preferNotReversed = CurveTools.PolygonArea(outline) >= CurveTools.PolygonArea(outlineReversed);
					preferReversed = !preferNotReversed;
				}
			}

			if (preferNotReversed)
			{
				path = pathNotReversed;
				for (int i = 0; i < this.Count; i++) exclude[i] |= excludeNotReversed[i];
				for (int i = 0; i < this.Count; i++) used[i] |= usedNotReversed[i];
				return outline;
			}
			else if (preferReversed)
			{
				path = pathReversed;
				for (int i = 0; i < this.Count; i++) exclude[i] |= excludeReversed[i];
				for (int i = 0; i < this.Count; i++) used[i] |= usedReversed[i];
				return outlineReversed;
			}
			else
			{
				for (int i = 0; i < this.Count; i++) exclude[i] |= excludeNotReversed[i] | excludeReversed[i];
				for (int i = 0; i < this.Count; i++) used[i] |= usedNotReversed[i] | usedReversed[i];
				path = null;
				return null;
			}
		}

		public Curve GetOutlineAroundPoint(Vector point)
		{
			var usedNotReversed = new bool[this.Count];
			var usedReversed = new bool[this.Count];

			var excludeNotReversed = new bool[this.Count];
			var excludeReversed = new bool[this.Count];

			IList<int> path, pathReversed;
			Curve outline, outlineReversed;

			var closestPoint = (Vector)null;
			var curveIndex = ClosestCurveIndex(point, out closestPoint, usedReversed, excludeReversed);

			GetOutlineCandidates(curveIndex, out path, out pathReversed, out outline, out outlineReversed, ref usedNotReversed, ref usedReversed, ref excludeNotReversed, ref excludeReversed);

			if (outline != null && CurveTools.IsPointInPolygon(outline, point)) return outline;
			else if (outlineReversed != null && CurveTools.IsPointInPolygon(outlineReversed, point)) return outlineReversed;
			else return null;
		}

		public void GetOutlineCandidates(int startCurveIndex, out IList<int> path, out IList<int> pathReversed, out Curve outline, out Curve outlineReversed, ref bool[] usedNotReversed, ref bool[] usedReversed, ref bool[] excludeNotReversed, ref bool[] excludeReversed)
		{
			outline = GetOutline(startCurveIndex, false, out path, ref usedNotReversed, ref excludeNotReversed);
			outlineReversed = GetOutline(startCurveIndex, true, out pathReversed, ref usedReversed, ref excludeReversed);
		}

		private Curve GetOutline(int startIndex, bool reverseInitialCurve, out IList<int> path, ref bool[] used, ref bool[] exclude)
		{
			path = new List<int>();

			path.Add(startIndex);
			var lastIndex = startIndex;

			if (reverseInitialCurve) this[startIndex].Reverse();

			used[startIndex] = true;

			var lastTrackpoint = this[startIndex][this[startIndex].Count - 1];
			var endVector = this[startIndex].EndVector;

			do
			{
				double smallestAngle = 2 * Math.PI;
				int selected = -1;

				for (int i = 0; i < this.Count; i++)
				{
					if (used[i] || exclude[i]) continue;
					if (lastTrackpoint.Equals(this[i][this[i].Count - 1])) this[i].Reverse();
					if (lastTrackpoint.Equals(this[i][0]))
					{
						var startVector = this[i].StartVector;
						var angle = startVector.Angle(endVector);

						if (angle < smallestAngle)
						{
							smallestAngle = angle;
							selected = i;
						}
					}
				}

				if (selected == -1)
				{
					exclude[lastIndex] = true;
					used[lastIndex] = false;

					path.RemoveAt(path.Count - 1);

					if (path.Count == 0) return null;

					lastIndex = path[path.Count - 1];

					lastTrackpoint = this[lastIndex][this[lastIndex].Count - 1];
					endVector = this[lastIndex].EndVector;
					continue;
				}

				used[selected] = true;
				path.Add(selected);
				lastIndex = path[path.Count - 1];

				lastTrackpoint = this[lastIndex][this[lastIndex].Count - 1];
				endVector = this[lastIndex].EndVector;
			} while (this[path[0]][0] != lastTrackpoint);

			var outline = new Curve();
			foreach (var x in path)
			{
				foreach (var p in this[x])
				{
					outline.Add(p);
				}
			}
			return outline;
		}

		private int NorthWestCurveIndex()
		{
			return NorthWestCurveIndex(new bool[this.Count], new bool[this.Count]);
		}

		private int NorthWestCurveIndex(bool[] used, bool[] exclude)
		{
			Vector closestPoint;
			var p = new Vector() { Lat = Vector.MaxLat, Lon = 0 };
			return ClosestCurveIndex(p, out closestPoint, used, exclude);
		}

		private int ClosestCurveIndex(Vector vector, bool[] used, bool[] exclude)
		{
			Vector closestPoint;
			return ClosestCurveIndex(vector, out closestPoint, used, exclude);
		}

		private int ClosestCurveIndex(Vector vector, out Vector closestPoint, bool[] used, bool[] exclude)
		{
			int res = -1;
			double minDistSq = double.MaxValue;
			closestPoint = null;
			for (int i = 0; i < this.Count; i++)
			{
				if (used[i] || exclude[i]) continue;

				foreach (var point in this[i])
				{
					var distSq = (point - vector).LengthSq;
					if (distSq < minDistSq)
					{
						res = i;
						minDistSq = distSq;
						closestPoint = point;
					}
				}
			}
			return res;
		}

		public CurveCollection BreakApart()
		{
			CurveCollection result = new CurveCollection();

			foreach (Curve c in this)
			{
				foreach (Curve subc in c.Subcurves)
				{
					result.Add(subc);
				}
			}

			return result;
		}
	}
}
