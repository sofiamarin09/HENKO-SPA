using System;
using System.Collections.Concurrent;

namespace HankoSpa.Services
{
    public sealed class SimpleCacheManager
    {
        private static readonly Lazy<SimpleCacheManager> _instance = new(() => new SimpleCacheManager());
        private readonly ConcurrentDictionary<string, object> _cache = new();

        private SimpleCacheManager() { }

        public static SimpleCacheManager Instance => _instance.Value;

        public void Set(string key, object value)
        {
            _cache[key] = value;
        }

        public T? Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;
            return default;
        }

        public void Remove(string key)
        {
            _cache.TryRemove(key, out _);
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}