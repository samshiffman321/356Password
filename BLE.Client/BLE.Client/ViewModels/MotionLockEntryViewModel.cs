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
	public class MotionLockEntryViewModel : BaseViewModel
	{
		protected readonly IAdapter Adapter;
		private IDevice _device;
		private ISettings _settings;
		private MotionLockCapture _capture;
        private string _password;
        public ICommand AcceptPassword { protected set; get; }
        private string captureState = "Not Capturing";
		public string CaptureState
		{
			get { return captureState; }
			set
			{
				captureState = value;
				RaisePropertyChanged (() => CaptureState);
			}
		}

        public string Password
        {
            set
            {
                _password = value;
				RaisePropertyChanged ();
            }
        }

        public MotionLockEntryViewModel (IAdapter adapter, ISettings settings) : base(adapter)
		{
			Adapter = adapter;
			_settings = settings;
            AcceptPassword = new Command((nothing) =>
            {
				//this is where you will set the password after it is generated
				_settings.AddOrUpdateValue ("password", _password);
                MessagingCenter.Send<BaseViewModel>(this, "Password");
                Close(this);
            });
        }

		protected override void InitFromBundle(IMvxBundle bundle){
			_device = GetDeviceFromBundle (bundle);
			System.Diagnostics.Debug.WriteLine (_device);
			_capture = new MotionLockCapture (_device, this);
		}
	}
}
