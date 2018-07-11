using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;
using System.Text;
using System.IO;

namespace com.gscoder.graph.outline
{
	public class Curve : List<Vector>
	{
		public IDictionary<int, List<Vector>> Intersections = new Dictionary<int, List<Vector>>();

		public Curve()
		{ }

		public Curve(IEnumerable<Vector> vectors)
			: base(vectors)
		{ }

		public string Name;

		public Vector StartVector
		{
			get
			{
				return this[1] - this[0];
			}
		}

		public Vector EndVector
		{
			get
			{
				return this[Count - 2] - this[Count - 1];
			}
		}

		public Vector StartPoint
		{
			get
			{
				return this[0];
			}
		}

		public Vector EndPoint
		{
			get
			{
				return this[Count - 1];
			}
		}

		public IEnumerable<Curve> Subcurves
		{
			get
			{
				for (int i = 0; i < Count - 1; i++)
				{
					yield return new Curve(new Vector[] { this[i], this[i + 1] });
				}
			}
		}
	}
}
