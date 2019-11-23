using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Library
{
    public static class ExceptionHandler
    {
        public static void Try(this Action @try, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                @try();
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task TryAsync(this Func<Task> @try, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                await @try();
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static T Try<T>(this Func<T> @try, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                return @try();
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task<T> TryAsync<T>(this Func<Task<T>> @try, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                return await @try();
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        
        public static TOut Try<TIn, TOut>(this Func<TIn, TOut> @try, TIn value, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                return @try(value);
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task TryAsync<T>(this Func<T, Task> @try, T value, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                await @try(value);
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        public static async Task<TOut> TryAsync<TIn, TOut>(this Func<TIn, Task<TOut>> @try, TIn value, Action<Exception> @catch = null, 
            Action @finally = null, bool catchAll = false, params Type[] handledExceptions)
        {
            try
            {
                return await @try(value);
            }
            catch (Exception exception)
            {
                if(catchAll || IsExceptionHandled(exception, handledExceptions))
                    @catch?.Invoke(exception);

                throw;
            }
            finally
            {
                @finally?.Invoke();
            }
        }

        private static bool IsExceptionHandled(this Exception exception, IEnumerable<Type> handledExceptionTypes)
        {
            return handledExceptionTypes.Any(x => exception.GetType() == x);
        }
    }
}
