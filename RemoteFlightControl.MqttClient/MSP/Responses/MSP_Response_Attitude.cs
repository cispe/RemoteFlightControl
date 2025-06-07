using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

public class MSP_Response_Attitude : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_Attitude(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public short Roll
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.ReadUnaligned<short>(ref start);
        }
    }
    public short Pitch
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte pitch = ref Unsafe.Add(ref start, 2);
            return Unsafe.ReadUnaligned<short>(ref pitch);
        }
    }
    public short Yaw
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte yaw = ref Unsafe.Add(ref start, 4);
            return Unsafe.ReadUnaligned<short>(ref yaw);
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