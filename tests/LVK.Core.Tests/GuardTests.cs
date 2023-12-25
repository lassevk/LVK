namespace LVK.Core.Tests;

public class GuardTests
{
    [Test]
    public void NotNull_WithNullReference_ThrowsArgumentNullExceptionWithRightMessage()
    {
        try
        {
            Guard.NotNull(null!);
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Does.StartWith("null cannot be null"));
            return;
        }

        Assert.Fail("Did not throw");
    }

    [Test]
    public void NotNull_WithNonNullReference_DoesNotThrowAnException()
    {
        Guard.NotNull("");
    }

    [Test]
    [TestCase("Test", false)]
    [TestCase("", true)]
    [TestCase(" ", false)]
    [TestCase(null, true)]
    public void NotNullOrEmpty_WithTestCases(string? input, bool expected)
    {
        if (expected)
            Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty(input!));
        else
            Guard.NotNullOrEmpty(input!);
    }

    [Test]
    [TestCase("Test", false)]
    [TestCase("", true)]
    [TestCase(" ", true)]
    [TestCase(null, true)]
    public void NotNullOrWhiteSpace_WithTestCases(string? input, bool expected)
    {
        if (expected)
            Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrWhiteSpace(input!));
        else
            Guard.NotNullOrWhiteSpace(input!);
    }

    [Test]
    [TestCase(0, 0, 10, false)]
    [TestCase(5, 0, 10, false)]
    [TestCase(10, 0, 10, false)]
    [TestCase(-1, 0, 10, true)]
    [TestCase(11, 0, 10, true)]
    public void InRange_WithTestCases(int value, int lowestValue, int highestValue, bool expected)
    {
        if (expected)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.InRange(value, lowestValue, highestValue));
            // Assert.Throws<ArgumentNullException>(() => Guard.GreaterThan(value, lowestValue - 1));
            // Assert.Throws<ArgumentNullException>(() => Guard.GreaterThanOrEqual(value, lowestValue));
            // Assert.Throws<ArgumentNullException>(() => Guard.LessThan(value, highestValue + 1));
            // Assert.Throws<ArgumentNullException>(() => Guard.LessThanOrEqual(value, highestValue));
        }
        else
        {
            Guard.InRange(value, lowestValue, highestValue);
            // Guard.GreaterThan(value, lowestValue - 1);
            // Guard.GreaterThanOrEqual(value, lowestValue);
            // Guard.LessThan(value, highestValue + 1);
            // Guard.LessThanOrEqual(value, highestValue);
        }
    }

    [Test]
    [TestCase(0, 10, false)]
    [TestCase(5, 10, false)]
    [TestCase(9, 10, false)]
    [TestCase(10, 10, true)]
    [TestCase(11, 10, true)]
    public void LessThan_WithTestCases(int value, int lowerValue, bool expected)
    {
        if (expected)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThan(value, lowerValue));
        }
        else
        {
            Guard.LessThan(value, lowerValue);
        }
    }

    [Test]
    [TestCase(0, 10, false)]
    [TestCase(5, 10, false)]
    [TestCase(9, 10, false)]
    [TestCase(10, 10, false)]
    [TestCase(11, 10, true)]
    public void LessThanOrEqualTo_WithTestCases(int value, int lowerValue, bool expected)
    {
        if (expected)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThanOrEqualTo(value, lowerValue));
        }
        else
        {
            Guard.LessThanOrEqualTo(value, lowerValue);
        }
    }

    [Test]
    [TestCase(10, 0, false)]
    [TestCase(5, 0, false)]
    [TestCase(1, 0, false)]
    [TestCase(0, 0, true)]
    [TestCase(-1, 0, true)]
    public void GreaterThan_WithTestCases(int value, int lowerValue, bool expected)
    {
        if (expected)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThan(value, lowerValue));
        }
        else
        {
            Guard.GreaterThan(value, lowerValue);
        }
    }

    [Test]
    [TestCase(10, 0, false)]
    [TestCase(5, 0, false)]
    [TestCase(1, 0, false)]
    [TestCase(0, 0, false)]
    [TestCase(-1, 0, true)]
    public void GreaterThanOrEqualTo_WithTestCases(int value, int lowerValue, bool expected)
    {
        if (expected)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThanOrEqualTo(value, lowerValue));
        }
        else
        {
            Guard.GreaterThanOrEqualTo(value, lowerValue);
        }
    }

    [Test]
    [TestCase("a", "a", true)]
    [TestCase("a", "b", false)]
    public void DifferentInstances_WithTestCases(string a, string b, bool expected)
    {
        a = string.Intern(a);
        b = string.Intern(b);

        if (expected)
            Assert.Throws<ArgumentException>(() => Guard.DifferentInstances(a, b));
        else
            Guard.DifferentInstances(a, b);
    }

    [Test]
    public void Against_WithExpressionThatShouldFail_ThrowsInvalidOperationException()
    {
        var s = "TEST";
        Assert.Throws<InvalidOperationException>(() => Guard.Against(s == "TEST"));
    }

    [Test]
    public void Against_WithExpressionThatShouldSucceed_DoesNotThrow()
    {
        var s = "TEST 123";
        Guard.Against(s == "TEST");
    }

    [Test]
    public void Ensure_WithExpressionThatShouldFail_ThrowsInvalidOperationException()
    {
        var s = "TEST";
        Assert.Throws<InvalidOperationException>(() => Guard.Assert(s != "TEST"));
    }

    [Test]
    public void Ensure_WithExpressionThatShouldSucceed_DoesNotThrow()
    {
        var s = "TEST 123";
        Guard.Assert(s != "TEST");
    }
}