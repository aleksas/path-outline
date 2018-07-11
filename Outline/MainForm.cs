using com.gscoder.graph.outline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace com.gscoder.gis.vector.outline
{
	public partial class MainForm : Form
	{
		private Curve currentCurve;

		public MainForm()
		{
			InitializeComponent();
		}

		private void drawAreaPictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (currentCurve != null && e.Button == MouseButtons.Left)
			{
				var v = new Vector() { Lat = e.Y, Lon = e.X };
				if ((v - currentCurve[currentCurve.Count - 1]).LengthSq >= 1000)
				{
					currentCurve.Add(v);
					curveBox.Invalidate();
				}
			}
		}

		private void drawAreaPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (currentCurve != null && currentCurve.Count <= 1) curveBox.curves.Remove(currentCurve);
			currentCurve = null;
		}

		private void drawAreaPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			currentCurve = new Curve();
			currentCurve.Add(new Vector() { Lat = e.Y, Lon = e.X });
			curveBox.curves.Add(currentCurve);
			curveBox.Invalidate();
		}

		private void btnOutline_Click(object sender, EventArgs e)
		{
			curveBox.outlines = null;
			var dissolvedCurves = new List<Curve>(CurveTools.Dissolve(curveBox.curves));
			//var cleanedCurves = new List<Curve>(CurveTools.RemoveOrphanedCurves(dissolvedCurves));
			var curves = new CurveCollection(dissolvedCurves).BreakApart();
			curveBox.curves = new CurveCollection(dissolvedCurves);
			curveBox.outlines = new CurveCollection(curves.GetOutlines());
			curveBox.Invalidate();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			curveBox.curves.Clear();
			curveBox.outlines = null;
			currentCurve = null;
			curveBox.Invalidate();
		}

		private void btnDissolve_Click(object sender, EventArgs e)
		{
		}

		private void btnRemoveLooseEnds_Click(object sender, EventArgs e)
		{

		}
	}
}
