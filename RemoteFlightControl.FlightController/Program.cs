#nullable enable

using nanoFramework.Networking;

namespace RemoteFlightControl.FlightController
{
    public static class Program
    {
        public static void Main()
        {
            WifiNetworkHelper.SetupNetworkHelper("CISPE-PHONE", "11111111");
            MSP_Port msp_port = new();
            WifiNetworkHelper.NetworkReady.WaitOne();
            MSP_MQTT_Client msp_mqtt_client = new(msp_port);
            msp_mqtt_client.ReconnectLoop();
        }
    }
}