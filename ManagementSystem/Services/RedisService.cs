using StackExchange.Redis;
using System;
using System.Text.Json;

namespace ManagementSystem.Services;
public class RedisService
{
    private readonly IConnectionMultiplexer _connection;

    public RedisService(IConnectionMultiplexer connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public void SetKey<T>(string key, T value)
    {
        var database = _connection.GetDatabase();
        var serializedValue = JsonSerializer.Serialize(value);
        database.StringSet(key, serializedValue);
    }

    public T GetKey<T>(string key)
    {
        var database = _connection.GetDatabase();
        var serializedValue = database.StringGet(key);

        if (!serializedValue.IsNullOrEmpty)
        {
            var deserializedValue = JsonSerializer.Deserialize<T>(serializedValue);
            return deserializedValue;
        }

        return default;
    }
}
