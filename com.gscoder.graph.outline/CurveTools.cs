using com.gscoder.graph.outline;
using System;
using System.Collections.Generic;

namespace com.gscoder.graph.outline
{
	public class CurveTools
	{
		public static bool IsPointInPolygon(IList<Vector> polygon, Vector vector)
		{
			int i, j;
			bool c = false;
			for (i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
			{
				if ((((polygon[i].Lat <= vector.Lat) && (vector.Lat < polygon[j].Lat))
						|| ((polygon[j].Lat <= vector.Lat) && (vector.Lat < polygon[i].Lat)))
						&& (vector.Lon < (polygon[j].Lon - polygon[i].Lon) * (vector.Lat - polygon[i].Lat)
							/ (polygon[j].Lat - polygon[i].Lat) + polygon[i].Lon))

					c = !c;
			}

			return c;
		}

		public static double PolygonArea(IList<Vector> polygon)
		{
			int i, j;
			double area = 0;

			for (i = 0; i < polygon.Count; i++)
			{
				j = (i + 1) % polygon.Count;

				area += polygon[i].Lon * polygon[j].Lat;
				area -= polygon[i].Lat * polygon[j].Lon;
			}

			area /= 2;
			return (area < 0 ? -area : area);
		}

		public static Bounds PolygonBounds(IList<Vector> polygon)
		{
			var bounds = new Bounds(true);
			Bounds.Include(bounds, polygon);
			return bounds;
		}
		private static Vector LastOf(IList<Vector> points)
		{
			return points[points.Count - 1];
		}

		public void RemoveLooseEnds(CurveCollection curves)
		{
			for (int i = curves.Count - 1; i >= 0; i--)
			{
				bool foundStart = false;
				bool foundEnd = false;
				for (int j = curves.Count - 1; j >= 0; j--)
				{
					if (j == i) continue;
					if (
						curves[i][0] == curves[j][0] ||
						curves[i][0] == LastOf(curves[j]))
					{
						foundStart = true;
					}
					if (
						LastOf(curves[i]) == LastOf(curves[j]) ||
						LastOf(curves[i]) == curves[j][0])
					{
						foundEnd = true;
					}

					if (foundStart && foundEnd)
					{
						break;
					}
				}
				if (!foundStart || !foundEnd)
				{
					curves.RemoveAt(i);
				}
			}
		}

		public static Vector Intersect(Vector line1v1, Vector line1v2, Vector line2v1, Vector line2v2)
		{
			var denom = ((line1v2.Lon - line1v1.Lon) * (line2v2.Lat - line2v1.Lat)) - ((line1v2.Lat - line1v1.Lat) * (line2v2.Lon - line2v1.Lon));
			if (denom == 0) return null;
			var number = ((line1v1.Lat - line2v1.Lat) * (line2v2.Lon - line2v1.Lon)) - ((line1v1.Lon - line2v1.Lon) * (line2v2.Lat - line2v1.Lat));
			var r = number / denom;
			var numer2 = ((line1v1.Lat - line2v1.Lat) * (line1v2.Lon - line1v1.Lon)) - ((line1v1.Lon - line2v1.Lon) * (line1v2.Lat - line1v1.Lat));
			var s = numer2 / denom;

			if ((r < 0 || r > 1) || (s < 0 || s > 1)) return null;

			Vector v = new Vector()
			{
				Lon = line1v1.Lon + (r * (line1v2.Lon - line1v1.Lon)),
				Lat = line1v1.Lat + (r * (line1v2.Lat - line1v1.Lat))
			};

			if ((v == line1v1 || v == line1v2) && (v == line2v1 || v == line2v2)) return null;

			return v;
		}

		public static IEnumerable<Curve> Dissolve(IList<Curve> curves)
		{
			for (int i = 0; i < curves.Count; i++)
			{
				curves[i].Intersections.Clear();
			}

			for (int i = 0; i < curves.Count; i++)
			{
				var dci = curves[i];
				for (int j = i; j < curves.Count; j++)
				{
					var dcj = curves[j];
					for (int k = 0; k < curves[i].Count - 1; k++)
					{
						for (int l = 0; l < curves[j].Count - 1; l++)
						{
							if (i == j && k >= l) continue;

							var intersection = Intersect(curves[i][k], curves[i][k + 1], curves[j][l], curves[j][l + 1]);

							if (intersection != null)
							{
								if (!dci.Intersections.ContainsKey(k)) dci.Intersections.Add(k, new List<Vector>());
								dci.Intersections[k].Add(intersection);

								if (!dcj.Intersections.ContainsKey(l)) dcj.Intersections.Add(l, new List<Vector>());
								dcj.Intersections[l].Add(intersection);
							}
						}
					}
				}
			}

			var dissolved = new List<Curve>();
			for (int i = 0; i < curves.Count; i++)
			{
				var curve = curves[i];
				var vectors = new List<Vector>(curve);
				var indeces = new List<int>();
				int indexOffset = 0;

				indeces.Add(0);

				for (int key = 0; key < curve.Count; key++)
				{
					if (!curve.Intersections.ContainsKey(key)) continue;
					curve.Intersections[key].Sort(delegate(Vector v1, Vector v2)
					{
						var len1 = (v1 - curve[key]).LengthSq;
						var len2 = (v2 - curve[key]).LengthSq;

						return len1 < len2 ? -1 : +1;
					});

					vectors.InsertRange(key + 1 + indexOffset, curve.Intersections[key]);
					for (int l = 0; l < curve.Intersections[key].Count; l++)
					{
						indeces.Add(key + 1 + l + indexOffset);
					}
					indexOffset += curve.Intersections[key].Count;
				}
				indeces.Add(vectors.Count - 1);

				for (int l = 0; l < indeces.Count - 1; l++)
				{
					yield return new Curve(vectors.GetRange(indeces[l], indeces[l + 1] - indeces[l] + 1));
				}
			}
		}
	}
}
