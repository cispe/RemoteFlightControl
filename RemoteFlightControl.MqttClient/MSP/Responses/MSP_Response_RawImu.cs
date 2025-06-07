using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

public class MSP_Response_RawImu : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_RawImu(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public short AccelerationX
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.ReadUnaligned<short>(ref start);
        }
    }
    public short AccelerationY
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte acc_y = ref Unsafe.Add(ref start, 2);
            return Unsafe.ReadUnaligned<short>(ref acc_y);
        }
    }
    public short AccelerationZ
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte acc_z = ref Unsafe.Add(ref start, 4);
            return Unsafe.ReadUnaligned<short>(ref acc_z);
        }
    }
    public short GyroscopeX
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte gyro_x = ref Unsafe.Add(ref start, 6);
            return Unsafe.ReadUnaligned<short>(ref gyro_x);
        }
    }
    public short GyroscopeY
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte gyro_y = ref Unsafe.Add(ref start, 8);
            return Unsafe.ReadUnaligned<short>(ref gyro_y);
        }
    }
    public short GyroscopeZ
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte gyro_z = ref Unsafe.Add(ref start, 10);
            return Unsafe.ReadUnaligned<short>(ref gyro_z);
        }
    }
    public short MagnitudeX
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte mag_x = ref Unsafe.Add(ref start, 12);
            return Unsafe.ReadUnaligned<short>(ref mag_x);
        }
    }
    public short MagnitudeY
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte mag_y = ref Unsafe.Add(ref start, 14);
            return Unsafe.ReadUnaligned<short>(ref mag_y);
        }
    }
    public short MagnitudeZ
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte mag_z = ref Unsafe.Add(ref start, 16);
            return Unsafe.ReadUnaligned<short>(ref mag_z);
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