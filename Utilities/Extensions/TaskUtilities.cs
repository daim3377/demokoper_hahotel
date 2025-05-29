using Utilities.Abstractions;

namespace Utilities.Extensions
{
    public static class TaskUtilities
    {
        public static async Task FireSafeAsync(this Task task, IErrorHandler? handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }

        public static async Task<TOutput?> FireSafeAsync<TOutput>(this Task<TOutput> task, IErrorHandler? handler = null)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
            return default;
        }

        public static T Fire<T>(this T response, IErrorHandler handler) where T : IErrorResponse
        {
            if (response.Error is not null)
                handler.HandleError(response.Error);

            return response;
        }

        public static async void Forget(this Task task) => await task;
    }
}