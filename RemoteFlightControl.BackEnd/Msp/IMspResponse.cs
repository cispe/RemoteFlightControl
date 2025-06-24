using RemoteFlightControl.BackEnd.Msp.Responses;

namespace RemoteFlightControl.BackEnd.Msp;

public interface IMspResponse : IEnumerable<byte>
{
    public static IMspResponse Create(MspData msp_data)
    {
        return msp_data.Command switch
        {
            MspCommand.GetApiVersion => new MspResponse_GetApiVersion(msp_data),
            MspCommand.GetRawImu => new MspResponse_GetRawImu(msp_data),
            MspCommand.GetAttitude => new MspResponse_GetAttitude(msp_data),
            MspCommand.GetAltitude => new MspResponse_GetAltitude(msp_data),
            MspCommand.GetRawGps => new MspResponse_GetRawGps(msp_data),
            MspCommand.GetAnalog => new MspResponse_GetAnalog(msp_data),
            _ => throw new NotImplementedException()
        };
    }
}