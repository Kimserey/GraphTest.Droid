using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Core
{
	public class DataItem
	{
		public string X { get; set; }
		public double Y { get; set; }
	}

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


	public class GraphView : View
	{
		public static readonly BindableProperty DataProperty =
			BindableProperty.Create(
				propertyName: "Data",
				returnType: typeof(IEnumerable<DataItem>),
				declaringType: typeof(GraphView),
				defaultValue: Enumerable.Empty<DataItem>());

		public IEnumerable<DataItem> Data
		{
			get { return (IEnumerable<DataItem>)GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}
	}

	public class App : Application
	{
		public class CustomCell : ViewCell
		{
			public CustomCell()
			{
				var layout = new AbsoluteLayout { Padding = new Thickness(20, 5) };
				var x = new Label { HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalTextAlignment = TextAlignment.Center };
				var y = new Label { HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalTextAlignment = TextAlignment.Center };
				layout.Children.Add(x, new Rectangle(0, 0, .5, 1), AbsoluteLayoutFlags.All);
				layout.Children.Add(y, new Rectangle(1, 0, .5, 1), AbsoluteLayoutFlags.All);
				x.SetBinding(Label.TextProperty, "X");
				y.SetBinding(Label.TextProperty, "Y");
				this.View = layout;
			}
		}

		public App()
		{
			var data = new List<DataItem> {
				new DataItem { X = "JAN", Y = 266.7 },
				new DataItem { X = "FEB", Y = 250.4 },
				new DataItem { X = "MAR", Y = 330 },
				new DataItem { X = "JUN", Y = 126 },
				new DataItem { X = "JUL", Y = 220 },
				new DataItem { X = "AUG", Y = 230 },
				new DataItem { X = "SEP", Y = 266 }
			};

			var list = new ListView
			{
				ItemsSource = data,
				ItemTemplate = new DataTemplate(typeof(CustomCell))
			};

			var layout = new AbsoluteLayout();
			layout.Children.Add(new GraphView(), new Rectangle(0, 0, 1, .4), AbsoluteLayoutFlags.All);
			layout.Children.Add(list, new Rectangle(0, 1, 1, .6), AbsoluteLayoutFlags.All);

			this.MainPage =
				new NavigationPage(
					new ContentPage
					{
						Title = "Line chart",
						Content = layout
					});
		}

	}
}
