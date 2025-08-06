using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using SharedModule.Abstractions;
using SharedModule.EndPoint;
using SharedModule.Results;
using SharedModule.Shared;
using UserModule.Application.Abstractions;
using UserModule.Domain.Models;

namespace UserModule.Application.Features.User
{
    public class LoginUserQuery : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserResult User { get; set; }
    }

    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class LoginUserHandler(IHelperService helperService,
        IUserService userService,
        ILogger<LoginUserHandler> logger) : IRequestHandler<LoginUserQuery, Result>
    {
        public async Task<Result> Handle(LoginUserQuery query, CancellationToken cancellationToken)
        {
            return await helperService.ReturnResultAsync(async () =>
            {
                Result loginResult = await userService.LoginUserAsync(query);
                if (loginResult.IsFailure) return loginResult;

                ApplicationUser user = (ApplicationUser)loginResult.Data!;

                IList<string> userRoles = await userService.GetUsersRolesAsync(user);

                TokenModel accessToken = userService.GetAccessToken(user, userRoles);

                Result result = Result.Success(
                new LoginResult
                {
                    Token = accessToken.Token,
                    Expiration = accessToken.Expiration,
                    User = new UserResult()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName!,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = userRoles,
                    }
                }, "Login Successfully");

                return result;
            }, logger);
        }

        public class LoginUserEndPoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder builder)
            {
                builder.MapPost("api/auth/login", async (LoginUserQuery query, IMediator mediator) =>
                {
                    return Results.Ok(await mediator.Send(query));
                });

            }
        }
    }
}
