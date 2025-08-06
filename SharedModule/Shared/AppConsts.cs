namespace SharedModule.Shared
{
    public class AppConsts
    {
        public const string OrderGrpcServiceUrl = "https://localhost:7027";
        public const string BookingGrpcServiceUrl = "https://localhost:7096";
        public const string UserGrpcServiceUrl = "https://localhost:7105";

        public static class ClaimTypes
        {
            public const string UserIdClaim = "UserId";
            public const string UserName = "UserName";
            public const string Email = "Email";
            public const string UserFullName = "UserFullName";
        }

        public static class Jwt
        {
            public const string ValidAudience = "UserId";
            public const string ValidIssuer = "UserName";
            public const string Secret = "clknaOIHiiceAnlKsdyiuewKJBkcbJsdiuGBIASKBcsdbvciks";
        }

        public static class Headers
        {
            public static readonly string AccessToken = "Authorization";
        }
    }
}

