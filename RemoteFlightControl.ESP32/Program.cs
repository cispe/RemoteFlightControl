#nullable enable

using System;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using nanoFramework.M2Mqtt;
using nanoFramework.Networking;
using nanoFramework.Hardware.Esp32;
using nanoFramework.M2Mqtt.Messages;
using System.Security.Cryptography.X509Certificates;

namespace RemoteFlightControl.ESP32
{
    public static class Program
    {
        private static void MqttConnect(MqttClient client, string device_id)
        {
            Reconnect: try
            {
                client.Connect(device_id, "NF-FC", "NF-FCx00", true, 60);
                client.Publish("NF-FC", Encoding.UTF8.GetBytes(device_id));
                client.Subscribe(new string[] { $"+/{device_id}/requests" }, new MqttQoSLevel[] { MqttQoSLevel.AtMostOnce });
            }
            catch
            {
                goto Reconnect;
            }
        }
        public static void Main()
        {
            WifiNetworkHelper.SetupNetworkHelper("CISPE-PHONE", "11111111");
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);
            var msp_port = new SerialPort("COM2", 115200);
            msp_port.Open();

            var device_id = Guid.NewGuid().ToString();
            var hive_cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.HiveCert));
            WifiNetworkHelper.NetworkReady.WaitOne();
            var mqtt_client = new MqttClient("2184fd25ec7349aca4e1d88598347238.s1.eu.hivemq.cloud", 8883, true, hive_cert, null, MqttSslProtocols.TLSv1_2);

            byte[] request_buffer = new byte[256];
            request_buffer[0] = (byte)'$';
            request_buffer[1] = (byte)'M';
            request_buffer[2] = (byte)'<';
            byte[] response_header = new byte[4];
            byte[] response_buffer = new byte[256];
            mqtt_client.MqttMsgPublishReceived += delegate(object sender, MqttMsgPublishEventArgs event_args)
            {
                string sender_id = event_args.Topic.Split('/')[0];
                response_buffer[0] = event_args.Message[0];
                int response_length = 1;
                for(int index = 1; index < event_args.Message.Length;)
                {
                    int length = event_args.Message[index] + 3;
                    Array.Copy(event_args.Message, index, request_buffer, 3, length);
                    msp_port.Write(request_buffer, 0, length + 3);
                    index += length;

                    msp_port.Read(response_header, 0, 4);
                    length = response_header[3] + 2;
                    response_buffer[response_length++] = (byte)(length - 2);
                    msp_port.Read(response_buffer, response_length, length);
                    response_length += length;
                }
                byte[] response = new byte[response_length];
                Array.Copy(response_buffer, 0, response, 0, response_length);
                try
                {
                    mqtt_client.Publish($"{sender_id}/{device_id}/responses", response);
                }
                catch
                {
                    MqttConnect(mqtt_client, device_id);
                }
            };
            MqttConnect(mqtt_client, device_id);
            while(true)
            {
                Debug.WriteLine($"FreeMemory: {nanoFramework.Runtime.Native.GC.Run(true)}");
                Thread.Sleep(30000);
            }
        }
    }
}