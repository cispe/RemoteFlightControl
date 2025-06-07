using System.Collections;

namespace RemoteFlightControl.MqttClient.MSP.Requests;

public class MSP_Request_Altitude : IMSP_Request
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    public MSP_Request_Altitude()
    {
        MSP_Data = new MSP_Data(0)
        {
            Command = MSP_Command.ALTITUDE
        };
    }
    #endregion

    #region IMSP_Request
    public void Prepare()
    {
        MSP_Data.InitChecksum();
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