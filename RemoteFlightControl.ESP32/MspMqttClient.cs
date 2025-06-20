#nullable enable

using System;
using System.Text;
using System.IO.Ports;
using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.Hardware.Esp32;
using nanoFramework.M2Mqtt.Messages;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace RemoteFlightControl.ESP32
{
    public static class MspMqttClient
    {
        private static readonly MqttClient InternalClient;
        private static readonly string DeviceId = Guid.NewGuid().ToString();
        private static readonly ManualResetEvent ReadySignal = new(false);
        private static readonly SerialPort MspPort = new("COM2", 115200);
        private static readonly byte[] RequestBuffer = new byte[256];
        private static readonly byte[] ResponseHeader = new byte[4];
        private static readonly byte[] ResponseBuffer = new byte[256];

        static MspMqttClient()
        {
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);

            MspPort.ReadTimeout = 50;
            MspPort.WriteTimeout = 50;
            RequestBuffer[0] = (byte)'$';
            RequestBuffer[1] = (byte)'M';
            RequestBuffer[2] = (byte)'<';

            X509Certificate cert = new(Resources.GetBytes(Resources.BinaryResources.HiveCert));
            InternalClient = new MqttClient("2184fd25ec7349aca4e1d88598347238.s1.eu.hivemq.cloud", 8883, true, cert, null, MqttSslProtocols.TLSv1_2)
            {
                ProtocolVersion = MqttProtocolVersion.Version_5
            };
        }

        public static void Start()
        {
            MspPort.Open();
            InternalClient.MqttMsgPublishReceived += static delegate(object sender, MqttMsgPublishEventArgs event_args)
            {
                try
                {
                    string client_id = event_args.Topic.Split('/')[0];
                    byte[] buffer = event_args.Message;
                    ResponseBuffer[0] = buffer[0];

                    int response_length = 1;
                    for(int index = 1; index < buffer.Length;)
                    {
                        int length = buffer[index] + 3;
                        Array.Copy(buffer, index, RequestBuffer, 3, length);
                        MspPort.Write(RequestBuffer, 0, length + 3);
                        index += length;

                        MspPort.Read(ResponseHeader, 0, 4);
                        length = ResponseHeader[3] + 2;
                        ResponseBuffer[response_length++] = (byte)(length - 2);
                        MspPort.Read(RequestBuffer, response_length, length);
                        response_length += length;
                    }

                    buffer = new byte[response_length];
                    Array.Copy(ResponseBuffer, 0, buffer, 0, response_length);
                    InternalClient.Publish($"{client_id}/{DeviceId}/responses", buffer);
                }
                catch { }
            };
            new Thread(ConnectionHandler).Start();
            new Thread(AdvertisingHandler).Start();

            [DoesNotReturn] static void ConnectionHandler()
            {
                string[] topic = new string[] { $"+/{DeviceId}/requests" };
                MqttQoSLevel[] qos = new MqttQoSLevel[] { MqttQoSLevel.AtMostOnce };
                while(true)
                {
                    try
                    {
                        if(InternalClient.IsConnected is true)
                        {
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            ReadySignal.Reset();
                            InternalClient.Connect(DeviceId, "NF-FC", "NF-FCx00", true, 60);
                            InternalClient.Subscribe(topic, qos);
                            ReadySignal.Set();
                        }
                    }
                    catch { }
                }
            }
            [DoesNotReturn] static void AdvertisingHandler()
            {
                byte[] message = Encoding.UTF8.GetBytes(DeviceId);
                while(true)
                {
                    ReadySignal.WaitOne();
                    try
                    {
                        InternalClient.Publish("NF-FC", message, null, null, MqttQoSLevel.AtLeastOnce, false);
                        Thread.Sleep(5000);
                    }
                    catch { }
                }
            }
        }
    }
}