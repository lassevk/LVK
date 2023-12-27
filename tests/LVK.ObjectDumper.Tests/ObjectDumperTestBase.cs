using LVK.ObjectDumper.Rules;
using LVK.Typed.Rules;

namespace LVK.ObjectDumper.Tests;

public class ObjectDumperTestBase
{
    protected readonly IObjectDumper Dumper = new ObjectDumper(new Typed.TypeHelper([
        new GenericTypeNameRule(),
        new NormalTypeNameRule(),
        new NullableTypeNameRule(),
        new CSharpKeywordTypeNameRule()
    ]), [
        new AssemblyObjectDumperRule(),
        new BooleanObjectDumperRule(),
        new EnumerableObjectDumperRule(),
        new EnumObjectDumperRule(),
        new ExceptionObjectDumperRule(),
        new GuidObjectDumperRule(),
        new IntegerObjectDumperRule(),
        new NumericObjectDumperRule(),
        new PointerObjectDumperRule(),
        new StringObjectDumperRule(),
        new TypeObjectDumperRule(),
        new ByteArrayObjectDumperRule(),
        new IntPtrObjectDumperRule(),
        new NonRecursiveTypesObjectDumperRule(),
        new DateOrTimeTypesObjectDumperRule()
    ]);
}