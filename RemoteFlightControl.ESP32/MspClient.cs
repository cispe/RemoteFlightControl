#nullable enable

using System;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using nanoFramework.Hardware.Esp32;
using System.Diagnostics.CodeAnalysis;

namespace RemoteFlightControl.ESP32
{
    public static class MspClient
    {
        private static readonly SerialPort MspPort = new("COM2", 115200);
        private static readonly byte[] RequestBuffer = new byte[256];
        private static readonly byte[] ResponseHeader = new byte[4];
        private static readonly byte[] ResponseBuffer = new byte[256];
        private static readonly ConcurrentQueue RequestQueue = new();
        private static readonly ConcurrentQueue ResponseQueue = new();

        static MspClient()
        {
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);

            MspPort.ReadTimeout = 50;
            MspPort.WriteTimeout = 50;
            RequestBuffer[0] = (byte)'$';
            RequestBuffer[1] = (byte)'M';
            RequestBuffer[2] = (byte)'<';
        }

        public static void Start()
        {
            MspPort.Open();
            new Thread(MessageHandler).Start();

            [DoesNotReturn] static void MessageHandler()
            {
                while(true)
                {
                    try
                    {
                        var entry = (DictionaryEntry)RequestQueue.Dequeue();
                        byte[] buffer = (byte[])entry.Value;
                        ResponseBuffer[0] = buffer[0];

                        int response_length = 1;
                        for(int index = 1; index < buffer.Length;)
                        {
                            int length = buffer[index] + 3;
                            Array.Copy(buffer, index, RequestBuffer, 3, length);
                            MspPort.Write(RequestBuffer, 0, length + 3);
                            index += length;

                            MspPort.Read(ResponseHeader, 0, 4);
                            length = ResponseHeader[3] + 2;
                            ResponseBuffer[response_length++] = (byte)(length - 2);
                            MspPort.Read(RequestBuffer, response_length, length);
                            response_length += length;
                        }

                        buffer = new byte[response_length];
                        Array.Copy(ResponseBuffer, 0, buffer, 0, response_length);
                        entry.Value = buffer;
                        ResponseQueue.Enqueue(entry);
                    }
                    catch { }
                }
            }
        }

        #region MessageManagement
        public static void EnqueueRequest(string sender, byte[] message)
        {
            DictionaryEntry entry = new(sender, message);
            RequestQueue.Enqueue(entry);
        }
        public static DictionaryEntry DequeueResponse()
        {
            return (DictionaryEntry)ResponseQueue.Dequeue();
        }
        #endregion
    }
}