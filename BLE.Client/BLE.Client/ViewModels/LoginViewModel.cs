using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;

namespace BLE.Client.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		protected readonly new IAdapter Adapter;

		public ICommand EnterPassword { protected set; get; }
		public ICommand DeviceListClick { protected set; get; }

		public LoginViewModel(IAdapter adapter) : base(adapter)
		{
			Adapter = adapter;

			EnterPassword = new Command((nothing) =>
			{
			   string id = Adapter.ConnectedDevices.FirstOrDefault() != null ? this.Adapter.ConnectedDevices.FirstOrDefault().Id.ToString() : null;
			   if (id != null)
			   {
					ShowViewModel<MotionLockEntryViewModel>(new MvxBundle(new Dictionary<string, string> { { DeviceIdKey, id } }));
			   }
			   else
			   {
					Application.Current.MainPage.DisplayAlert("No Device Connected", "You must connect to a device before entering password", "OK");
			   }
		    });
			DeviceListClick = new Command((nothing) =>
			{
				ShowViewModel<DeviceListViewModel>();
			});
		}
	}
}