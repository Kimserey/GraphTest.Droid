using System;
using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;

namespace Graph
{
	public class Line
	{
		public float XStart { get; set; }
		public float XStop { get; set; }
		public float YStart { get; set; }
		public float YStop { get; set; }
	}

	public class Padding
	{
		public float Left { get; set; }
		public float Right { get; set; }
		public float Top { get; set; }
		public float Bottom { get; set; }
	}

	public class DataItem
	{
		public string X { get; set; }
		public double Y { get; set; }
	}

	public class LineChart : View
	{
		Paint textPaint;
		Paint axesPaint;
		Paint bandsPaint;
		IEnumerable<DataItem> data;
		Padding padding;


		public LineChart(Context context) : base(context)
		{
			Initialise();
		}

		public LineChart(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialise();
		}

		public LineChart(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialise();
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			LineChart.DrawChart(padding,
						   canvas,
						   axesPaint,
						   bandsPaint,
						   textPaint,
						   Resources.DisplayMetrics.Density,
						   this.Width,
						   this.Height,
						   data);
		
			textPaint.Dispose();
			axesPaint.Dispose();
			bandsPaint.Dispose();
		}

		static void DrawChart(
			Padding padding, 
			Canvas canvas, 
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
			DrawMarkers(canvas, density, axesPaint, horizontal, vertical, items);

			canvas.DrawLine(horizontal.XStart, horizontal.YStart, horizontal.XStop, horizontal.YStop, axesPaint);
			canvas.DrawLine(vertical.XStart, vertical.YStart, vertical.XStop, vertical.YStop, axesPaint);

		}

		static void DrawXLabels(Canvas canvas, Paint textPaint, Line horizontal, IEnumerable<string> labels)
		{ 
			textPaint.TextAlign = Paint.Align.Left;

			var sectionWidth = (horizontal.XStop - horizontal.XStart) / labels.Count();

			foreach (Tuple<string, int> l in labels.Select((string l, int index) => Tuple.Create(l, index)))
			{
				var x  = sectionWidth * (l.Item2 + 1f / 2f) + horizontal.XStart;
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
			
			var numberOfSections = (int)Math.Ceiling(values.Max() / 50);
			var sectionWidth = (vertical.YStop - vertical.YStart) / numberOfSections;

			foreach (var v in Enumerable.Range(0, numberOfSections).Select(i => Tuple.Create(i * 50, i)))
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

		static void DrawMarkers(Canvas canvas, float density, Paint markersPaint, Line horizontal, Line vertical, IEnumerable<DataItem> items)
		{ 
			var sectionWidth = (horizontal.XStop - horizontal.XStart) / items.Count();
			var ceiling = (int)Math.Ceiling(items.Max(i => i.Y) / 50f) * 50f;

			foreach (var l in items.Select((l, index) => Tuple.Create(l.X, l.Y, index)))
			{
				var x = sectionWidth * (l.Item3 + 1f / 2f) + horizontal.XStart;
				var y = (float)l.Item2 * (vertical.YStop - vertical.YStart) / ceiling;

				canvas.DrawCircle(
					cx: x, 
					cy: vertical.YStop - y,
					radius: 5 * density,
					paint: markersPaint);
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
				Color = Color.Gray,
				TextSize = 14 * Resources.DisplayMetrics.Density
			};

			axesPaint = new Paint
			{
				StrokeWidth = 2 * Resources.DisplayMetrics.Density,
				Color = Color.Gray
			};

			bandsPaint = new Paint
			{
				Color = Color.LightGray
			};

			data = new List<DataItem> {
				new DataItem { X = "JAN", Y = 266.7 },
				new DataItem { X = "FEB", Y = 250.4 },
				new DataItem { X = "MAR", Y = 330 },
				new DataItem { X = "JUN", Y = 126 }
			};
		}
	}
}
