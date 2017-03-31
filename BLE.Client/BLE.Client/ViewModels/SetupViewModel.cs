﻿using System;
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
        private readonly ISettings _settings;
        private int index = 0;
        private string _password;
        private Boolean _finishVisible = false;
        public ICommand FinishButton { protected set; get; }
        public ICommand ConnectDevice { protected set; get; }
        public ICommand CreatePassword { protected set; get; }
        public int SelectedIndex {
			get { return index; }
			set {
				index = value;
				RaisePropertyChanged (() => SelectedIndex);
			}
		}
        public Boolean Finish
        {
            get { return !_finishVisible; }
            set
            {
                RaisePropertyChanged(() => Finish);
            }
        }
        public Boolean FinishVisible
        {
            get { return _finishVisible; }
            set
            {
                _finishVisible = value;
                RaisePropertyChanged(() => FinishVisible);
                Finish = !value;
            }
        }

        public string DeviceName
        {
            get { return Adapter.ConnectedDevices.FirstOrDefault () != null ? Adapter.ConnectedDevices.FirstOrDefault ().Name : "No Device Connected"; }
            
        }

        public String Password
        {
            set
            {
                _password = value;
                _settings.AddOrUpdateValue("password", _password);
                RaisePropertyChanged();
            }
        }

        public SetupViewModel (IAdapter adapter, ISettings settings) : base (adapter)
		{
			Adapter = adapter;
            _settings = settings;


            ConnectDevice = new Command((nothing) =>
            {
                ShowViewModel<DeviceListViewModel>();

            });
            FinishButton = new Command((nothing) =>
            {
                //MOVE TO LIST

            });
            CreatePassword = new Command((nothing) =>
            {
				string id = Adapter.ConnectedDevices.FirstOrDefault () != null ? Adapter.ConnectedDevices.FirstOrDefault ().Id.ToString () : null;

				if (id != null){
					ShowViewModel<MotionLockEntryViewModel> (new MvxBundle (new Dictionary<string, string> { { DeviceIdKey, id } }));
				} else {
					Application.Current.MainPage.DisplayAlert ("No Device Connected", "You must connect to a device before entering password", "OK");
				}

                
            });
            MessagingCenter.Subscribe<BaseViewModel>(this, "Reload", (sender) => {
                var device = DeviceName;
            });
            MessagingCenter.Subscribe<BaseViewModel, String>(this, "Password", (sender, arg) => {
                Password = arg;
                FinishVisible = true;
            });
        }

	}
}