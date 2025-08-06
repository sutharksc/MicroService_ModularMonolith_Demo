using System.Net;

namespace SharedModule.Shared
{
    public class Result
    {
        internal Result(bool isSuccess, Error error, object? data = null, string? message = null)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid Eror", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Data = data;
            Message = message;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;
        public string? Message { get; }
        public Error Error { get; }
        public object? Data { get; }

        public static Result Success(object? data = null, string? message = null) => new(true, Error.None, data, message);
        public static Result Failure(Error error, int status = (int)HttpStatusCode.BadRequest) => new(false, error);
    }

    public sealed record Error(string Code, string Description, int? Status = (int)HttpStatusCode.BadRequest)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);
    }
}
