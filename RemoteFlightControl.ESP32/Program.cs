#nullable enable

using System.Threading;
using nanoFramework.Networking;

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