using MQTTnet;
using System.Buffers;
using MQTTnet.Protocol;
using RemoteFlightControl.BackEnd.Msp;

namespace RemoteFlightControl.BackEnd;

public class MspMqttClient
{
    private readonly string ClientId = Guid.NewGuid().ToString();
    private readonly IMqttClient InternalClient = new MqttClientFactory().CreateMqttClient();
    private readonly HashSet<MspClient> AvailableDevices = [];
    private readonly Dictionary<byte, TaskCompletionSource<IMspResponse?>[]> MessageDictionary = [];
    private byte MessageId = 0;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    public event Action<MspMqttClient, MspClient>? NewClientFound;

    internal void SendMessage(string device_id, IEnumerable<byte> message, TaskCompletionSource<IMspResponse?>[] responses)
    {
        byte message_id;
        lock(MessageDictionary)
        {
            message_id = MessageId++;
            while(MessageDictionary.TryAdd(message_id, responses) is false)
            {
                message_id += 1;
            }
        }
        message = message.Prepend(message_id);
        InternalClient.PublishBinaryAsync($"{ClientId}/{device_id}/requests", message);
        Task.Delay(Timeout).ContinueWith(delegate
        {
            TaskCompletionSource<IMspResponse?>[]? task_sources;
            lock(MessageDictionary)
            {
                MessageDictionary.Remove(message_id, out task_sources);
            }
            if(task_sources is null)
            {
                return;
            }
            foreach(TaskCompletionSource<IMspResponse?> task_source in task_sources)
            {
                task_source.TrySetResult(null);
            }
        });
    }
    public async ValueTask Start()
    {
        MqttClientFactory factory = new();
        MqttClientOptionsBuilder builder = factory.CreateClientOptionsBuilder();
        builder.WithTcpServer("2184fd25ec7349aca4e1d88598347238.s1.eu.hivemq.cloud", 8883);
        builder.WithTlsOptions(tls =>
        {
            tls.UseTls();
            tls.WithAllowUntrustedCertificates();
            tls.WithSslProtocols(System.Security.Authentication.SslProtocols.Tls12);
        });
        builder.WithClientId(ClientId);
        builder.WithCredentials("CS-MC", "CS-MCx00");
        builder.WithNoKeepAlive();

        InternalClient.ApplicationMessageReceivedAsync += delegate(MqttApplicationMessageReceivedEventArgs event_args)
        {
            return Task.Run(HandleConnectionMessage);

            void HandleConnectionMessage()
            {
                if(event_args.ApplicationMessage.Topic is not "NF-FC")
                {
                    return;
                }
                string device_id = event_args.ApplicationMessage.ConvertPayloadToString();
                MspClient client = new(device_id, this);
                AvailableDevices.Add(client);
                NewClientFound?.Invoke(this, client);
            }
        };
        InternalClient.ApplicationMessageReceivedAsync += delegate(MqttApplicationMessageReceivedEventArgs event_args)
        {
            return Task.Run(HandleMspResponse);

            void HandleMspResponse()
            {
                if(event_args.ApplicationMessage.Topic is "NF-FC")
                {
                    return;
                }
                byte[] message = event_args.ApplicationMessage.Payload.ToArray();
                TaskCompletionSource<IMspResponse?>[]? task_sources;
                lock(MessageDictionary)
                {
                    MessageDictionary.Remove(message[0], out task_sources);
                }
                if(task_sources is null)
                {
                    return;
                }
                for(int index = 1, task_index = 0; index < message.Length; index += message[index] + 3, task_index++)
                {
                    MspData msp_data = new(message, index);
                    IMspResponse response = IMspResponse.Create(msp_data);
                    task_sources[task_index].TrySetResult(response);
                }
            }
        };

        await InternalClient.ConnectAsync(builder.Build());
        await InternalClient.SubscribeAsync("NF-FC", MqttQualityOfServiceLevel.AtLeastOnce);
        await InternalClient.SubscribeAsync($"{ClientId}/+/responses", MqttQualityOfServiceLevel.AtMostOnce);
    }
}