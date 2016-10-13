using System;
using System.Linq;
using Xamarin.Forms;
using Core;
using Renderer.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.Collections.Generic;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer (typeof(GraphView), typeof(GraphViewRenderer))]
namespace Renderer.Droid
{
	public class GraphViewRenderer : ViewRenderer<GraphView, GraphViewRenderer>
	{
		Paint textPaint;
		Paint axesPaint;
		Paint bandsPaint;
		Paint linePaint;
		Paint markersPaint;
		IEnumerable<DataItem> data;
		Padding padding;

		public GraphViewRenderer()
		{
			this.SetWillNotDraw(false);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<GraphView> e)
		{
			base.OnElementChanged(e);

			Initialise();
			if (Control == null)
			{
			}

			if (e.OldElement != null)
			{
			}
			if (e.NewElement != null)
			{
			}
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			GraphViewRenderer.DrawChart(padding,
						   canvas,
						   linePaint,
						   markersPaint,
						   axesPaint,
						   bandsPaint,
						   textPaint,
						   Resources.DisplayMetrics.Density,
						   this.Width,
						   this.Height,
						   data);
		}

		static void DrawChart(
			Padding padding,
			Canvas canvas,
			Paint linePaint,
			Paint marketsPaint,
			Paint axesPaint,
			Paint bandsPaint,
			Paint textPaint,
			float density,
			int viewWidth,
			int viewHeight,
			IEnumerable<DataItem> items)
		{
			var horizontal = new Line
			{
				XStart = padding.Left,
				XStop = viewWidth - padding.Right,
				YStart = viewHeight - padding.Bottom,
				YStop = viewHeight - padding.Bottom
			};


			var vertical = new Line
			{
				XStart = padding.Left,
				XStop = padding.Left,
				YStart = padding.Top,
				YStop = viewHeight - padding.Bottom
			};

			DrawXLabels(canvas, textPaint, horizontal, items.Select(i => i.X));
			DrawYLabels(canvas, density, bandsPaint, textPaint, horizontal, vertical, items.Select(i => i.Y));
			DrawPlot(canvas, density, linePaint, marketsPaint, textPaint, horizontal, vertical, items);

			canvas.DrawLine(horizontal.XStart, horizontal.YStart, horizontal.XStop, horizontal.YStop, axesPaint);
			canvas.DrawLine(vertical.XStart, vertical.YStart, vertical.XStop, vertical.YStop, axesPaint);

		}

		static void DrawXLabels(Canvas canvas, Paint textPaint, Line horizontal, IEnumerable<string> labels)
		{
			textPaint.TextAlign = Paint.Align.Left;

			var sectionWidth = (horizontal.XStop - horizontal.XStart) / labels.Count();

			foreach (Tuple<string, int> l in labels.Select((string l, int index) => Tuple.Create(l, index)))
			{
				var x = sectionWidth * (l.Item2 + 1f / 2f) + horizontal.XStart;
				var halfedTextSize = textPaint.MeasureText(l.Item1) / 2f;

				canvas.DrawText(
					text: l.Item1,
					x: x - halfedTextSize,
					y: horizontal.YStart + textPaint.TextSize,
					paint: textPaint);
			}
		}

		static void DrawYLabels(Canvas canvas, float density, Paint bandsPaint, Paint textPaint, Line horizontal, Line vertical, IEnumerable<double> values)
		{
			textPaint.TextAlign = Paint.Align.Right;

			var numberOfSections = (int)Math.Ceiling(values.Max() / 100);
			var sectionWidth = (vertical.YStop - vertical.YStart) / numberOfSections;

			foreach (var v in Enumerable.Range(0, numberOfSections).Select(i => Tuple.Create(i * 100, i)))
			{
				var y = vertical.YStop - sectionWidth * v.Item2;

				canvas.DrawText(
					text: v.Item1.ToString(),
					x: vertical.XStart - 2 * density,
					y: y - (textPaint.Ascent() / 2f),
					paint: textPaint);

				if (v.Item2 % 2 > 0 && v.Item2 < numberOfSections)
					canvas.DrawRect(
						left: horizontal.XStart,
						top: y - sectionWidth,
						right: horizontal.XStop,
						bottom: y,
						paint: bandsPaint);
			}
		}

		static void DrawPlot(Canvas canvas, float density, Paint linePaint, Paint markersPaint, Paint valuePaint, Line horizontal, Line vertical, IEnumerable<DataItem> items)
		{
			var sectionWidth = (horizontal.XStop - horizontal.XStart) / items.Count();
			var ceiling = (int)Math.Ceiling(items.Max(i => i.Y) / 100f) * 100f;
			var points = new List<Tuple<float, float, double>>();

			foreach (var l in items.Select((l, index) => Tuple.Create(l.X, l.Y, index)))
			{
				var x = sectionWidth * (l.Item3 + 1f / 2f) + horizontal.XStart;
				var y = (float)l.Item2 * (vertical.YStop - vertical.YStart) / ceiling;

				points.Add(Tuple.Create(x, vertical.YStop - y, l.Item2));
			}

			for (int i = 0; i < points.Count; i++)
			{
				if (i < points.Count - 1)
					canvas.DrawLine(
						points[i].Item1,
						points[i].Item2,
						points[i + 1].Item1,
						points[i + 1].Item2,
						linePaint);

				canvas.DrawCircle(
					cx: points[i].Item1,
					cy: points[i].Item2,
					radius: 5 * density,
					paint: markersPaint);

				canvas.DrawText(
					text: points[i].Item3.ToString(),
					x: points[i].Item1,
					y: points[i].Item2,
					paint: valuePaint);
			}
		}

		void Initialise()
		{
			padding = new Padding
			{
				Left = 40 * Resources.DisplayMetrics.Density,
				Right = 20 * Resources.DisplayMetrics.Density,
				Bottom = 30 * Resources.DisplayMetrics.Density,
				Top = 20 * Resources.DisplayMetrics.Density
			};

			textPaint = new Paint
			{
				TextSize = 14 * Resources.DisplayMetrics.Density,
				Color = Color.ParseColor("#37474F")
			};

			axesPaint = new Paint
			{
				StrokeWidth = 2 * Resources.DisplayMetrics.Density,
				Color = Color.ParseColor("#37474F")
			};

			linePaint = new Paint
			{
				StrokeWidth = 2 * Resources.DisplayMetrics.Density,
				Color = Color.ParseColor("#FF5722")
			};

			markersPaint = new Paint
			{
				StrokeWidth = 3 * Resources.DisplayMetrics.Density,
				Color = Color.ParseColor("#448AFF")
			};

			bandsPaint = new Paint
			{
				Color = Color.ParseColor("#EEEEEE")
			};

			data = new List<DataItem> {
				new DataItem { X = "JAN", Y = 266.7 },
				new DataItem { X = "FEB", Y = 250.4 },
				new DataItem { X = "MAR", Y = 330 },
				new DataItem { X = "JUN", Y = 126 },
				new DataItem { X = "JUL", Y = 220 },
				new DataItem { X = "AUG", Y = 230 },
				new DataItem { X = "SEP", Y = 266 }
			};
		}
    }
}
