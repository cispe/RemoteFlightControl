using RemoteFlightControl.BackEnd.Msp.Requests;

namespace RemoteFlightControl.BackEnd.Msp;

public interface IMspRequest : IEnumerable<byte>
{
    public static IMspRequest Create(MspCommand command)
    {
        return command switch
        {
            MspCommand.GetApiVersion => new MspRequest_GetApiVersion(),
            _ => throw new NotImplementedException()
        };
    }
}