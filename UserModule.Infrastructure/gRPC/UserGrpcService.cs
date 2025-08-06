using Grpc.Core;
using SharedModule.Protos;
using UserModule.Application.Abstractions;

namespace UserModule.Infrastructure.gRPC
{
    public class UserGrpcService(IUserService userService) : UserService.UserServiceBase
    {
        public override async Task<UserResponse?> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await userService.GetUserByIdAsync(request.UserId);

            return user;
        }
    }
}
