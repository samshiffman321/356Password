using System.Collections.Generic;
using MvvmCross.Platform;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.IO;
using BLE.Client.Pages;
using BLE.Client.ViewModels;

namespace BLE.Client.Helpers
{
    public class MotionLockCapture
    {
        private IDevice _device;
        private IService _buttons;
        private ICharacteristic button;
        private bool Intent = false;
		private MotionLockEntryViewModel _vm;

        public IList<IService> Services { get; private set; }
		public MotionLockCapture(IDevice device, MotionLockEntryViewModel vm)
        {
            _device = device;
			_vm = vm;
            LoadServices();
        }

        private async void CapturePassword()
        {
            IList<ICharacteristic> characteristics = await _buttons.GetCharacteristicsAsync();
            button = characteristics[0];
            button.ValueUpdated -= IntentFound;
            button.ValueUpdated += IntentFound;
            await button.StartUpdatesAsync();
        }

        private async void LoadServices()
        {
            Services = await _device.GetServicesAsync();
            foreach (IService current_service in Services)
            {
                if (current_service.Id.ToString().Substring(0,7).CompareTo("0000ffe") == 0)
                {
                    _buttons = current_service;
                }
            }
            CapturePassword();
        }

        private void IntentFound(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
			if (button.Value[0] == 1 || button.Value [0] == 2 || button.Value [0] == 3){
				Intent = true;
				//COLLECT DATA  
				System.Diagnostics.Debug.WriteLine ("Something is happening");
				_vm.CaptureState = "Capturing";
			} else if (button.Value[0] == 0 && Intent){
                Intent = false;
				//this password won't get set if intent isn't set to true, meaning you need to click the button 
                _vm.Password = "THIS IS WHERE PASSWORD IS RECORDED";
				_vm.CaptureState = "Done Capturing";
            }
        }


    }
       
}
