namespace SharedModule.Abstractions
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }

        Guid UserId { get; }

        string Email { get; }

        string UserName { get; }

        string UserFullName { get; }

        DateTime? ExpirationTime { get; }

        string AccessToken { get; }

        List<string> UserRoles { get; }
    }
}
