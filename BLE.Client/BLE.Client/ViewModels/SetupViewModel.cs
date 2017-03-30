using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BLE.Client.Extensions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.Settings.Abstractions;
using BLE.Client.Helpers;
using Xamarin.Forms;
using System.Windows.Input;


namespace BLE.Client.ViewModels
{
	public class SetupViewModel : BaseViewModel
	{
		protected readonly IAdapter Adapter;
		private MotionLockCapture _capture;
		private string type = "Motion";
        private string _deviceName = "No Device Connected";
        public ICommand ConnectDevice { protected set; get; }
        public ICommand CreatePassword { protected set; get; }
        public string Type {
			get { return type; }
			set {
				type = value;
				RaisePropertyChanged (() => Type);
			}
		}

        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                type = value;
                RaisePropertyChanged(() => DeviceName);
            }
        }

        public SetupViewModel (IAdapter adapter) : base (adapter)
		{
			Adapter = adapter;
            ConnectDevice = new Command((nothing) =>
            {
                ShowViewModel<DeviceListViewModel>();

            });
            CreatePassword = new Command((nothing) =>
            {
                ShowViewModel<MotionLockEntryViewModel>(new MvxBundle(new Dictionary<string, string> { { DeviceIdKey, _device.Id.ToString() } }));
            });

        }

	}
}
