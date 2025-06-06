namespace RemoteFlightControl.MqttClient.MSP.Requests;

public class MspRequest_Altitude : IMspRequest
{
    #region Static
    public const int PayloadLength = 0;
    #endregion

    #region Instance
    public PayloadX Data { get; }

    public MspRequest_Altitude()
    {
        Data = new PayloadX(PayloadLength);
        Data.MspCommand = MspCommand.ALTITUDE;
    }
    public MspRequest_Altitude(PayloadX payload_x)
    {
        Data = payload_x;
    }
    #endregion
}