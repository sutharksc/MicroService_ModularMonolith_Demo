using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using SharedModule.Abstractions;
using SharedModule.EndPoint;
using SharedModule.Shared;
using UserModule.Application.Abstractions;

namespace UserModule.Application.Features.User
{
    public class CreateUserCommand : IRequest<Result>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }

    public class CreateUserHandler(IHelperService helperService,
        IUserService userService,
        ILogger<CreateUserHandler> logger) : IRequestHandler<CreateUserCommand, Result>
    {
        public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            return await helperService.ReturnResultAsync(async () =>
            {
                await userService.CreateUserAsync(command);
                return Result.Success();
            }, logger);
        }
    }

    public class CreateUserEndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapPost("api/user/create", async (CreateUserCommand command, IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(command));
            }).RequireAuthorization();
        }
    }
}
