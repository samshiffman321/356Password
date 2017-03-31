using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using System.Windows.Input;

namespace BLE.Client.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		protected readonly new IAdapter Adapter;

		private LoginViewModel value1;

		//public LoginViewModel ButtonSelect
		//{
		//	get
		//	{
		//		enterPassword();
		//	}
		//	set { if (value != null) value1 = value;}
		//}

		public LoginViewModel(IAdapter adapter) : base(adapter)
		{
			Adapter = adapter;

		}

		public ICommand Click
		{
			get
			{
				return new Command(() =>
				{
					ShowViewModel<DeviceListViewModel>();
				});
			}
		}
	}
}