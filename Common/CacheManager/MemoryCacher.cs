﻿using System;
using System.Runtime.Caching;

public class MemoryCacher
{
    public static object GetValue(string key)
    {
        MemoryCache memoryCache = MemoryCache.Default;
        return memoryCache.Get(key);
    }

    public static bool Add(string key, object value, DateTimeOffset absExpiration)
    {
        MemoryCache memoryCache = MemoryCache.Default;
        return memoryCache.Add(key, value, absExpiration);
    }

    public static void Delete(string key)
    {
        MemoryCache memoryCache = MemoryCache.Default;
        if (memoryCache.Contains(key))
        {
            memoryCache.Remove(key);
        }
    }

    public static implicit operator MemoryCache(MemoryCacher v)
    {
        throw new NotImplementedException();
    }
}