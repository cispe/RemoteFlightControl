using RemoteFlightControl.MqttClient;
using RemoteFlightControl.MqttClient.MSP;
using RemoteFlightControl.MqttClient.MSP.Responses;

MspMqttClient msp_mqtt_client = new();
while(true)
{
    IMspRequest request = IMspRequest.Create(MspCommand.ATTITUDE);
    MspResponse_Attitude? response = await msp_mqtt_client.SendAsync(request) as MspResponse_Attitude;
    Console.WriteLine($"Yaw: {response?.Yaw}");
}