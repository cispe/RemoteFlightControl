#nullable enable

using System;
using System.Text;
using System.Threading;
using System.Collections;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace RemoteFlightControl.ESP32
{
    public static class MspMqttClient
    {
        private static readonly string DeviceId = Guid.NewGuid().ToString();
        private static readonly X509Certificate HiveCert = new(Resources.GetBytes(Resources.BinaryResources.HiveCert));
        private static readonly MqttClient InternalClient = new("2184fd25ec7349aca4e1d88598347238.s1.eu.hivemq.cloud", 8883, true, HiveCert, null, MqttSslProtocols.TLSv1_2);
        private static readonly ManualResetEvent ReadySignal = new(false);

        public static void Start()
        {
            new Thread(ConnectionHandler).Start();
            new Thread(AdvertisingHandler).Start();
            new Thread(MessageHandler).Start();

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
                ArrayList user_properties = new();
                while(true)
                {
                    ReadySignal.WaitOne();
                    try
                    {
                        InternalClient.Publish("NF-FC", message, "", user_properties, MqttQoSLevel.AtLeastOnce, false);
                        Thread.Sleep(5000);
                    }
                    catch { }
                }
            }
            [DoesNotReturn] static void MessageHandler()
            {
                InternalClient.MqttMsgPublishReceived += delegate(object sender, MqttMsgPublishEventArgs event_args)
                {
                    MspClient.EnqueueRequest(event_args.Topic.Split('/')[0], event_args.Message);
                };
                while(true)
                {
                    try
                    {
                        ReadySignal.WaitOne();
                        DictionaryEntry entry = MspClient.DequeueResponse();
                        InternalClient.Publish($"{entry.Key}/{DeviceId}/responses", (byte[])entry.Value);
                    }
                    catch { }
                }
            }
        }
    }
}