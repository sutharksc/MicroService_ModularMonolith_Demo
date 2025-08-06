using Microsoft.Extensions.Logging;
using SharedModule.Abstractions;
using SharedModule.Shared;

namespace SharedModule.Services
{
    public class HelperService : IHelperService
    {
        public async Task<Result> ReturnResultAsync(Func<Task<Result>> action,
            ILogger logger)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error :- {ex.Message}");
                return Result.Failure(ClientErrors.InternalServerError(ex.Message));
            }
        }
    }
}
