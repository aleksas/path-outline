using com.gscoder.graph.outline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace com.gscoder.gis.vector.outline
{
	public class CurveBox : Panel
	{
		public CurveCollection curves;
		public IEnumerable<Curve> outlines;
		private IDictionary<int, Pen> pens;
		
		private Pen outlinePen = new Pen(Color.Red, 4);

		public CurveBox()
		{
			curves = new CurveCollection();
			pens = new Dictionary<int, Pen>();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}
		private Point[] ToPoints(Curve c)
		{
			var points = new Point[c.Count];
			for (int i = 0; i < c.Count; i++)
			{
				points[i] = new Point((int)c[i].Lon, (int)c[i].Lat);
			}
			return points;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			var rnd = new Random(DateTime.Now.Millisecond);

			var pen = new Pen(Color.Red, 3);
			for (int i = 0; i < curves.Count; i++)//DrawableCurve curve in curves)
			{
				if (curves[i].Count > 1)
				{
					if (!pens.ContainsKey(i)) pens[i] = pen = new Pen(Color.FromArgb(rnd.Next(200), rnd.Next(200), rnd.Next(200)), 2);
					var points = ToPoints(curves[i]);
					pe.Graphics.DrawCurve(pens[i], points, 0);
				}
			}

			if (outlines != null)
			{
				foreach (var outline in outlines)
				{
					var points = ToPoints(outline);
					pe.Graphics.DrawCurve(outlinePen, points, 0);
				}
			}
		}
	}
}
