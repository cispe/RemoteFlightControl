namespace RemoteFlightControl.BackEnd.Msp;

public interface IMspRequest : IEnumerable<byte>
{
    public static IMspRequest Create(MspCommand command)
    {
        throw new NotImplementedException("IMspRequest not implemented");
    }
}