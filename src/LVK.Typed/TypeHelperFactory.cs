namespace LVK.Typed;

public static class TypeHelperFactory
{
    internal static IEnumerable<ITypeNameRule> GetRules()
    {
        yield return new NormalTypeNameRule();
        yield return new CSharpKeywordTypeNameRule();
        yield return new NullableTypeNameRule();
        yield return new GenericTypeNameRule();
    }

    public static ITypeHelper Create() => new TypeHelper(GetRules());
}