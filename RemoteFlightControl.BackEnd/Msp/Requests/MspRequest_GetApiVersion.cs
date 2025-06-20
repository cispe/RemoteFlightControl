using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Requests;

public class MspRequest_GetApiVersion : IMspRequest
{
    #region Instance
    private readonly MspData MspData = new(0, MspCommand.GetApiVersion);
    #endregion

    #region IMspRequest
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