using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace ProjekatAndrejAntanaskovic.Services
{
    public static class DeviceService
    {
        private const string DeviceIdKey = "UserDeviceIdKey";

        public static string GetDeviceId()
        {
            string deviceId = Preferences.Get(DeviceIdKey, string.Empty);

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();
                Preferences.Set(DeviceIdKey, deviceId);
            }

            return deviceId;
        }
    }
}