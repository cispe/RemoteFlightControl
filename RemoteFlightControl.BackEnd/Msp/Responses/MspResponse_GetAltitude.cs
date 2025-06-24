using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Responses;

public class MspResponse_GetAltitude(MspData MspData) : IMspResponse
{
    #region Instance
    public int EstimatedAltitude
    {
        get => BitConverter.ToInt32(MspData.Payload[0..4]);
    }
    public short EstimatedVariation
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