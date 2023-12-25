﻿
namespace LVK.Typed.Tests;

[TestFixture]
public class TypeHelperTests
{
    [Test]
    public void NameOf_NullType_ThrowsArgumentNullException()
    {
        var th = new TypeHelper(new List<ITypeNameRule>());

        Assert.Throws<ArgumentNullException>(() => th.NameOf(null!));
    }

    [Test]
    [TestCase(typeof(int), NameOfTypeOptions.Default, "int")]
    [TestCase(typeof(int?), NameOfTypeOptions.Default, "int?")]
    [TestCase(typeof(int), NameOfTypeOptions.None, "Int32")]
    [TestCase(typeof(int?), NameOfTypeOptions.None, "Nullable<Int32>")]
    [TestCase(typeof(int), NameOfTypeOptions.IncludeNamespaces, "System.Int32")]
    [TestCase(typeof(int?), NameOfTypeOptions.IncludeNamespaces, "System.Nullable<System.Int32>")]
    [TestCase(typeof(List<int?>), NameOfTypeOptions.Default & ~NameOfTypeOptions.IncludeNamespaces, "List<int?>")]
    [TestCase(typeof(List<int?>), NameOfTypeOptions.Default, "System.Collections.Generic.List<int?>")]
    [TestCase(typeof(Dictionary<List<int>, HashSet<object>>), NameOfTypeOptions.Default & ~NameOfTypeOptions.IncludeNamespaces, "Dictionary<List<int>, HashSet<object>>")]
    public void NameOf_SmokeTests(Type type, NameOfTypeOptions options, string expected)
    {
        ITypeHelper typeHelper = TypeHelperFactory.Create();

        string? output = typeHelper.TryGetNameOf(type, options);

        Assert.That(output, Is.EqualTo(expected));
    }
}