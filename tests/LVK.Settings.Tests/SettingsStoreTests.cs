using LVK.Typed;

using NSubstitute;

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

        var store = new SettingsStore(Substitute.For<ITypeHelper>());

        await store.SaveAsync("TEST", settings);
        Settings loaded = await store.LoadAsync<Settings>("TEST");

        Assert.That(loaded.A, Is.EqualTo(settings.A));
        Assert.That(loaded.B, Is.EqualTo(settings.B));
    }

    [Test]
    [TestCase("", "")]
    [TestCase("aa", "bb")]
    [TestCase("bb", "aa")]
    public async Task SaveAndLoadWithoutName_ReturnsSavedSettings_WithTestCases(string a, string b)
    {
        var settings = new Settings
        {
            A = a, B = b,
        };

        ITypeHelper typeHelper = Substitute.For<ITypeHelper>();
        typeHelper.NameOf<Settings>().Returns("settings");

        var store = new SettingsStore(typeHelper);

        await store.SaveAsync(settings);
        Settings loaded = await store.LoadAsync<Settings>();

        Assert.That(loaded.A, Is.EqualTo(settings.A));
        Assert.That(loaded.B, Is.EqualTo(settings.B));
    }
}

public class Settings
{
    public string A { get; init; } = "";
    public string B { get; init; } = "";
}