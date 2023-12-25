using LVK.ObjectDumper.Rules;
using LVK.Typed;

namespace LVK.ObjectDumper;

public static class ObjectDumperFactory
{
    private static readonly Lazy<IObjectDumper> _factory = new(CreateInstance, LazyThreadSafetyMode.None);

    private static IObjectDumper CreateInstance() => new ObjectDumper(TypeHelperFactory.Create(), GetRules());

    public static IObjectDumper Create() => _factory.Value;

    public static IEnumerable<IObjectDumperRule> GetRules()
    {
        yield return new StringObjectDumperRule();
        yield return new IntegerObjectDumperRule();
        yield return new BooleanObjectDumperRule();
        yield return new NumericObjectDumperRule();
        yield return new IntPtrObjectDumperRule();
        yield return new PointerObjectDumperRule();
        yield return new NonRecursiveTypesObjectDumperRule();
        yield return new TypeObjectDumperRule();
        yield return new AssemblyObjectDumperRule();
        yield return new GuidObjectDumperRule();
        yield return new EnumerableObjectDumperRule();
        yield return new DateOrTimeTypesObjectDumperRule();
        yield return new EnumerableObjectDumperRule();
        yield return new EnumObjectDumperRule();
        yield return new ByteArrayObjectDumperRule();
        yield return new ExceptionObjectDumperRule();
    }
}