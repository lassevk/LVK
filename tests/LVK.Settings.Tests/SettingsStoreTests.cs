namespace LVK.Settings.Tests;

public class SettingsStoreTests
{
    [Test]
    [TestCase("", "")]
    [TestCase("aa", "bb")]
    [TestCase("bb", "aa")]
    public async Task SaveAndLoad_ReturnsSavedSettings_WithTestCases(string a, string b)
    {
        var settings = new Settings
        {
            A = a, B = b,
        };
        var store = new SettingsStore();

        await store.SaveAsync("TEST", settings);
        Settings loaded = await store.LoadAsync<Settings>("TEST");

        Assert.That(loaded.A, Is.EqualTo(settings.A));
        Assert.That(loaded.B, Is.EqualTo(settings.B));
    }
}

public class Settings
{
    public string A { get; init; } = "";
    public string B { get; init; } = "";
}