using SharedModule.Protos;
using SharedModule.Shared;
using UserModule.Application.Features.User;
using UserModule.Domain.Models;

namespace UserModule.Application.Abstractions
{
    public interface IUserService
    {
        Task<Result> CreateUserAsync(CreateUserCommand command);
        Task<Result> LoginUserAsync(LoginUserQuery query);
        Task<IList<string>> GetUsersRolesAsync(ApplicationUser user);
        TokenModel GetAccessToken(ApplicationUser user, IList<string> userRoles);
        Task<UserResponse?> GetUserByIdAsync(string userId);
    }
}
