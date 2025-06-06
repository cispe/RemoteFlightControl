using RemoteFlightControl.MqttClient.MSP.Responses;

namespace RemoteFlightControl.MqttClient.MSP;

/// <summary>
/// Represents a Multiwii Serial Protocol (MSP) response.
/// Implementations encapsulate the structure and data returned by a flight controller in response to a specific MSP command
/// </summary>
public interface IMspResponse
{
    #region Static
    /// <summary>
    /// Factory method to create an <see cref="IMspResponse"/> instance from a given <see cref="PayloadX"/>.
    /// The specific response type is determined by the <see cref="MspCommand"/> in the payload
    /// </summary>
    /// <param name="payload_x">The payload containing the MSP command and response data</param>
    /// <returns>An <see cref="IMspResponse"/> implementation for the specified command</returns>
    /// <exception cref="NotImplementedException">Thrown if the command is not supported</exception>
    public static IMspResponse Create(PayloadX payload_x)
    {
        switch(payload_x.MspCommand)
        {
            case MspCommand.ATTITUDE:
            {
                return new MspResponse_Attitude(payload_x);
            }
            case MspCommand.ALTITUDE:
            {
                return new MspResponse_Altitude(payload_x);
            }
            default:
            {
                throw new NotImplementedException();
            }
        }
    }
    #endregion

    #region Instance
    /// <summary>
    /// Gets the underlying MSP payload data for this response
    /// </summary>
    public abstract PayloadX Data { get; }
    #endregion
}