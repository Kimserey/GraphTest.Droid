using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Core
{
	public class App : Application
	{
		public App()
		{
			this.MainPage = new ContentPage { Title = "Hello page" };
		}

	}
}
