using System;
using System.Collections.Generic;
using System.Text;

namespace com.gscoder.graph.outline
{
	public class Vector
	{
		public static readonly double NorthLat = 90;
		public static readonly double MaxLat = Math.Sqrt(int.MaxValue) / 2;

		public double Lat;
		public double Lon;

		public Vector() { }

		public Vector(double lat, double lon) 
		{
			Lat = lat;
			Lon = lon;
		}

		public double Angle(Vector vector)
		{
			return Angle(this, vector);
		}

		public static double Angle(Vector v1, Vector v2)
		{
			var dot = v1.Lon * v2.Lon + v1.Lat * v2.Lat;
			var det = v1.Lon * v2.Lat - v1.Lat * v2.Lon;
			var rad = Math.Atan2(det, dot);
			if (rad < 0) rad += 2 * Math.PI;
			return rad;
		}

		public static Vector operator -(Vector v1, Vector v2)
		{
			return new Vector() { Lat = v1.Lat - v2.Lat, Lon = v1.Lon - v2.Lon };
		}

		public static Vector operator +(Vector v1, Vector v2)
		{
			return new Vector() { Lat = v1.Lat + v2.Lat, Lon = v1.Lon + v2.Lon };
		}

		public Vector UnitVector
		{
			get
			{
				var len = Math.Sqrt(Lat * Lat + Lon * Lon);
				return new Vector() { Lat = Lat / len, Lon = Lon / len };
			}
		}

		public double LengthSq
		{
			get
			{
				return Lon * Lon + Lat * Lat;
			}
		}

		public static bool operator ==(Vector v1, Vector v2)
		{
			if (System.Object.ReferenceEquals(v1, v2))
			{
				return true;
			}

			if (((object)v1 == null) || ((object)v2 == null))
			{
				return false;
			}

			return v1.Equals(v2);
		}

		public static bool operator !=(Vector v1, Vector v2)
		{
			return !(v1 == v2);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			var other = obj as Vector;
			return (Lat == other.Lat && Lon == other.Lon);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
