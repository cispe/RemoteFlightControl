using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace RemoteFlightControl.MqttClient.MSP;

/// <summary>
/// Represents a Multiwii Serial Protocol (MSP) message buffer, providing access to message fields and payload.
/// Encapsulates the raw byte array structure used for MSP communication, including message ID, command, payload, and checksum
/// </summary>
public class PayloadX
{
    #region Static
    /// <summary>
    /// The number of extra bytes in the message buffer (2 for MessageID, 1 for Command, 1 for Checksum)
    /// </summary>
    public const int ExtraLength = 4;
    #endregion

    #region Instance
    /// <summary>
    /// The underlying byte array containing the full MSP message.
    /// Layout: [0-1] MessageID (ushort, little-endian), [2] Command, [3..N-2] Payload, [N-1] Checksum
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PayloadX"/> with an existing data buffer
    /// </summary>
    /// <param name="data">A byte array containing the MSP message. Must be at least <see cref="ExtraLength"/> bytes long</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="data"/> is too short</exception>
    public PayloadX(byte[] data)
    {
        if (data.Length < ExtraLength)
        {
            throw new ArgumentException($"Array length must be >= {ExtraLength}");
        }
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PayloadX"/> with a specified payload length.
    /// The total buffer size will be <paramref name="payload_length"/> plus <see cref="ExtraLength"/>
    /// </summary>
    /// <param name="payload_length">The length of the payload in bytes</param>
    public PayloadX(nuint payload_length)
    {
        Data = new byte[payload_length + ExtraLength];
    }

    /// <summary>
    /// Gets or sets the message ID (first two bytes, little-endian) of the MSP message
    /// </summary>
    public ushort MessageID
    {
        get => Unsafe.As<byte, ushort>(ref MemoryMarshal.GetArrayDataReference(Data));
        set => Unsafe.As<byte, ushort>(ref MemoryMarshal.GetArrayDataReference(Data)) = value;
    }

    /// <summary>
    /// Gets or sets the MSP command code (third byte) of the message
    /// </summary>
    public MspCommand MspCommand
    {
        get => (MspCommand)Data[2];
        set => Data[2] = (byte)value;
    }

    /// <summary>
    /// Gets a span representing the payload section of the message (excluding header and checksum)
    /// </summary>
    public Span<byte> Payload => Data.AsSpan(3, Data.Length - ExtraLength);

    /// <summary>
    /// Gets the checksum byte (last byte) of the message
    /// </summary>
    public byte Checksum => Data[^1];

    /// <summary>
    /// Calculates and sets the checksum for the current message buffer.
    /// The checksum is computed as XOR of the command, payload length, and all payload bytes
    /// </summary>
    public void InitChecksum()
    {
        int checksum = (int)MspCommand ^ (Data.Length - ExtraLength);
        foreach (byte b in Payload)
        {
            checksum ^= b;
        }
        Data[^1] = (byte)checksum;
    }
    #endregion
}