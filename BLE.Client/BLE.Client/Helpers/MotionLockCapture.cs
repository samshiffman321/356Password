using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.BLE.Abstractions.Contracts;

namespace BLE.Client.Helpers
{
    public class MotionLockCapture
    {
        private IDevice _device;
        private IService _buttons;
        private IService _acelerometer;
        private IService _gyroscope;

        public IList<IService> Services { get; private set; }
        public MotionLockCapture(IDevice device)
        {
            _device = device;
            LoadServices();
            foreach (IService current_service in Services)
            {
                if (current_service.Id.ToString().CompareTo("FFE1") == 0)
                {
                    _buttons = current_service;
                }
                else if (current_service.Id.ToString().CompareTo("AA51") == 0)
                {
                    _gyroscope = current_service;
                }
            }
            CapturePassword();
        }

        private async void CapturePassword()
        {
            IList<ICharacteristic> button = await _buttons.GetCharacteristicsAsync();
            
        }

        private async void LoadServices()
        {
            Services = await _device.GetServicesAsync();
        }




    }
       
}
