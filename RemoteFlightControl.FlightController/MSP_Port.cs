#nullable enable

using System;
using System.IO.Ports;
using nanoFramework.Hardware.Esp32;

namespace RemoteFlightControl.FlightController
{
    public class MSP_Port
    {
        private readonly SerialPort SerialPort;
        private readonly byte[] MSP_SendBuffer;
        private readonly byte[] MSP_HeadBuffer;
        private readonly byte[] MSP_RecvBuffer;

        public MSP_Port()
        {
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);

            SerialPort = new SerialPort("COM2", 115200)
            {
                ReadBufferSize = 512,
                WriteBufferSize = 512,
                ReadTimeout = 500,
                WriteTimeout = 500,
            };
            SerialPort.Open();

            MSP_SendBuffer = new byte[512];
            MSP_SendBuffer[0] = (byte)'$';
            MSP_SendBuffer[1] = (byte)'M';
            MSP_SendBuffer[2] = (byte)'<';

            MSP_HeadBuffer = new byte[4];
            MSP_RecvBuffer = new byte[512];
        }

        public byte[] SendReceive(byte[] input_message)
        {
            int output_index = 0;
            for(int input_index = 2; input_index < input_message.Length;)
            {
                int msp_data_length = input_message[input_index] + 3;
                Array.Copy(input_message, input_index, MSP_SendBuffer, 3, msp_data_length);
                SerialPort.Write(MSP_SendBuffer, 0, msp_data_length + 3);
                input_index += msp_data_length;

                SerialPort.Read(MSP_HeadBuffer, 0, MSP_HeadBuffer.Length);
                msp_data_length = MSP_HeadBuffer[3];
                MSP_RecvBuffer[output_index++] = (byte)msp_data_length;
                SerialPort.Read(MSP_RecvBuffer, output_index, msp_data_length + 2);
                output_index += msp_data_length + 2;
            }
            byte[] output_message = new byte[output_index + 2];
            Array.Copy(input_message, 0, output_message, 0, 2);
            Array.Copy(MSP_RecvBuffer, 0, output_message, 2, output_index);
            return output_message;
        }
    }
}