namespace LVK.Typed.Rules;

internal class CSharpKeywordTypeNameRule : ITypeNameRule
{
    private readonly Dictionary<Type, string> _cSharpKeywordTypeNames = new Dictionary<Type, string>
    {
        [typeof(sbyte)]  = "sbyte",
        [typeof(byte)]   = "byte",
        [typeof(short)]  = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)]    = "int",
        [typeof(uint)]   = "uint",
        [typeof(long)]   = "long",
        [typeof(ulong)]  = "ulong",
        [typeof(char)]   = "char",
        [typeof(string)] = "string",
        [typeof(double)] = "double",
        [typeof(float)]  = "float",
        [typeof(bool)]   = "bool",
        [typeof(object)] = "object",
        [typeof(nint)]   = "nint",
        [typeof(nuint)]  = "nuint", };

    public int Priority => 1;

    public string? TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeHelper);

        if ((options & NameOfTypeOptions.UseCSharpKeywords) == 0)
        {
            return null;
        }

        _cSharpKeywordTypeNames.TryGetValue(type, out string? name);
        return name;
    }
}