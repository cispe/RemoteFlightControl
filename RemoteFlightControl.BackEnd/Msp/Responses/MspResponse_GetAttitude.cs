using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Responses;

public class MspResponse_GetAttitude(MspData MspData) : IMspResponse
{
    #region Instance
    public short Roll
    {
        get => BitConverter.ToInt16(MspData.Payload[0..2]);
    }
    public short Pitch
    {
        get => BitConverter.ToInt16(MspData.Payload[2..4]);
    }
    public short Yaw
    {
        get => BitConverter.ToInt16(MspData.Payload[4..6]);
    }
    #endregion

    #region IMspResponse
    public IEnumerator<byte> GetEnumerator()
    {
        return MspData.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
}