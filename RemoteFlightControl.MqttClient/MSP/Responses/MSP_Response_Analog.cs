using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

public class MSP_Response_Analog : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_Analog(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public byte Voltage
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return start;
        }
    }
    public ushort mAhDrawn
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte mah = ref Unsafe.Add(ref start, 1);
            return Unsafe.ReadUnaligned<ushort>(ref mah);
        }
    }
    public ushort RSSI
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte rssi = ref Unsafe.Add(ref start, 3);
            return Unsafe.ReadUnaligned<ushort>(ref rssi);
        }
    }
    public short Amperage
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte amperage = ref Unsafe.Add(ref start, 5);
            return Unsafe.ReadUnaligned<short>(ref amperage);
        }
    }
    public ushort BatteryVoltage
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte batV = ref Unsafe.Add(ref start, 7);
            return Unsafe.ReadUnaligned<ushort>(ref batV);
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