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
		private MotionLockCapture _capture;
        private String _password;
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

        public String Password
        {
            set
            {
                _password = value;
            }
        }

        public MotionLockEntryViewModel (IAdapter adapter) : base(adapter)
		{
			Adapter = adapter;
            AcceptPassword = new Command((nothing) =>
            {
                MessagingCenter.Send<BaseViewModel,String>(this, "Password",_password);
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
