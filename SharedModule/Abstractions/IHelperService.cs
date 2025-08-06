using Microsoft.Extensions.Logging;
using SharedModule.Shared;

namespace SharedModule.Abstractions
{
    public interface IHelperService
    {
        Task<Result> ReturnResultAsync(Func<Task<Result>> action, ILogger logger);
    }
}
