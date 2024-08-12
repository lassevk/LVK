namespace LVK.Core.App.Console.Tests;

public class ProgressBarTests
{
    [TestCase(0000, 1000, "[                         ]   0.0%")]
    [TestCase(1000, 1000, "[\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588] 100.0%")]
    [TestCase(0005, 1000, "[\u258f                        ]   0.5%")]
    [TestCase(0010, 1000, "[\u258e                        ]   1.0%")]
    [TestCase(0015, 1000, "[\u258d                        ]   1.5%")]
    [TestCase(0020, 1000, "[\u258c                        ]   2.0%")]
    [TestCase(0025, 1000, "[\u258b                        ]   2.5%")]
    [TestCase(0030, 1000, "[\u258a                        ]   3.0%")]
    [TestCase(0035, 1000, "[\u2589                        ]   3.5%")]
    [TestCase(0040, 1000, "[\u2588                        ]   4.0%")]
    [TestCase(0045, 1000, "[\u2588\u258f                       ]   4.5%")]
    public void Get_WithTestCases(int current, int total, string expected)
    {
        string output = ProgressBar.Format(current, total);

        Assert.That(output, Is.EqualTo(expected));
    }
}