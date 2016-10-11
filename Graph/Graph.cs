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

			canvas.DrawLine(horizontal.XStart, horizontal.YStart, horizontal.XStop, horizontal.YStop, axesPaint);

			var vertical = new Line
			{
				XStart = padding.Left,
				XStop = padding.Left,
				YStart = padding.Top,
				YStop = viewHeight - padding.Bottom
			};

			canvas.DrawLine(vertical.XStart, vertical.YStart, vertical.XStop, vertical.YStop, axesPaint);

			// Make X axes labels
			var sectionWidth = (horizontal.XStop - horizontal.XStart) / items.Select(i => i.X).Count();
			Func<int, float> getLabelPlacement = (int i) => sectionWidth * (i + 1 / 2) + horizontal.XStart;


			foreach (Tuple<string, int> l in items.Select(i => i.X).Select((string l, int index) => new Tuple<string, int>(l, index)))
			{
				// Middle of the section minus half of the label text
				var placement = getLabelPlacement(l.Item2) - (textPaint.MeasureText(l.Item1) / 2f);
				textPaint.TextAlign = Paint.Align.Left;

				canvas.DrawText(l.Item1, placement + padding.Left, horizontal.YStart + textPaint.TextSize, textPaint);
			}


			// Make Y axes labels
			var numberOfSections = (int)Math.Ceiling(items.Max(i => i.Y) / 50);
			sectionWidth = (vertical.YStop - vertical.YStart) / numberOfSections;
			Func<int, float> getValuePlacement = (int i) => vertical.YStop - (sectionWidth * i);

			foreach (Tuple<string, int> v in Enumerable.Range(0, numberOfSections + 1).Select(i => new Tuple<string, int>((i * 50).ToString(), i)))
			{
				var placement = getValuePlacement(v.Item2);
				textPaint.TextAlign = Paint.Align.Right;

				canvas.DrawText(v.Item1, padding.Left - density * 5, placement - textPaint.Ascent() / 2, textPaint);


				if (v.Item2 % 2 > 0 && v.Item2 < numberOfSections)
					canvas.DrawRect(padding.Left + (axesPaint.StrokeWidth / 2), placement, viewWidth - padding.Right, placement - sectionWidth, bandsPaint);
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
