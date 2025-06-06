namespace RemoteFlightControl.MqttClient.MSP.Responses;

/// <summary>
/// Represents an MSP response for the <see cref="MspCommand.ATTITUDE"/> command.
/// This response provides the current attitude (roll, pitch, yaw) as reported by the flight controller
/// </summary>
public class MspResponse_Attitude(PayloadX payload_x) : IMspResponse
{
    /// <summary>
    /// Gets the underlying MSP payload data for this response
    /// </summary>
    public PayloadX Data => payload_x;

    /// <summary>
    /// Roll angle in tenths of a degree
    /// </summary>
    public short Roll
    {
        get
        {
            var payload = payload_x.Payload;
            return (short)(payload[0] | (payload[1] << 8));
        }
    }

    /// <summary>
    /// Pitch angle in tenths of a degree
    /// </summary>
    public short Pitch
    {
        get
        {
            var payload = payload_x.Payload;
            return (short)(payload[2] | (payload[3] << 8));
        }
    }

    /// <summary>
    /// Yaw angle in tenths of a degree
    /// </summary>
    public short Yaw
    {
        get
        {
            var payload = payload_x.Payload;
            return (short)(payload[4] | (payload[5] << 8));
        }
    }
}