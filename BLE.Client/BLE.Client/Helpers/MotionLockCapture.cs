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

namespace BLE.Client.Helpers
{
    public class MotionLockCapture
    {
        private IDevice _device;
        private IService _buttons;
        private ICharacteristic button;
        private IService _acelerometer;
        private IService _gyroscope;
        private bool Intent = false;

        public IList<IService> Services { get; private set; }
        public MotionLockCapture(IDevice device)
        {
            _device = device;
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
                if (current_service.Id.ToString().Substring(0,8).CompareTo("0000ffe1") == 0)
                {
                    _buttons = current_service;
                }
                else if (current_service.Id.ToString().Substring(0, 8).CompareTo("0000aa51") == 0)
                {
                    _gyroscope = current_service;
                }
            }
            CapturePassword();
        }

        private void IntentFound(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
            if (button.Value.Equals(1) || button.Value.Equals(2) || button.Value.Equals(3))
            {
                Intent = true;
                //COLLECT DATA                                                  `
            }else if (button.Value.Equals(0) && Intent)
            {
                Intent = false;
                //INTERPERET DATA FROM DEVICE
            }
        }


    }
       
}
