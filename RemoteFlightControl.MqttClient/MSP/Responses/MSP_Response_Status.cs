using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RemoteFlightControl.MqttClient.MSP.Responses;

[Flags] public enum SensorFlags : ushort
{
    None = 0,
    Accelerometer = 1 << 0,
    Barometer = 1 << 1,
    Magnetometer = 1 << 2,
    GPS = 1 << 3,
    Rangefinder = 1 << 4,
    Gyroscope = 1 << 5,
    OpticalFlow = 1 << 6
}
[Flags] public enum FlightModeFlags : ulong
{
    None = 0,

    Arm = 1UL << 0,
    Prearm = 1UL << 1,
    AirMode = 1UL << 2,
    Angle = 1UL << 3,
    Horizon = 1UL << 4,
    AltHold = 1UL << 5,
    PosHold = 1UL << 6,
    HeadFree = 1UL << 7,
    HeadAdjust = 1UL << 8,
    FPVAngleMix = 1UL << 9,
    Calib = 1UL << 10,
    AcroTrainer = 1UL << 11,
    Mag = 1UL << 12,
    GPSRescue = 1UL << 13,
    BeepGpsCount = 1UL << 14,
    Failsafe = 1UL << 15,
    PassThru = 1UL << 16,
    BeeperOn = 1UL << 17,
    BeeperMute = 1UL << 18,
    LedLow = 1UL << 19,
    Blackbox = 1UL << 20,
    BlackboxErase = 1UL << 21,
    Mode3D = 1UL << 22,
    CrashFlip = 1UL << 23,
    CamStab = 1UL << 24,
    OSD = 1UL << 25,
    Telemetry = 1UL << 26,
    Servo1 = 1UL << 27,
    Servo2 = 1UL << 28,
    Servo3 = 1UL << 29,
    Camera1 = 1UL << 30,
    Camera2 = 1UL << 31,
    Camera3 = 1UL << 32,
    VTXPitMode = 1UL << 33,
    VTXControlDisable = 1UL << 34,
    Paralyze = 1UL << 35,
    User1 = 1UL << 36,
    User2 = 1UL << 37,
    User3 = 1UL << 38,
    User4 = 1UL << 39,
    PidAudio = 1UL << 40,
    LaunchControl = 1UL << 41,
    MSPOverride = 1UL << 42,
    StickCommandDisable = 1UL << 43,
    Ready = 1UL << 44,
    LapTimerReset = 1UL << 45,
    Chirp = 1UL << 46
}
public class MSP_Response_Status : IMSP_Response
{
    #region Instance
    private readonly MSP_Data MSP_Data;

    internal MSP_Response_Status(MSP_Data msp_data)
    {
        MSP_Data = msp_data;
    }

    public ushort CycleTime
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.ReadUnaligned<ushort>(ref start);
        }
    }
    public ushort I2CErrorCount
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 2);
            return Unsafe.ReadUnaligned<ushort>(ref ptr);
        }
    }
    public SensorFlags Sensors
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 4);
            return (SensorFlags)Unsafe.ReadUnaligned<ushort>(ref ptr);
        }
    }
    public FlightModeFlags FlightModeFlags
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 6);
            return (FlightModeFlags)Unsafe.ReadUnaligned<uint>(ref ptr);
        }
    }
    public byte CurrentPidProfileIndex
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.Add(ref start, 10);
        }
    }
    public ushort SystemLoad
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 11);
            return Unsafe.ReadUnaligned<ushort>(ref ptr);
        }
    }
    public ushort GyroCycleTime
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 13);
            return Unsafe.ReadUnaligned<ushort>(ref ptr);
        }
    }
    public byte FlightModeFlagsExtByteCount
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            return Unsafe.Add(ref start, 15);
        }
    }
    public ReadOnlySpan<byte> FlightModeFlagsExtBytes
    {
        get
        {
            byte count = FlightModeFlagsExtByteCount;
            return MSP_Data.Payload.Slice(16, count);
        }
    }
    public byte ArmingDisableFlagsCount
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 16 + FlightModeFlagsExtByteCount);
            return ptr;
        }
    }
    public uint ArmingDisableFlags
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 17 + FlightModeFlagsExtByteCount);
            return Unsafe.ReadUnaligned<uint>(ref ptr);
        }
    }
    public byte ConfigStateFlags
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 21 + FlightModeFlagsExtByteCount);
            return ptr;
        }
    }
    public ushort CPU_Temperature
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 22 + FlightModeFlagsExtByteCount);
            return Unsafe.ReadUnaligned<ushort>(ref ptr);
        }
    }
    public byte ControlRateProfileCount
    {
        get
        {
            ref byte start = ref MemoryMarshal.GetReference(MSP_Data.Payload);
            ref byte ptr = ref Unsafe.Add(ref start, 24 + FlightModeFlagsExtByteCount);
            return ptr;
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