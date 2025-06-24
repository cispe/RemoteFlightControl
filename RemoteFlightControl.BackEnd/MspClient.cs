using RemoteFlightControl.BackEnd.Msp;
using System.Diagnostics.CodeAnalysis;

namespace RemoteFlightControl.BackEnd;

public class MspClient
{
    private readonly List<IMspRequest> RequestList = [];
    private readonly List<TaskCompletionSource<IMspResponse?>> ResponseList = [];

    public string DeviceId { get; }
    public MspMqttClient MspMqttClient { get; }
    public TimeSpan SendPeriod { get; set; } = TimeSpan.FromMilliseconds(500);

    public MspClient(string device_id, MspMqttClient msp_mqtt_client)
    {
        DeviceId = device_id;
        MspMqttClient = msp_mqtt_client;
        new Thread(MessageHandler).Start();

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
                    message = RequestList.ToArray().SelectMany(request => request);
                    responses = ResponseList.ToArray();
                    RequestList.Clear();
                    ResponseList.Clear();
                }
                MspMqttClient.SendMessage(DeviceId, message, responses);
            }
        }
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