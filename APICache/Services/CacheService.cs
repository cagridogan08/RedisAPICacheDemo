using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace APICache.Services;

public class CacheService:ICacheService
{
    private IDatabase? _database;

    public CacheService()
    {
        ConfigureRedis();
    }

    private void ConfigureRedis()
    {
        _database = ConnectionHelper.Connection.GetDatabase();
    }
    public T? GetData<T>(string key)
    {
        Debug.Assert(_database != null, nameof(_database) + " != null");
        var val = _database.StringGet(key);
        return !string.IsNullOrEmpty(val) ? JsonSerializer.Deserialize<T>(val) : default;
    }

    public bool SetData<T>(string key, T data, DateTimeOffset expireTime)
    {
        var expirationTime = expireTime.DateTime.Subtract(DateTime.Now);
        var isSet = _database.StringSet(key, JsonSerializer.Serialize(data), expirationTime);
        return isSet;
    }

    public object RemoveData(string key)
    {
        var isExist = _database.KeyExists(key);
        if (isExist)
        {
            return _database.KeyDelete(key);
        }

        return false;
    }
}