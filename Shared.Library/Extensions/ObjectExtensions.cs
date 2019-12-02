﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Library.Extensions
{
    public static class ObjectExtensions
    {
        public static T AssignOrDefault<T>(this T value, T defaultValue)
        {
            return (T)AssignOrDefault((object)value, (object)defaultValue);
        }
        public static object AssignOrDefault(this object value, object defaultValue)
        {
            if(value == null || value == default)
                return defaultValue;

            return value;
        }
        public static bool TryParse(this string value, out int result)
        {
            return int.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out bool result)
        {
            return bool.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out double result)
        {
            return double.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out float result)
        {
            return float.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out DateTime result)
        {
            return DateTime.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out Guid result)
        {
            return Guid.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out DateTimeOffset result)
        {
            return DateTimeOffset.TryParse(value, out result);
        }

        public static bool TryParse<T>(this object value, out T? result)
            where T: struct
        {
            result = default;
            
            if(!(value is T tResult))
                return false;
            
            result = tResult;
            
            return true;
        }

        public static bool TryParse<T>(this object value, out T result)
            where T: class
        {
            result = default;
            
            if(!(value is T tResult))
                return false;
            
            result = tResult;
            
            return true;
        }

        public static void AsLock(this object value, Action onLock)
        {
            lock (value)
            {
                onLock();
            }
        }

        public static T AsLock<T>(this object value, Func<T> onLock)
        {
            lock (value)
            {
                return onLock();
            }
        }

        public static async Task AsLockAsync(this SemaphoreSlim semaphoreSlim , Func<Task> onLock)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                await onLock();
            }
            finally
            {
                semaphoreSlim.Release();
            }
            
        }

        public static async Task<T> AsLockAsync<T>(this SemaphoreSlim semaphoreSlim , Func<Task<T>> onLock)
        {
            await semaphoreSlim.WaitAsync();
            try{
                return await onLock();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
