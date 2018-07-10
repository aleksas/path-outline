using com.gscoder.graph.outline;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.gscoder
{
	public class Bounds
	{
		public double MinLat;
		public double MinLon;
		public double MaxLat;
		public double MaxLon;

		public Bounds()
		{
		}

		public Bounds(bool oppositeValues)
		{
			if (oppositeValues)
			{
				MinLat = double.MaxValue;
				MinLon = double.MaxValue;
				MaxLat = double.MinValue;
				MaxLon = double.MinValue;
			}
		}

		public static bool operator !=(Bounds b1, Bounds b2)
		{
			return !(b1 == b2);
		}

		public static bool operator ==(Bounds b1, Bounds b2)
		{
			return b1.MaxLat == b2.MaxLat && 
				b1.MinLat == b2.MinLat && 
				b1.MaxLon == b2.MaxLon && 
				b1.MinLon == b2.MinLon;
		}

		public Bounds Union(Bounds b)
		{
			var nb = new Bounds();
			nb.MaxLat = Math.Max(MaxLat, b.MaxLat);
			nb.MaxLon = Math.Max(MaxLon, b.MaxLon);

			nb.MinLat = Math.Min(MinLat, b.MinLat);
			nb.MinLon = Math.Min(MinLon, b.MinLon);
			return nb;
		}

		public bool PointInside(Vector point)
		{
			return point.Lat >= MinLat && point.Lat <= MaxLat && point.Lon >= MinLon && point.Lon <= MaxLon;
		}

		public bool Itersect(Bounds bounds)
		{
			return bounds.MinLat >= MinLat && bounds.MaxLat <= MaxLat && bounds.MinLon >= MinLon && bounds.MaxLon <= MaxLon;
		}

        public static Bounds FromCurves(IEnumerable<Curve> curves)
        {
            var bounds = new Bounds(true);

            foreach (var curve in curves)
            {
				Include(bounds, curve);
            }
			return bounds;
		}

		public static Bounds FromVectors(IEnumerable<Vector> vectors)
		{
			if (vectors == null) return null;
			var bounds = new Bounds(true);
			Include(bounds, vectors);
			return bounds;
		}

		public static void Include(Bounds bounds, IEnumerable<Vector> vectors)
		{
			foreach (var vector in vectors)
			{
				Include(bounds, vector);
			}
		}

		public static void Include(Bounds bounds, Vector vector)
		{
			bounds.MinLat = Math.Min(bounds.MinLat, vector.Lat);
			bounds.MinLon = Math.Min(bounds.MinLon, vector.Lon);
			bounds.MaxLat = Math.Max(bounds.MaxLat, vector.Lat);
			bounds.MaxLon = Math.Max(bounds.MaxLon, vector.Lon);
		}

		public bool IsOutsideOf(Bounds bounds)
		{
			return MinLon <= bounds.MinLon && MaxLon >= bounds.MaxLon && MinLat <= bounds.MinLat && MaxLat >= bounds.MaxLat;
		}
	}
}
