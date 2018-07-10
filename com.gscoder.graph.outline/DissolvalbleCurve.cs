using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;
using System.Text;
using System.IO;

namespace com.gscoder.graph.outline
{
	public class DissolvalbleCurve : Curve
	{
		public IDictionary<int, List<Vector>> Intersections = new Dictionary<int, List<Vector>>();

        public DissolvalbleCurve()
        { }

        public DissolvalbleCurve(IEnumerable<Vector> vectors) : base(vectors)
        { }
	}
}
