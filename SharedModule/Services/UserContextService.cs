using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SharedModule.Abstractions;
using SharedModule.DI;
using SharedModule.Shared;

namespace SharedModule.Services
{
    [Authorize]
    public class UserContextService(IHttpContextAccessor _httpContextAccessor) : IUserContext
    {
        public bool IsAuthenticated => _httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated ?? false;

        public Guid UserId => _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ?? throw new UnauthorizedAccessException("Invalid or missing user ID claim.");

        public string Email => _httpContextAccessor
            .HttpContext?
            .User
            .GetUserDataOf(AppConsts.ClaimTypes.Email) ?? string.Empty;

        public string UserName => _httpContextAccessor
            .HttpContext?
            .User
            .GetUserDataOf(AppConsts.ClaimTypes.UserName) ?? string.Empty;

        public string UserFullName => _httpContextAccessor
            .HttpContext?
            .User
            .GetUserDataOf(AppConsts.ClaimTypes.UserFullName) ?? string.Empty;

        public DateTime? ExpirationTime
        {
            get
            {
                var expirationClaim = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst("exp");

                if (expirationClaim != null && long.TryParse(expirationClaim.Value, out long expUnixTime))
                {
                    return DateTimeOffset.FromUnixTimeSeconds(expUnixTime).DateTime;
                }

                return null;
            }
        }

        public string AccessToken => _httpContextAccessor.HttpContext?
            .Request
            .Headers
            .GetAuthToken(AppConsts.Headers.AccessToken) ?? string.Empty;



        public List<string> UserRoles => _httpContextAccessor.HttpContext?
            .User
            .GetRoles() ?? new List<string>();
    }
}
