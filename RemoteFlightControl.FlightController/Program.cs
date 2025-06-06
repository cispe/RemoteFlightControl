#nullable enable

using System.IO.Ports;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.Networking;
using nanoFramework.Hardware.Esp32;
using nanoFramework.M2Mqtt.Messages;
using System.Security.Cryptography.X509Certificates;

namespace RemoteFlightControl.FlightController
{
    public static class Program
    {
        public static SerialPort SerialPort = null!;
        public static MqttClient MqttClient = null!;

        private static void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs event_args)
        {
            byte[] request = new byte[event_args.Message.Length + 2];
            request[0] = (byte)'$';
            request[1] = (byte)'M';
            request[2] = (byte)'<';
            request[3] = (byte)(event_args.Message.Length - 4);
            for(int index = event_args.Message.Length - 1; index >= 2; index--)
            {
                request[index + 2] = event_args.Message[index];
            }
            SerialPort.Write(request, 0, request.Length);

            while(SerialPort.BytesToRead < 4) ;
            byte[] response_header = new byte[4];
            SerialPort.Read(response_header, 0, response_header.Length);
            byte[] response = new byte[response_header[3] + 4];
            response[0] = event_args.Message[0];
            response[1] = event_args.Message[1];
            SerialPort.Read(response, 2, response.Length - 2);

            try
            {
                MqttClient.Publish(topic: "flight_controller/response", message: response);
            }
            catch { }
        }

        private static void InitSerialPort()
        {
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);
            SerialPort = new SerialPort(portName: "COM2", baudRate: 115200);
            SerialPort.Open();
        }
        private static void InitWifi()
        {
            WifiNetworkHelper.SetupNetworkHelper("CISPE-PHONE", "11111111");
            WifiNetworkHelper.NetworkReady.WaitOne();
        }
        private static void InitMqttClient()
        {
            X509Certificate cert = new(Resources.GetBytes(Resources.BinaryResources.Cert));
            MqttClient = new MqttClient(
                brokerHostName: "33c797ca3ef9400da94893d7b115f6a8.s1.eu.hivemq.cloud",
                brokerPort: 8883,
                secure: true,
                caCert: cert,
                clientCert: null,
                sslProtocol: MqttSslProtocols.TLSv1_2
            );
        }

        public static void Main()
        {
            InitSerialPort();
            InitWifi();
            InitMqttClient();

            while(true)
            {
                if(MqttClient.IsConnected is true)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                try
                {
                    MqttClient.Connect(
                        clientId: "FlightController",
                        username: "FlightController",
                        password: "FCx000001",
                        cleanSession: true,
                        keepAlivePeriod: 60
                    );
                    string[] topics = new string[] { "flight_controller/request" };
                    MqttQoSLevel[] qos = new MqttQoSLevel[] { MqttQoSLevel.AtMostOnce };
                    MqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                    MqttClient.Subscribe(topics, qos);
                }
                catch { }
            }
        }
    }
}