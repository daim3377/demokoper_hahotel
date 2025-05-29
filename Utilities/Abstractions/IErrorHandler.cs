namespace Utilities.Abstractions
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}