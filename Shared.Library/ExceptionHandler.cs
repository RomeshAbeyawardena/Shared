using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Library
{
    public static class ExceptionHandler
    {
        public static void Try(this Action @try, Action<Exception> @catch, Action @finally = null, params Type[] exceptionTypes)
        {
            try
            {
                @try();
            }
            catch (Exception ex)
            {
                if(!ex.IsExceptionCaught(exceptionTypes))
                    throw;

                @catch?.Invoke(ex);
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task TryAsync(this Func<Task> @try, Action<Exception> @catch, 
            Func<Exception, Task> catchAsync = null, Action @finally = null, params Type[] exceptionTypes)
        {
            try
            {
                await @try();
            }
            catch (Exception ex)
            {
                if(!ex.IsExceptionCaught(exceptionTypes))
                    throw;

                @catch?.Invoke(ex);
                catchAsync?.Invoke(ex);
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task<T> TryAsync<T>(this Func<Task<T>> @try, Action<Exception> @catch, 
            Func<Exception, Task> catchAsync = null, Action @finally = null, params Type[] exceptionTypes)
        {
            try
            {
                return await @try();
            }
            catch (Exception ex)
            {
                if(!ex.IsExceptionCaught(exceptionTypes))
                    throw;

                @catch?.Invoke(ex);
                catchAsync?.Invoke(ex);
            }
            finally
            {
                @finally?.Invoke();
            }

            return default;
        }

        public static T Try<T>(this Func<T> @try, 
            Action<Exception> @catch = null, 
            Action @finally = null, 
            params Type[] exceptionTypes)
        {
            try
            {
                return @try();
            }
            catch (Exception ex)
            {
                if(!ex.IsExceptionCaught(exceptionTypes))
                    throw;

                @catch?.Invoke(ex);
            }
            finally
            {
                @finally?.Invoke();
            }

            return default;
        }

        public static bool IsExceptionCaught(this Exception exception, IEnumerable<Type> caughtExceptionTypes)
        {
            return caughtExceptionTypes.Any(x => exception.GetType() == x);
        }
    }
}
