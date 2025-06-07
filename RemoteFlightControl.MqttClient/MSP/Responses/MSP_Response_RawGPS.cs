using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

public enum GPSFixType : byte
{
    None = 0,
    Fix2D = 1,
    Fix3D = 2,
    DGPS = 3,
    RTK = 4
}
public class MSP_Response_RawGPS : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_RawGPS(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public GPSFixType Fix
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return (GPSFixType)start;
        }
    }
    public byte Satellites
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.Add(ref start, 1);
        }
    }
    public int Latitude
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte lat = ref Unsafe.Add(ref start, 2);
            return Unsafe.ReadUnaligned<int>(ref lat);
        }
    }
    public int Longitude
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte lon = ref Unsafe.Add(ref start, 6);
            return Unsafe.ReadUnaligned<int>(ref lon);
        }
    }
    public ushort Altitude
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte alt = ref Unsafe.Add(ref start, 10);
            return Unsafe.ReadUnaligned<ushort>(ref alt);
        }
    }
    public ushort GroundSpeed
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte spd = ref Unsafe.Add(ref start, 12);
            return Unsafe.ReadUnaligned<ushort>(ref spd);
        }
    }
    public ushort GroundCourse
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte crs = ref Unsafe.Add(ref start, 14);
            return Unsafe.ReadUnaligned<ushort>(ref crs);
        }
    }
    public ushort PDOP
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte pdop = ref Unsafe.Add(ref start, 16);
            return Unsafe.ReadUnaligned<ushort>(ref pdop);
        }
    }
    #endregion

    #region IEnumerable<byte>
    public IEnumerator<byte> GetEnumerator()
    {
        return MSP_Data.GetEnumerator();
    }
    #endregion

    #region IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
}
