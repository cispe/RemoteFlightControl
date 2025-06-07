using RemoteFlightControl.MqttClient.MSP.Responses;

namespace RemoteFlightControl.MqttClient.MSP;

public interface IMSP_Response : IEnumerable<byte>
{
    #region Static
    public static IMSP_Response Create(MSP_Data msp_data)
    {
        switch(msp_data.Command)
        {
            case MSP_Command.STATUS:
            {
                return new MSP_Response_Status(msp_data);
            }
            case MSP_Command.RAW_IMU:
            {
                return new MSP_Response_RawImu(msp_data);
            }
            case MSP_Command.MOTOR:
            {
                return new MSP_Response_Motor(msp_data);
            }
            case MSP_Command.RAW_GPS:
            {
                return new MSP_Response_RawGPS(msp_data);
            }
            case MSP_Command.ATTITUDE:
            {
                return new MSP_Response_Attitude(msp_data);
            }
            case MSP_Command.ALTITUDE:
            {
                return new MSP_Response_Altitude(msp_data);
            }
            case MSP_Command.ANALOG:
            {
                return new MSP_Response_Analog(msp_data);
            }
            default:
            {
                return default;
            }
        }
    }
    #endregion
}