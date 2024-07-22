using System.Text;

namespace LVK.ObjectDumper.Rules;

public class ByteArrayObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [typeof(byte[])];

    public bool SupportsType(Type type) => false;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var array = (byte[])value;
        Span<byte> span = array;
        if (span.Length > context.Options.MaxByteArrayDumpLength)
            span = span[..context.Options.MaxByteArrayDumpLength];

        if (array.Length <= 16)
            DumpShortByteArray(context, name, value, span, array.Length);
        else
            DumpLongByteArray(context, name, value, span, array.Length);
    }

    private void DumpShortByteArray(IObjectDumperContext context, string name, object value, Span<byte> span, int originalLength)
    {
        var sb = new StringBuilder();
        sb.Append($"Length: {originalLength}");
        sb.Append(", Contents: ");
        FormatBytes(span, 0, sb, false);
        sb.Append(" = ");
        FormatCharacters(span, 0, sb, false);
        if (span.Length < originalLength)
            sb.Append(" ...");

        context.Writer.WriteFormatted(name, value, sb.ToString(), out _);
    }

    private static void DumpLongByteArray(IObjectDumperContext context, string name, object value, Span<byte> span, int originalLength)
    {
        context.Writer.WriteFormatted(name, value, $"Length: {originalLength}", out bool isFirstTime);
        if (!isFirstTime)
            return;

        context.Writer.BeginBlock();
        int addressLength;
        if (span.Length > 0xffffff)
            addressLength = 8;
        else if (span.Length > 0xffff)
            addressLength = 6;
        else if (span.Length > 0xff)
            addressLength = 4;
        else
            addressLength = 2;

        var separator1 = "  :  ";
        string separator2 = separator1[1..];

        var sb = new StringBuilder();
        sb.Append("ADDRESS "[..addressLength].Trim().PadLeft(addressLength, ' '));
        sb.Append(separator1);
        for (var index = 0; index < 16; index++)
        {
            sb.Append($"{index:X2} ");
        }

        sb.Append(separator2);
        sb.Append("CHARACTERS");

        context.Writer.WriteLine(sb.ToString());
        context.Writer.WriteLine(new string('-', 77));

        for (var index = 0; index < span.Length; index += 16)
        {
            sb.Clear();
            sb.Append(index.ToString("X").PadLeft(addressLength, '0'));
            sb.Append(separator1);

            // Byte values
            FormatBytes(span, index, sb, true);

            // Characters
            sb.Append(separator2);
            FormatCharacters(span, index, sb, true);

            context.Writer.WriteLine(sb.ToString());
        }

        if (span.Length < originalLength)
            context.Writer.WriteLine($"+ {originalLength - span.Length} byte{(originalLength - span.Length == 1 ? "" : "s")}");

        context.Writer.EndBlock();
    }

    private static void FormatCharacters(Span<byte> span, int index, StringBuilder target, bool pad)
    {
        for (var delta = 0; delta < 16; delta++)
        {
            int address = index + delta;
            if (address < span.Length)
            {
                var c = (char)span[address];
                if (c < 32 || c > 127)
                    c = '.';

                target.Append(c);
            }
            else if (pad)
            {
                target.Append(' ');
            }
        }
    }

    private static void FormatBytes(Span<byte> span, int index, StringBuilder target, bool pad)
    {
        for (var delta = 0; delta < 16; delta++)
        {
            int address = index + delta;
            if (address < span.Length)
                target.Append(span[address].ToString("X2")).Append(' ');
            else if (pad)
                target.Append("   ");
        }
    }
}