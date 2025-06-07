using RemoteFlightControl.MqttClient.MSP;

namespace RemoteFlightControl.MqttClient;

public class MQTT_MessageHandler
{
    #region Instance
    private readonly Dictionary<ushort, TaskCompletionSource<IMSP_Response?>[]> MessageDictionary = [];
    private readonly List<TaskCompletionSource<IMSP_Response?>> ResponseList = [];
    private List<IMSP_Request> RequestList = [];
    private readonly Lock SyncRoot = new();
    private ushort MessageID = 0;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(2);

    public bool IsEmpty
    {
        get => RequestList.Count is 0;
    }

    public Task<IMSP_Response?> EnqueueRequest(IMSP_Request request)
    {
        TaskCompletionSource<IMSP_Response?> task_source = new();
        lock(SyncRoot)
        {
            RequestList.Add(request);
            ResponseList.Add(task_source);
        }
        return task_source.Task;
    }
    public IEnumerable<byte> FlushMessage()
    {
        lock(SyncRoot)
        {
            ushort message_id = MessageID++;
            MessageDictionary.Add(message_id, [..ResponseList]);
            ResponseList.Clear();
            IEnumerable<byte> message_data = RequestList.SelectMany(request =>
            {
                request.Prepare();
                return request;
            });
            RequestList = [];
            Task.Delay(Timeout).ContinueWith(task =>
            {
                TaskCompletionSource<IMSP_Response?>[]? task_sources;
                lock(SyncRoot)
                {
                    MessageDictionary.Remove(message_id, out task_sources);
                    if(task_sources is null)
                    {
                        return;
                    }
                }
                foreach(TaskCompletionSource<IMSP_Response?> task_source in task_sources)
                {
                    task_source.TrySetResult(null);
                }
            });
            return BitConverter.GetBytes(message_id).Concat(message_data);
        }
    }
    public void ResolveMessage(byte[] message)
    {
        TaskCompletionSource<IMSP_Response?>[]? task_sources;
        ushort message_id = BitConverter.ToUInt16(message, 0);
        lock(SyncRoot)
        {
            MessageDictionary.Remove(message_id, out task_sources);
            if(task_sources is null)
            {
                return;
            }
        }
        int index = 2;
        foreach(TaskCompletionSource<IMSP_Response?> task_source in task_sources)
        {
            MSP_Data msp_data = new(message, index);
            IMSP_Response response = IMSP_Response.Create(msp_data);
            task_source.SetResult(response);
            index += response.Count();
        }
    }
    #endregion
}