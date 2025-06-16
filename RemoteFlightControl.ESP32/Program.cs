#nullable enable

using System;
using System.IO.Ports;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.Networking;
using nanoFramework.Hardware.Esp32;
using System.Security.Cryptography.X509Certificates;

namespace RemoteFlightControl.ESP32
{
    public class Program
    {
        public static void Main()
        {
            WifiNetworkHelper.SetupNetworkHelper("CISPE-PHONE", "11111111");
            MspClient.Start();
            WifiNetworkHelper.NetworkReady.WaitOne();
            MspMqttClient.Start();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}