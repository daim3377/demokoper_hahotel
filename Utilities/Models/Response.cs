using Utilities.Abstractions;

namespace Utilities.Models
{
    public record Response(int Status, string? Message) : IResponse
    {
        public const int Success = 0;
        public const int Fail = 1;
        public const int Warning = 2;
    }

    public record Response<T> : Response
    {
        public T Data { get; init; }
        public Response(int Status, string? Message, T data) : base(Status, Message) => Data = data;
        public Response<O> As<O>(Func<T, O> converter) => new(Status, Message, converter(Data));
    }

    public record ErrorResponse : Response, IErrorResponse
    {
        public Exception? Error { get; init; }
        public ErrorResponse(int Status, string? Message, Exception? exception) : base(Status, Message)
            => Error = exception;
    }

    public record FullResponse<T> : Response<T>, IErrorResponse
    {
        public Exception? Error { get; init; }
        public FullResponse(int Status, string? Message, T data, Exception? exception) : base(Status, Message, data)
            => Error = exception;

        public new FullResponse<O> As<O>(Func<T, O> converter) => new(Status, Message, converter(Data), Error);
        public async Task<FullResponse<O>> As<O>(Func<T, CancellationToken, Task<O>> converter, CancellationToken cancel) => new(Status, Message, await converter(Data, cancel), Error);
    }
}
