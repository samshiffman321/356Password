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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BLE.Client.Pages;
using BLE.Client.ViewModels;

namespace BLE.Client.Helpers
{
    public class MotionLockCapture
    {
        private IDevice _device;
        private IService _buttons;
        private ICharacteristic button;
        private IService _movement;
        private ICharacteristic movementData;
        private bool Intent = false;
		private MotionLockEntryViewModel _vm;

        public IList<IService> Services { get; private set; }
		public MotionLockCapture(IDevice device, MotionLockEntryViewModel vm)
        {
            _device = device;
			_vm = vm;
            LoadServices();
        }

		private async Task CapturePassword()
        {
            IList<ICharacteristic> characteristics = await _buttons.GetCharacteristicsAsync();
            button = characteristics[0];
            button.ValueUpdated -= IntentFound;
            button.ValueUpdated += IntentFound;
            await button.StartUpdatesAsync();
        }

        private async Task MovementConfig()
        {
            IList<ICharacteristic> mov_characteristics = await _movement.GetCharacteristicsAsync();

            await mov_characteristics[1].WriteAsync(new byte[] {0x07, 0x00}); // Set config bit to get data
            //await characteristics[2].WriteAsync(new byte[] {0x0A}); // Set period to 100ms
            movementData = mov_characteristics[0];
            movementData.ValueUpdated -= UpdateGyroData;
            movementData.ValueUpdated += UpdateGyroData;
            await movementData.StartUpdatesAsync();
        }

        private async void LoadServices()
        {
            Services = await _device.GetServicesAsync();
            foreach (IService current_service in Services)
            {
                if (current_service.Id.ToString().Substring(0,7).CompareTo("0000ffe") == 0)
                {
                    _buttons = current_service;
                } else if (current_service.Id.ToString().Substring(0, 8).CompareTo("f000aa80") == 0)
                {
                    _movement = current_service;
                }
            }
            await CapturePassword();
            await MovementConfig();
        }

        private void IntentFound(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
			if (button.Value[0] == 1 || button.Value [0] == 2 || button.Value [0] == 3){
				Intent = true;
				//COLLECT DATA  
				System.Diagnostics.Debug.WriteLine ("Something is happening");
			    //_vm.CaptureState = string.Concat(movementData.Value.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

			} else if (button.Value[0] == 0 && Intent){
                //Intent = false;
				//this password won't get set if intent isn't set to true, meaning you need to click the button 
                _vm.Password = "THIS IS WHERE PASSWORD IS RECORDED";
				_vm.CaptureState = "Done Capturing";
            }
        }

        private void UpdateGyroData(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
            if (Intent)
            {
                var moveData = movementData.Value;
                short gyroX = BitConverter.ToInt16(new byte[2] {moveData[0], moveData[1]}, 0);
                var xDeg = (gyroX * 1.0) / (65536 / 500);
                short gyroY = BitConverter.ToInt16(new byte[2] { moveData[2], moveData[3] }, 0); ;
                var yDeg = (gyroY * 1.0) / (65536 / 500);
                short gyroZ = BitConverter.ToInt16(new byte[2] { moveData[4], moveData[5] }, 0); ;
                var zDeg = (gyroZ * 1.0) / (65536 / 500);

                _vm.CaptureState = xDeg + " deg/s\n" + yDeg + " deg/s\n" + zDeg + " deg/s";
            }
        }


    }
       
}
