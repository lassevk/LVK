using System.Globalization;

using LVK.Typed;

namespace LVK.ObjectDumper;

public class ObjectDumperOptions
{
    private int _indentationSize = 4;
    private int _maxIndentationLevel = 256;
    private int _maxRecursionLevel = 32;
    private CultureInfo _formattingCulture = CultureInfo.InvariantCulture;
    private int _maxStringDumpLength = 32_768;
    private int _maxByteArrayDumpLength = 32_768;
    private int _maxCollectionDumpLength = 10_000;

    public int IndentationSize
    {
        get => _indentationSize;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 10);

            _indentationSize = value;
        }
    }

    public int MaxIndentationLevel
    {
        get => _maxIndentationLevel;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 5);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 200);

            _maxIndentationLevel = value;
        }
    }

    public int MaxRecursionLevel
    {
        get => _maxRecursionLevel;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);

            _maxRecursionLevel = value;
        }
    }

    public CultureInfo FormattingCulture
    {
        get => _formattingCulture;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            _formattingCulture = value;
        }
    }

    public bool StringsOnOneLine { get; set; } = false;

    public int MaxStringDumpLength
    {
        get => _maxStringDumpLength;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 10);

            _maxStringDumpLength = value;
        }
    }

    public int MaxByteArrayDumpLength
    {
        get => _maxByteArrayDumpLength;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 16);

            _maxByteArrayDumpLength = value;
        }
    }

    public int MaxCollectionDumpLength
    {
        get => _maxCollectionDumpLength;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

            _maxCollectionDumpLength = value;
        }
    }

    public NameOfTypeOptions NameOfTypeOptions { get; set; } = NameOfTypeOptions.IncludeNamespaces | NameOfTypeOptions.SpaceAfterCommas | NameOfTypeOptions.UseShorthandSyntax;

    public bool IncludeFields { get; set; } = true;
    public bool IncludePrivateMembers { get; set; } = false;
}