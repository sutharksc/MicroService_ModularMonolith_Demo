using SharedModule.Shared;
using System.Security.Claims;

namespace SharedModule.DI
{
    internal static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal? principal)
        {
            string? userId = principal?.GetUserDataOf(AppConsts.ClaimTypes.UserIdClaim);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Invalid or missing user ID claim.");

            return Guid.Parse(userId);
        }

        public static string GetUserDataOf(this ClaimsPrincipal? principal, string claimTypes)
        {
            string? userData = principal?.FindFirst(claimTypes)?.Value;

            return !string.IsNullOrEmpty(userData) ?
                userData :
                string.Empty;
        }

        public static List<string> GetRoles(this ClaimsPrincipal? principal)
        {
            IEnumerable<Claim>? claims = principal?.Claims;

            if (claims == null)
                return [];

            return claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        }
    }
}
