using RemoteFlightControl.BackEnd.Msp;
using System.Diagnostics.CodeAnalysis;

namespace RemoteFlightControl.BackEnd;

public class MspClient
{
    private CancellationTokenSource? CancellationSource;
    private readonly List<IMspRequest> RequestList = [];
    private readonly List<TaskCompletionSource<IMspResponse?>> ResponseList = [];

    public string DeviceId { get; }
    public MspMqttClient MspMqttClient { get; }
    public bool IsOnline { get; private set; } = true;
    public TimeSpan SendPeriod { get; set; } = TimeSpan.FromMilliseconds(50);

    public event Action<MspClient>? ClientBecameOnline;
    public event Action<MspClient>? ClientBecameOffline;

    public MspClient(string device_id, MspMqttClient msp_mqtt_client)
    {
        DeviceId = device_id;
        MspMqttClient = msp_mqtt_client;
        new Thread(MessageHandler).Start();
        OnlineConfirmed();

        [DoesNotReturn] void MessageHandler()
        {
            while(true)
            {
                Thread.Sleep(SendPeriod);
                IEnumerable<byte> message;
                TaskCompletionSource<IMspResponse?>[] responses;
                lock(RequestList)
                lock(ResponseList)
                {
                    if(RequestList.Count is 0)
                    {
                        continue;
                    }
                    message = RequestList.SelectMany(request => request);
                    responses = ResponseList.ToArray();
                    RequestList.Clear();
                    ResponseList.Clear();
                }
                MspMqttClient.SendMessage(DeviceId, message, responses);
            }
        }
    }

    internal void OnlineConfirmed()
    {
        CancellationSource?.Cancel();
        if(IsOnline is false)
        {
            IsOnline = true;
            ClientBecameOnline?.Invoke(this);
        }
        CancellationSource = new CancellationTokenSource();
        Task.Delay(7000, CancellationSource.Token).ContinueWith(delegate
        {
            IsOnline = false;
            ClientBecameOffline?.Invoke(this);
        });
    }
    public Task<IMspResponse?> SendReceive(IMspRequest request)
    {
        TaskCompletionSource<IMspResponse?> task_source = new();
        lock(RequestList)
        lock(ResponseList)
        {
            RequestList.Add(request);
            ResponseList.Add(task_source);
        }
        return task_source.Task;
    }
}