namespace LVK.Data.Encoding;

public static class DataEncoder
{
    // TODO: Add Span targets

    public static void WriteUInt32(Stream target, uint value)
    {
        while (value != 0)
        {
            var next = (byte)(value & 0x7f);
            value = value >> 7;

            if (value != 0)
                next |= 0x80;

            target.WriteByte(next);
        }
    }

    public static void WriteInt32(Stream target, int value) => WriteUInt32(target, unchecked((uint)value));

    public static uint ReadUInt32(Stream source)
    {
        uint value = 0;
        var shift = 0;
        while (true)
        {
            int next = source.ReadByte();
            if (next < 0)
                return value;

            value = value | (uint)((next & 0x7f) << shift);
            if ((next & 0x80) == 0)
                return value;

            shift += 7;
        }
    }

    public static int ReadInt32(Stream source) => unchecked((int)ReadUInt32(source));
}