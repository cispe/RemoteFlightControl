using RemoteFlightControl.MqttClient.MSP.Requests;

namespace RemoteFlightControl.MqttClient.MSP;

public interface IMSP_Request : IEnumerable<byte>
{
    #region Static
    public static IMSP_Request Create(MSP_Command command)
    {
        switch(command)
        {
            case MSP_Command.STATUS:
            {
                return new MSP_Request_Status();
            }
            case MSP_Command.RAW_IMU:
            {
                return new MSP_Request_RawImu();
            }
            case MSP_Command.MOTOR:
            {
                return new MSP_Request_Motor();
            }
            case MSP_Command.RAW_GPS:
            {
                return new MSP_Request_RawGPS();
            }
            case MSP_Command.ATTITUDE:
            {
                return new MSP_Request_Attitude();
            }
            case MSP_Command.ALTITUDE:
            {
                return new MSP_Request_Altitude();
            }
            case MSP_Command.ANALOG:
            {
                return new MSP_Request_Analog();
            }
            default:
            {
                return default;
            }
        }
    }
    #endregion

    #region Instance
    public abstract void Prepare();
    #endregion
}