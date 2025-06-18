namespace RemoteFlightControl.BackEnd.Msp;

public interface IMspResponse : IEnumerable<byte>
{
    public static IMspResponse Create(MspData msp_data)
    {
        throw new NotImplementedException("IMspResponse not implemented");
    }
}