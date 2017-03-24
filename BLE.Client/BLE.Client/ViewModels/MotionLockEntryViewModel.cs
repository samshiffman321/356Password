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


namespace BLE.Client.ViewModels
{
	public class MotionLockEntryViewModel : BaseViewModel
	{
		protected readonly IAdapter Adapter;
		private IDevice _device;
		private MotionLockCapture _capture;
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

		public MotionLockEntryViewModel (IAdapter adapter) : base(adapter)
		{
			Adapter = adapter;

		}

		protected override void InitFromBundle(IMvxBundle bundle){
			_device = GetDeviceFromBundle (bundle);
			System.Diagnostics.Debug.WriteLine (_device);
			_capture = new MotionLockCapture (_device, this);
		}
	}
}
