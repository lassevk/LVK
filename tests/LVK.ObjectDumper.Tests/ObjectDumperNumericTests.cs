using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using NUnit.Framework.Internal;

namespace LVK.ObjectDumper.Tests;

public class ObjectDumperNumericTests
{
    [Test]
    [TestCase("Value", (long)10, "Value = 10 [System.Int64]")]
    [TestCase("Value", (ulong)11, "Value = 11 [System.UInt64]")]
    [TestCase("Value", 12, "Value = 12 [System.Int32]")]
    [TestCase("Value", (uint)13, "Value = 13 [System.UInt32]")]
    [TestCase("Value", (short)14, "Value = 14 [System.Int16]")]
    [TestCase("Value", (ushort)15, "Value = 15 [System.UInt16]")]
    [TestCase("Value", (sbyte)16, "Value = 16 [System.SByte]")]
    [TestCase("Value", (byte)17, "Value = 17 [System.Byte]")]
    [TestCase("Value", (float)19.456, "Value = 19.456 [System.Single]")]
    [TestCase("Value", (double)20.789, "Value = 20.789 [System.Double]")]
    public void Dump_WithTestCases(string name, object value, string expected)
    {
        string output = ObjectDumper.Instance.Dump(name, value);
        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void Dump_DecimalType()
    {
        string output = ObjectDumper.Instance.Dump("Value", 19.456M);
        Assert.That(output, Is.EqualTo("Value = 19.456 [System.Decimal]"));
    }
}