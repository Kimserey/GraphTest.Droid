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
	public class GraphView : View
	{
		Paint textPaint;
		Paint linePaint;
		Paint boxPaint1;
		float scaleFactor;
		Paint boxPaint2;
		double cost = 10;
		double earnings = 15;


		public GraphView(Context context) : base(context)
		{
			Initialise();
		}

		public GraphView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialise();
		}

		public GraphView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialise();
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			var padding = (int)(10 * scaleFactor);
			int maxBarHeight = Height - 5 * padding;
			float middle = (float)(Width * 0.5);
			float bar1height;
			float bar2height;
			float quarter = (float)(Width * 0.25);
			float threequarter = (float)(Width * 0.75);


			if (earnings > cost)
			{
				bar2height = (float)maxBarHeight;
				bar1height = (float)(cost / earnings * maxBarHeight);
			}
			else {
				bar1height = (float)maxBarHeight;
				bar2height = (float)(earnings / cost * maxBarHeight);
			}

			float bar1bottom = Height - padding * 3;
			float bar1top = bar1bottom - bar1height;
			canvas.DrawRect(padding * 2, bar1top, middle - padding, bar1bottom, boxPaint1);
			canvas.DrawText("Cost", quarter - padding, Height - padding, textPaint);
			canvas.DrawText("$" + cost / 1000 + "K", quarter - padding, bar1top - padding, textPaint);

			float bar2bottom = Height - padding * 3;
			float bar2top = bar2bottom - bar2height;
			canvas.DrawRect(middle + padding, bar2top, Width - (padding * 2), bar2bottom, boxPaint2);
			canvas.DrawText("Earnings", threequarter - padding, Height - padding, textPaint);
			canvas.DrawText("$" + earnings / 1000 + "K", threequarter - padding, bar2top - padding, textPaint);

			var x = padding;
			var y = (float)(Height - 2.5 * padding);
			var stopx = Width - padding;
			var stopy = (float)(Height - 2.5 * padding);
			canvas.DrawLine(x, y, stopx, stopy, linePaint);
		}

		void Initialise()
		{
			scaleFactor = Resources.DisplayMetrics.Density;
			
			textPaint = new Paint
			{
				Color = Color.Gray,
				TextSize = 14 * scaleFactor
			};
			linePaint =
				new Paint
				{
					StrokeWidth = 1,
					Color = Color.Gray
				};
			boxPaint1 =
				new Paint
				{
					Color = Color.Blue
				};
			boxPaint2 =
				new Paint
				{
					Color = Color.Yellow
				};
		}

		public void SetCost(double value)
		{
			cost = value;
			this.Invalidate();
		}

		public void SetEarnings(double value)
		{
			earnings = value;
			this.Invalidate();
		}
	}
}
