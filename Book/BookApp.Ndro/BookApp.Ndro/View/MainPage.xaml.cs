﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookApp.Ndro.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : NavigationPage
    {
		public MainPage (HomePage rootPage):base(rootPage)
		{
			InitializeComponent ();
		}
	}
}