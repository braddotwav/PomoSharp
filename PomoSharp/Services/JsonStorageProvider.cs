using System.IO;
using System.Text.Json;

namespace PomoSharp.Services;

public class JsonStorageProvider<T> : IStorageProvider<T> where T : new()
{
    private const string FILE_SUFFIX = ".json";

    public T Data;

    private readonly string _basePath;
    private readonly string _fileName;
    private readonly string _fullPath;

    private readonly JsonSerializerOptions _serializerOptions = new() { WriteIndented = true };

    public JsonStorageProvider(string fileName)
    {
        _fileName = string.Concat(fileName, FILE_SUFFIX);
        _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PomoSharp");
        _fullPath = Path.Combine(_basePath, _fileName);

        ValidateDirectory(_basePath);

        Data = Load();
    }

    public void Save()
    {
        ValidateDirectory(_basePath);
        
        string content = JsonSerializer.Serialize(Data, _serializerOptions);
        File.WriteAllText(_fullPath, content);
    }

    public T Load()
    {
        try
        {
            var content = File.ReadAllText(_fullPath);

            if (!string.IsNullOrWhiteSpace(content))
            {
                Data = JsonSerializer.Deserialize<T>(content) ?? LoadDefault();
            }
        }
        catch (Exception)
        {
            // Log if necessary
        }

        return Data ??= LoadDefault();
    }

    private T LoadDefault()
    {
        return new T();
    }

    private void ValidateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}