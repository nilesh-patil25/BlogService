﻿using BlogService.Core.AppSettings;
using BlogService.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace BlogService.Core.Services
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDB;

        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect(AppSettingsHelper.GetValue(ConfigConstants.CacheConnectionString));
            _cacheDB = redis.GetDatabase();
        }
        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _cacheDB.StringGetAsync(key);
            if(!string.IsNullOrEmpty(value))
                return JsonSerializer.Deserialize<T>(value);

            return default;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            try
            {
                var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
                return await _cacheDB.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while setting data for key: {key}");
                return false;
            }
        }


        public async Task<object> RemoveDataAsync(string key)
        {
            bool keyExists = await _cacheDB.KeyExistsAsync(key);
            if(keyExists)
            {
                return await _cacheDB.KeyDeleteAsync(key);
            }
            return false;
        }

    }
}
