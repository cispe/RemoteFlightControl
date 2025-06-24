using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Responses;

public class MspResponse_GetRawImu(MspData MspData) : IMspResponse
{
    #region Instance
    public short AccelerationX
    {
        get => BitConverter.ToInt16(MspData.Payload[0..2]);
    }
    public short AccelerationY
    {
        get => BitConverter.ToInt16(MspData.Payload[2..4]);
    }
    public short AccelerationZ
    {
        get => BitConverter.ToInt16(MspData.Payload[4..6]);
    }
    public short GyroscopeX
    {
        get => BitConverter.ToInt16(MspData.Payload[6..8]);
    }
    public short GyroscopeY
    {
        get => BitConverter.ToInt16(MspData.Payload[8..10]);
    }
    public short GyroscopeZ
    {
        get => BitConverter.ToInt16(MspData.Payload[10..12]);
    }
    public short MagnitometerX
    {
        get => BitConverter.ToInt16(MspData.Payload[12..14]);
    }
    public short MagnitometerY
    {
        get => BitConverter.ToInt16(MspData.Payload[14..16]);
    }
    public short MagnitometerZ
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