using RemoteFlightControl.BackEnd.Msp.Requests;

namespace RemoteFlightControl.BackEnd.Msp;

public interface IMspRequest : IEnumerable<byte>
{
    public static IMspRequest Create(MspCommand command)
    {
        return command switch
        {
            MspCommand.GetApiVersion => new MspRequest_GetApiVersion(),
            MspCommand.GetRawImu => new MspRequest_GetRawImu(),
            MspCommand.GetAttitude => new MspRequest_GetAttitude(),
            MspCommand.GetAltitude => new MspRequest_GetAltitude(),
            MspCommand.GetRawGps => new MspRequest_GetRawGps(),
            MspCommand.GetAnalog => new MspRequest_GetAnalog(),
            _ => throw new NotImplementedException()
        };
    }
}