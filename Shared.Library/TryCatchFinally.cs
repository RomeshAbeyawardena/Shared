using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Library
{
    public static class ExceptionHandler
    {
        public static void Try(Action @try, Action<Exception> @catch, Action @finally = null, params Exception[] exceptions)
        {
            try
            {
                @try();
            }
            catch (Exception ex)
            {
                if(!exceptions.Any(x => ex == x))
                    throw;

                @catch(ex);
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task TryAsync(Func<Task> @try, Action<Exception> @catch, Action @finally = null, params Exception[] exceptions)
        {
            try
            {
                await @try();
            }
            catch (Exception ex)
            {
                if(!exceptions.Any(x => ex == x))
                    throw;

                @catch(ex);
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task<T> TryAsync<T>(Func<Task<T>> @try, Action<Exception> @catch, Action @finally = null, params Exception[] exceptions)
        {
            try
            {
                return await @try();
            }
            catch (Exception ex)
            {
                if(!exceptions.Any(x => ex == x))
                    throw;

                @catch(ex);
            }
            finally
            {
                @finally?.Invoke();
            }

            return default;
        }

        public static T Try<T>(Func<T> @try, Action<Exception> @catch, Action @finally = null, params Exception[] exceptions)
        {
            try
            {
                return @try();
            }
            catch (Exception ex)
            {
                if(!exceptions.Any(x => ex == x))
                    throw;

                @catch(ex);
            }
            finally
            {
                @finally?.Invoke();
            }

            return default;
        }
    }
}
