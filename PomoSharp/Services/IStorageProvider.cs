namespace PomoSharp.Services;

public interface IStorageProvider<T>
{
    public void Save();
    public T Load();
}