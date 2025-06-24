using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Responses;

public class MspResponse_GetRawGps(MspData MspData) : IMspResponse
{
    #region Instance
    public byte GpsFix
    {
        get => MspData.Payload[0];
    }
    public byte SatelliteCount
    {
        get => MspData.Payload[1];
    }
    public int Latitude
    {
        get => BitConverter.ToInt32(MspData.Payload[2..6]);
    }
    public int Longtitude
    {
        get => BitConverter.ToInt32(MspData.Payload[6..10]);
    }
    public short Altitude
    {
        get => BitConverter.ToInt16(MspData.Payload[10..12]);
    }
    public short GroundSpeed
    {
        get => BitConverter.ToInt16(MspData.Payload[12..14]);
    }
    public short GroundDirection
    {
        get => BitConverter.ToInt16(MspData.Payload[14..16]);
    }
    public short PositionDilutionOfPrecision
    {
        get => BitConverter.ToInt16(MspData.Payload[16..18]);
    }
    #endregion

    #region IMspResponse
    public IEnumerator<byte> GetEnumerator()
    {
        return MspData.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
}