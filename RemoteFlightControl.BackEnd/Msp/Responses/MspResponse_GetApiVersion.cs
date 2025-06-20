using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Responses;

public class MspResponse_GetApiVersion(MspData MspData) : IMspResponse
{
    #region Instance
    public byte ProtocolVersion
    {
        get => MspData.Payload[0];
    }
    public byte ApiMajor
    {
        get => MspData.Payload[1];
    }
    public byte ApiMinor
    {
        get => MspData.Payload[2];
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