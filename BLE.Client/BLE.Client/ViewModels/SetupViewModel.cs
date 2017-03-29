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
	public class SetupViewModel : BaseViewModel
	{
		protected readonly IAdapter Adapter;
		private IDevice _device;
		private MotionLockCapture _capture;
		private string type = "Motion";
		public string Type {
			get { return type; }
			set {
				type = value;
				RaisePropertyChanged (() => Type);
			}
		}

		public SetupViewModel (IAdapter adapter) : base (adapter)
		{
			Adapter = adapter;

		}

		protected override void InitFromBundle (IMvxBundle bundle)
		{
			
		}
	}
}
