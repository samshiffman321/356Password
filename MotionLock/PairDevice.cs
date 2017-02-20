using System;

using Xamarin.Forms;

namespace MotionLock
{
	public class PairDevice : ContentPage
	{
		public PairDevice ()
		{
			NavigationPage.SetBackButtonTitle (this, "Cancel");
			Content = new StackLayout {
				Children = {
					new Label { Text = "Searching for SensorTags" }
				}
			};
		}
	}
}

