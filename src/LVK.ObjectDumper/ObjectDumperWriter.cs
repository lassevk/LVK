using LVK.Typed;

namespace LVK.ObjectDumper;

internal class ObjectDumperWriter : IObjectDumperWriter
{
    private readonly TextWriter _target;
    private readonly ObjectDumperOptions _options;
    private readonly Dictionary<object, int> _objectIds = new();
    private readonly Stack<string> _blocks = new();

    private int _indentationLevel;
    private string _indentationTemp;
    private bool _isFirstLine = true;

    public ObjectDumperWriter(TextWriter target, ObjectDumperOptions options)
    {
        _target = target;
        _options = options;
        _indentationTemp = new string(' ', options.IndentationSize * 8);
    }

    public void WriteLine(string line)
    {
        if (!_isFirstLine)
            _target.WriteLine();

        if (_indentationLevel * 4 > _indentationTemp.Length)
            _indentationTemp = new string(' ', _indentationLevel * 4);

        _target.Write(_indentationTemp[.. (_indentationLevel * 4)]);
        _target.Write(line);
        _isFirstLine = false;
    }

    public void Indent()
    {
        if (_indentationLevel == _options.MaxIndentationLevel)
            throw new InvalidOperationException("Unbounded recursion");

        _indentationLevel++;
    }

    public void Outdent()
    {
        if (_indentationLevel == 0)
            throw new InvalidOperationException("Internal error, imbalanced indentation level");

        _indentationLevel--;
    }

    public string FormatType(Type type) => TypeHelper.Instance.NameOf(type, _options.NameOfTypeOptions);

    public void WriteFormatted(string name, object value, string formatted, out bool isFirstTime)
    {
        var keyPrefix = "";
        var keySuffix = "";
        isFirstTime = true;

        if (!value.GetType().IsValueType)
        {
            if (_objectIds.TryGetValue(value, out int keyId))
            {
                keySuffix = $" (see <{keyId}>)";
                isFirstTime = false;
            }
            else
            {
                keyId = _objectIds.Count;
                _objectIds[value] = keyId;
                keyPrefix = $"<{keyId}> ";
                isFirstTime = true;
            }
        }

        string type = FormatType(value.GetType());

        if (formatted != "")
            WriteLine($"{keyPrefix}{name} = {formatted} [{type}]{keySuffix}");
        else
            WriteLine($"{keyPrefix}{name} = {type}{keySuffix}");
    }

    public void BeginBlock(string name = "")
    {
        if (name != "")
            WriteLine(name);

        WriteLine("{");
        Indent();
        _blocks.Push(name);
    }

    public void EndBlock()
    {
        Outdent();
        string name = _blocks.Pop();
        if (name != "")
            WriteLine($"}} // {name}");
        else
            WriteLine("}");
    }
}