using MQTTnet;
using RemoteFlightControl.MqttClient.MSP;
using System.Buffers;

namespace RemoteFlightControl.MqttClient;

/// <summary>
/// Handles MSP-over-MQTT communication with a flight controller, including connection, request/response, and message dispatch
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="MspMqttClient"/> and connects to the specified MQTT broker
/// </remarks>
public class MspMqttClient(MqttClientFactory factory)
{
    #region Static
    public static MqttClientOptions GetMqttOptions(MqttClientFactory factory)
    {
        MqttClientOptionsBuilder options_builder = factory.CreateClientOptionsBuilder();
        options_builder = options_builder.WithTcpServer("33c797ca3ef9400da94893d7b115f6a8.s1.eu.hivemq.cloud", 8883);
        options_builder = options_builder.WithCredentials("FlightController", "FCx000001");
        options_builder = options_builder.WithClientId("MspMqttClient");
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
    public static async ValueTask<MspMqttClient> CreateAndConnectAsync(MqttClientFactory factory)
    {
        MspMqttClient client = new(factory);
        MqttClientOptions options = GetMqttOptions(factory);
        await client.ConnectAndSubscribeAsync(options);
        return client;
    }
    #endregion

    #region Instance
    /// <summary>
    /// The underlying MQTT client
    /// </summary>
    public IMqttClient MqttClient { get; } = factory.CreateMqttClient();

    /// <summary>
    /// Registry for tracking pending MSP requests and their responses
    /// </summary>
    public MqttRequestRegistry RequestRegistry { get; } = new MqttRequestRegistry();

    /// <summary>
    /// Establishes a connection to the MQTT broker using the specified options and subscribes to the "flight_controller/response" topic
    /// </summary>
    /// <remarks>
    /// This method connects to the MQTT broker, subscribes to the "flight_controller/response"
    /// topic, and registers a handler for incoming application messages. Ensure that the <paramref name="options"/> 
    /// parameter is properly configured before calling this method
    /// </remarks>
    /// <param name="options">The MQTT client options used to configure the connection</param>
    /// <returns></returns>
    private async Task ConnectAndSubscribeAsync(MqttClientOptions options)
    {
        await MqttClient.ConnectAsync(options);
        await MqttClient.SubscribeAsync("flight_controller/response");
        MqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
    }

    /// <summary>
    /// Handles the receipt of an MQTT application message and processes it asynchronously
    /// </summary>
    /// <remarks>
    /// This method processes the payload of the received MQTT application message, creates a
    /// response based on the payload, and completes the corresponding request in the registry
    /// </remarks>
    /// <param name="eventArgs">The event arguments containing the MQTT application message and related metadata</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    private Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        return Task.Run(() =>
        {
            var payloadX = new PayloadX(eventArgs.ApplicationMessage.Payload.ToArray());
            var response = IMspResponse.Create(payloadX);
            RequestRegistry.Complete(response.Data.MessageID, response);
        });
    }

    /// <summary>
    /// Sends an MSP request over MQTT and returns a task that completes with the response.
    /// </summary>
    /// <param name="mspRequest">The MSP request to send.</param>
    /// <returns>A task that completes with the MSP response, or null if timed out.</returns>
    public Task<IMspResponse?> SendAsync(IMspRequest mspRequest)
    {
        mspRequest.Data.InitChecksum();
        mspRequest.Data.MessageID = RequestRegistry.AddNew(out var task);
        MqttClient.PublishBinaryAsync("flight_controller/request", mspRequest.Data.Data);
        return task;
    }
    #endregion
}