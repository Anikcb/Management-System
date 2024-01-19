namespace ManagementSystem.Services
{
    public interface IRedisService
    {
        void SetKey<T>(string key, T value);
        T GetKey<T>(string key);
    }
}
