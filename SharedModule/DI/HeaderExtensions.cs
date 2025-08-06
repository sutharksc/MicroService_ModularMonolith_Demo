using Microsoft.AspNetCore.Http;

namespace SharedModule.DI
{
    internal static class HeaderExtensions
    {
        public static string Get(this IHeaderDictionary? headers, string headerName)
        {
            return headers != null && headers.TryGetValue(headerName, out var value)
            ? value.ToString()
            : string.Empty;
        }

        public static string GetAuthToken(this IHeaderDictionary? headers, string headerName)
        {
            string? headerValue = headers.Get(headerName)?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

            return !string.IsNullOrEmpty(headerValue)
                ? headerValue
                : string.Empty;
        }
    }
}
