using RemoteFlightControl.MqttClient.MSP.Requests;

namespace RemoteFlightControl.MqttClient.MSP;

/// <summary>
/// Represents a Multiwii Serial Protocol (MSP) request.
/// Implementations encapsulate the structure and data required to send a specific MSP command to a flight controller
/// </summary>
public interface IMspRequest
{
    #region Static
    /// <summary>
    /// Factory method to create an <see cref="IMspRequest"/> instance from a given <see cref="PayloadX"/>.
    /// The specific request type is determined by the <see cref="MspCommand"/> in the payload
    /// </summary>
    /// <param name="payload_x">The payload containing the MSP command and data</param>
    /// <returns>An <see cref="IMspRequest"/> implementation for the specified command</returns>
    /// <exception cref="NotImplementedException">Thrown if the command is not supported</exception>
    public static IMspRequest Create(PayloadX payload_x)
    {
        switch(payload_x.MspCommand)
        {
            case MspCommand.ATTITUDE:
                return new MspRequest_Attitude(payload_x);
            case MspCommand.ALTITUDE:
                return new MspRequest_Altitude(payload_x);
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Factory method to create an <see cref="IMspRequest"/> instance for a given <see cref="MspCommand"/>
    /// </summary>
    /// <param name="command">The MSP command to create a request for</param>
    /// <returns>An <see cref="IMspRequest"/> implementation for the specified command</returns>
    /// <exception cref="NotImplementedException">Thrown if the command is not supported</exception>
    public static IMspRequest Create(MspCommand command)
    {
        switch(command)
        {
            case MspCommand.ATTITUDE:
                return new MspRequest_Attitude();
            case MspCommand.ALTITUDE:
                return new MspRequest_Altitude();
            default:
                throw new NotImplementedException();
        }
    }
    #endregion

    #region Instance
    /// <summary>
    /// Gets the underlying MSP payload data for this request
    /// </summary>
    public abstract PayloadX Data { get; }
    #endregion
}