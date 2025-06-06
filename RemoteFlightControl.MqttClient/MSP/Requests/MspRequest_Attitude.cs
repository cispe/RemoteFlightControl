namespace RemoteFlightControl.MqttClient.MSP.Requests;

/// <summary>
/// Represents an MSP request for the <see cref="MspCommand.ATTITUDE"/> command.
/// This request is used to query the current attitude (roll, pitch, yaw) from the flight controller
/// </summary>
public class MspRequest_Attitude : IMspRequest
{
    #region Static
    /// <summary>
    /// The payload length for the ATTITUDE request (always zero, as no payload is required)
    /// </summary>
    public const int PayloadLength = 0;
    #endregion

    #region Instance
    /// <summary>
    /// Gets the underlying MSP payload data for this request
    /// </summary>
    public PayloadX Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MspRequest_Attitude"/> class with a default payload
    /// </summary>
    public MspRequest_Attitude()
    {
        Data = new PayloadX(PayloadLength);
        Data.MspCommand = MspCommand.ATTITUDE;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MspRequest_Attitude"/> class from an existing payload
    /// </summary>
    /// <param name="payload_x">The payload containing the MSP command and data</param>
    public MspRequest_Attitude(PayloadX payload_x)
    {
        Data = payload_x;
    }
    #endregion
}