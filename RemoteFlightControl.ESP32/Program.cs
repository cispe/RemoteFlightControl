#nullable enable

using System.Threading;
using System.Diagnostics;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;

namespace RemoteFlightControl.ESP32
{
    public static class Program
    {
        public static void Main()
        {
            WifiNetworkHelper.SetupNetworkHelper("CISPE-PHONE", "11111111");
            WifiNetworkHelper.NetworkReady.WaitOne();
            MspMqttClient.Start();
            while(true)
            {
                Debug.WriteLine($"FreeMemory: {GC.Run(true)}");
                Thread.Sleep(2000);
            }
        }
    }
}