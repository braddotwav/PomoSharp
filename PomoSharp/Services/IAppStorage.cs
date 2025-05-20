using PomoSharp.Models;

namespace PomoSharp.Services;

public interface IAppStorage
{
    public Settings Settings { get; }
    public Stats Stats { get; }
    public void Save();
}