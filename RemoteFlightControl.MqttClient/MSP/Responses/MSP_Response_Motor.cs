using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

public class MSP_Response_Motor : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_Motor(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public ushort Motor1
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.ReadUnaligned<ushort>(ref start);
        }
    }
    public ushort Motor2
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor2 = ref Unsafe.Add(ref start, 2);
            return Unsafe.ReadUnaligned<ushort>(ref motor2);
        }
    }
    public ushort Motor3
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor3 = ref Unsafe.Add(ref start, 4);
            return Unsafe.ReadUnaligned<ushort>(ref motor3);
        }
    }
    public ushort Motor4
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor4 = ref Unsafe.Add(ref start, 6);
            return Unsafe.ReadUnaligned<ushort>(ref motor4);
        }
    }
    public ushort Motor5
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor5 = ref Unsafe.Add(ref start, 8);
            return Unsafe.ReadUnaligned<ushort>(ref motor5);
        }
    }
    public ushort Motor6
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor6 = ref Unsafe.Add(ref start, 10);
            return Unsafe.ReadUnaligned<ushort>(ref motor6);
        }
    }
    public ushort Motor7
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor7 = ref Unsafe.Add(ref start, 12);
            return Unsafe.ReadUnaligned<ushort>(ref motor7);
        }
    }
    public ushort Motor8
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte motor8 = ref Unsafe.Add(ref start, 14);
            return Unsafe.ReadUnaligned<ushort>(ref motor8);
        }
    }
    #endregion

    #region IEnumerable<byte>
    public IEnumerator<byte> GetEnumerator()
    {
        return MSP_Data.GetEnumerator();
    }
    #endregion

    #region IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
}