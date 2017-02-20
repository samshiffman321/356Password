using Xamarin.Forms;
using System.Collections.Generic;

namespace MotionLock
{
	public partial class SetupPage : ContentPage
	{
		private List<string> options;
		public SetupPage ()
		{
			InitializeComponent ();
			var layout = new StackLayout ();
			layout.Orientation = StackOrientation.Vertical;
			layout.Spacing = 10;
			layout.Margin = new Thickness (10, 40, 10, 40);

			var typePicker = new Picker ();
			this.options = new List<string> { "Motion", "Orientation", "Combination" };
			options.ForEach ((string obj) => typePicker.Items.Add (obj));
			typePicker.Title = "MotionLock Type";
			typePicker.SelectedIndexChanged += (sender, e) => typeSelectionChanged(sender);
			layout.Children.Add (typePicker);

			Content = layout;
		}

		public void typeSelectionChanged(object sender) {
			Picker typePicker = sender as Picker;
			System.Console.WriteLine ("type selection changed to: " + this.options[typePicker.SelectedIndex]);
		}
	}
}
