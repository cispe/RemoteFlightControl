using RemoteFlightControl.BackEnd.Msp.Responses;

namespace RemoteFlightControl.BackEnd.Msp;

public interface IMspResponse : IEnumerable<byte>
{
    public static IMspResponse Create(MspData msp_data)
    {
        return msp_data.Command switch
        {
            MspCommand.GetApiVersion => new MspResponse_GetApiVersion(msp_data),
            _ => throw new NotImplementedException()
        };
    }
}