using LVK.Core.Collections;

namespace LVK.Core.Tests.Collections;

public class TopologicalSortTests
{
    [Test]
    public void Sort_NoInputs_NoOutputs()
    {
        IEnumerable<int> outputs = TopologicalSort.Sort<int>([], []);
        Assert.That(outputs, Is.Empty);
    }

    [Test]
    public void Sort_NoDependencies_ReturnsAllItems()
    {
        IEnumerable<int> outputs = TopologicalSort.Sort<int>([1, 2, 3, 4, 5], []);
        Assert.That(outputs, Is.EquivalentTo(new[]
        {
            1, 2, 3, 4, 5,
        }));
    }

    [Test]
    [TestCase(1, 2, 1, 2, 2, 1)]
    [TestCase(2, 1, 1, 2, 2, 1)]
    [TestCase(1, 2, 2, 1, 1, 2)]
    [TestCase(2, 1, 2, 1, 1, 2)]
    public void Sort_WithDependencies_ReturnsItemsInCorrectOrder(int item1, int item2, int item, int itemDependency, int expectedOutput1, int expectedOutput2)
    {
        IEnumerable<int> outputs = TopologicalSort.Sort<int>([item1, item2], [(item, itemDependency)]);

        Assert.That(outputs, Is.EqualTo(new[]
        {
            expectedOutput1, expectedOutput2,
        }));
    }

    [Test]
    public void Sort_WithCyclicDependency_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => TopologicalSort.Sort<int>([1, 2], [(1, 2), (2, 1)]).ToList());
    }
}