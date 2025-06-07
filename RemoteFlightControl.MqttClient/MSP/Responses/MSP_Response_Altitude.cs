using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

public class MSP_Response_Altitude : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_Altitude(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public int Altitude
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.ReadUnaligned<int>(ref start);
        }
    }
    public short Variance
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte variance = ref Unsafe.Add(ref start, 4);
            return Unsafe.ReadUnaligned<short>(ref variance);
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