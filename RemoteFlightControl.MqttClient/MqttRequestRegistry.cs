using System.Collections.Concurrent;
using RemoteFlightControl.MqttClient.MSP;

namespace RemoteFlightControl.MqttClient;

/// <summary>
/// Manages the registration and tracking of pending MSP requests by message ID,
/// providing timeout handling and thread-safe message ID generation
/// </summary>
public class MqttRequestRegistry
{
    /// <summary>
    /// Represents the unique identifier for a message in the protocol
    /// </summary>
    /// <remarks>
    /// The identifier is stored as an <see langword="int"/> to support atomic operations. It is cast
    /// to <see langword="ushort"/> when used in the protocol to conform to protocol requirements
    /// </remarks>
    private int MessageID;

    /// <summary>
    /// Maps message IDs to their corresponding TaskCompletionSource for awaiting responses
    /// </summary>
    public ConcurrentDictionary<ushort, TaskCompletionSource<IMspResponse?>> MessageID_ResponseTask_Dict { get; } = new();

    /// <summary>
    /// Gets or sets the timeout for awaiting a response
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Registers a new request and returns a unique message ID and the associated task
    /// </summary>
    /// <param name="task">The task to await the response</param>
    /// <returns>The unique message ID for the request</returns>
    public ushort AddNew(out Task<IMspResponse?> task)
    {
        TaskCompletionSource<IMspResponse?> task_source = new(TaskCreationOptions.RunContinuationsAsynchronously);
        ushort messageId;
        do
        {
            messageId = (ushort)Interlocked.Increment(ref MessageID);
        }
        while(MessageID_ResponseTask_Dict.TryAdd(messageId, task_source) is false);
        Task.Delay(Timeout).ContinueWith(task =>
        {
            if(task_source.TrySetResult(null) is true)
            {
                MessageID_ResponseTask_Dict.TryRemove(messageId, out _);
            }
        });
        
        task = task_source.Task;
        return messageId;
    }

    /// <summary>
    /// Completes the pending request for the given message ID with the provided response
    /// </summary>
    /// <param name="message_id">The message ID of the request</param>
    /// <param name="response">The response to set</param>
    /// <returns><see langword="true"/> if the request was found and completed; otherwise, <see langword="false"/></returns>
    public bool Complete(ushort message_id, IMspResponse? response)
    {
        if(MessageID_ResponseTask_Dict.TryRemove(message_id, out TaskCompletionSource<IMspResponse?>? task_source) is true)
        {
            return task_source.TrySetResult(response);
        }
        return false;
    }
}