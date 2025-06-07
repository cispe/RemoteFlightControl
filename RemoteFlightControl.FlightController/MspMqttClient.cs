#nullable enable

using System.Threading;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace RemoteFlightControl.FlightController
{
    public class MSP_MQTT_Client
    {
        private readonly MSP_Port MSP_Port;
        private readonly MqttClient MqttClient;

        public MSP_MQTT_Client(MSP_Port msp_port)
        {
            MSP_Port = msp_port;

            X509Certificate cert = new(Resources.GetBytes(Resources.BinaryResources.Cert));
            MqttClient = new MqttClient(
                brokerHostName: "33c797ca3ef9400da94893d7b115f6a8.s1.eu.hivemq.cloud",
                    brokerPort: 8883,
                        secure: true,
                        caCert: cert,
                    clientCert: null,
                   sslProtocol: MqttSslProtocols.TLSv1_2
            );
            MqttClient.MqttMsgPublishReceived += MspRequestMessageHandler;
        }

        private void MspRequestMessageHandler(object sender, MqttMsgPublishEventArgs event_args)
        {
            try
            {
                byte[] output_message = MSP_Port.SendReceive(event_args.Message);
                MqttClient.Publish("flight_controller/response", output_message);
            }
            catch { }
        }
        [DoesNotReturn] public void ReconnectLoop()
        {
            string[] topic = new string[] { "flight_controller/request" };
            MqttQoSLevel[] qos = new MqttQoSLevel[] { MqttQoSLevel.AtMostOnce };

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
                        clientId: "FCC",
                        username: "FC_Client",
                        password: "FCx00000000",
                        cleanSession: true,
                        keepAlivePeriod: 60
                    );
                    MqttClient.Subscribe(topic, qos);
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}