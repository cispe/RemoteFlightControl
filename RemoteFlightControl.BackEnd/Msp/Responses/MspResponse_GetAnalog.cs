using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp.Responses;

public class MspResponse_GetAnalog(MspData MspData) : IMspResponse
{
    #region Instance
    public byte LegacyBatteryVoltage
    {
        get => MspData.Payload[0];
    }
    public short MahDrawn
    {
        get => BitConverter.ToInt16(MspData.Payload[1..3]);
    }
    public short Rssi
    {
        get => BitConverter.ToInt16(MspData.Payload[3..5]);
    }
    public short Amperage
    {
        get => BitConverter.ToInt16(MspData.Payload[5..7]);
    }
    public short BatteryVoltage
    {
        get => BitConverter.ToInt16(MspData.Payload[7..9]);
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