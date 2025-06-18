using System.Collections;

namespace RemoteFlightControl.BackEnd.Msp;

public readonly struct MspData : IEnumerable<byte>
{
    #region Instance
    private readonly byte[] Buffer = [];
    private readonly int StartIndex = 0;

    internal MspData(int payload_size, MspCommand command)
    {
        Buffer = new byte[payload_size + 3];
        PayloadSize = (byte)payload_size;
        Command = command;
    }
    internal MspData(byte[] buffer, int start_index)
    {
        Buffer = buffer;
        StartIndex = start_index;
    }

    public byte PayloadSize
    {
        get => Buffer[StartIndex];
        set
        {
            Buffer[StartIndex] = value;
        }
    }
    public MspCommand Command
    {
        get => (MspCommand)Buffer[StartIndex + 1];
        set
        {
            Buffer[StartIndex + 1] = (byte)value;
        }
    }
    public Span<byte> Payload
    {
        get => Buffer.AsSpan(StartIndex + 2, PayloadSize);
    }
    public byte Checksum
    {
        get => Buffer[StartIndex + 2 + PayloadSize];
        private set
        {
            Buffer[StartIndex + 2 + PayloadSize] = value;
        }
    }

    public void InitChecksum()
    {
        int checksum = PayloadSize ^ (int)Command;
        foreach(byte b in Payload)
        {
            checksum ^= b;
        }
        Checksum = (byte)checksum;
    }
    #endregion

    #region IEnumerable<byte>
    public IEnumerator<byte> GetEnumerator()
    {
        InitChecksum();
        return Buffer.Skip(StartIndex).Take(PayloadSize + 3).GetEnumerator();
    }
    #endregion

    #region IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
}