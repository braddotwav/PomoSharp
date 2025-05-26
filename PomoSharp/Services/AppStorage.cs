using PomoSharp.Models;

namespace PomoSharp.Services;

public class AppStorage(JsonStorageProvider<Settings> settingsStorage,
                  JsonStorageProvider<Stats> statsStorage) : IAppStorage
{
    private readonly JsonStorageProvider<Settings> _settingsStorage = settingsStorage;
    private readonly JsonStorageProvider<Stats> _statsStorage = statsStorage;

    public Settings Settings => _settingsStorage.Data;
    public Stats Stats => _statsStorage.Data;

    public event Action? OnStorageSaved;

    public void Save()
    {
        _settingsStorage.Save();
        _statsStorage.Save();
        OnStorageSaved?.Invoke();
    }
}