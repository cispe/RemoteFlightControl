#nullable enable

using System.Threading;
using System.Collections;

namespace RemoteFlightControl.ESP32
{
    public class ConcurrentQueue
    {
        private Queue queue { get; } = new();
        private ManualResetEvent non_empty_signal { get; } = new(false);

        public void Enqueue(object item)
        {
            lock(queue)
            {
                queue.Enqueue(item);
                non_empty_signal.Set();
            }
        }
        public object Dequeue()
        {
            non_empty_signal.WaitOne();
            lock(queue)
            {
                object item = queue.Dequeue();
                if(queue.Count is 0)
                {
                    non_empty_signal.Reset();
                }
                return item;
            }
        }
        public void Clear()
        {
            lock(queue)
            {
                queue.Clear();
                non_empty_signal.Reset();
            }
        }
    }
}