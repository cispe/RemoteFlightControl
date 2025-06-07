using MQTTnet;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using RemoteFlightControl.MqttClient.MSP;

namespace RemoteFlightControl.MqttClient;

public class MSP_MQTT_Client(MqttClientFactory factory)
{
    #region Static
    public static MqttClientOptions GetMqttOptions(MqttClientFactory factory)
    {
        MqttClientOptionsBuilder options_builder = factory.CreateClientOptionsBuilder();
        options_builder = options_builder.WithTcpServer("33c797ca3ef9400da94893d7b115f6a8.s1.eu.hivemq.cloud", 8883);
        options_builder = options_builder.WithCredentials("RFC_Client", "RFCx00000000");
        options_builder = options_builder.WithClientId("RFCC");
        options_builder = options_builder.WithCleanSession(false);
        options_builder = options_builder.WithNoKeepAlive();
        options_builder = options_builder.WithTlsOptions(tls_builder =>
        {
            tls_builder = tls_builder.UseTls();
            tls_builder = tls_builder.WithSslProtocols(System.Security.Authentication.SslProtocols.Tls12);
            tls_builder = tls_builder.WithAllowUntrustedCertificates();
        });
        return options_builder.Build();
    }
    public static async ValueTask<MSP_MQTT_Client> CreateAndConnectAsync(MqttClientFactory factory)
    {
        MSP_MQTT_Client client = new(factory);
        MqttClientOptions options = GetMqttOptions(factory);
        await client.ConnectAndSubscribeAsync(options);
        return client;
    }
    #endregion

    #region Instance
    private readonly IMqttClient MqttClient = factory.CreateMqttClient();
    private readonly MQTT_MessageHandler MessageHandler = new();

    public TimeSpan LoopDuration { get; set; } = TimeSpan.FromMilliseconds(500);

    private async Task ConnectAndSubscribeAsync(MqttClientOptions options)
    {
        await MqttClient.ConnectAsync(options);
        await MqttClient.SubscribeAsync("flight_controller/response");
        MqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
    }
    private Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs event_args)
    {
        return Task.Run(() =>
        {
            MessageHandler.ResolveMessage(event_args.ApplicationMessage.Payload.ToArray());
        });
    }
    public Task<IMSP_Response?> SendAsync(IMSP_Request msp_request)
    {
        return MessageHandler.EnqueueRequest(msp_request);
    }
    [DoesNotReturn] public void SendLoop()
    {
        while(true)
        {
            if(MessageHandler.IsEmpty is true)
            {
                Thread.Sleep(LoopDuration);
                continue;
            }
            IEnumerable<byte> message = MessageHandler.FlushMessage();
            MqttClient.PublishBinaryAsync("flight_controller/request", message);
            Thread.Sleep(LoopDuration);
        }
    }
    #endregion
}