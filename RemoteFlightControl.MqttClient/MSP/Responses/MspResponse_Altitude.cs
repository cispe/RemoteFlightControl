namespace RemoteFlightControl.MqttClient.MSP.Responses;

public class MspResponse_Altitude(PayloadX payload_x) : IMspResponse
{
    public PayloadX Data => payload_x;

    public int EstimatedAltitude
    {
        get
        {
            var payload = payload_x.Payload;
            return payload[0] | (payload[1] << 8) | (payload[2] << 16) | (payload[3] << 24);
        }
    }
    public short EstimatedVerticalVelocity
    {
        get
        {
            var payload = payload_x.Payload;
            return (short)(payload[4] | (payload[5] << 8));
        }
    }
}