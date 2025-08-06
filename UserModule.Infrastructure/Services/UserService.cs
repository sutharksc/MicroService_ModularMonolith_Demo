using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedModule.Protos;
using SharedModule.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserModule.Application.Abstractions;
using UserModule.Application.Features.User;
using UserModule.Domain.Models;

namespace UserModule.Infrastructure.Services
{
    public class UserService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) : IUserService
    {
        public async Task<Result> CreateUserAsync(CreateUserCommand command)
        {
            // Check if the user already exists
            ApplicationUser? existingUser = await userManager.FindByEmailAsync(command.Email);

            if (existingUser != null) return Result.Failure(ClientErrors.EmailAlreadyExists);

            existingUser = await userManager.FindByEmailAsync(command.Email);

            if (existingUser != null) return Result.Failure(ClientErrors.EmailAlreadyExists);

            ApplicationUser user = new ApplicationUser
            {
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PhoneNumber = command.Phone,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true
            };

            IdentityResult createUserResult = await userManager.CreateAsync(user, command.Password);

            if (createUserResult.Succeeded)
                return Result.Success(message: "User Created");

            return Result.Failure(new Error("400", string.Join(", ", createUserResult.Errors.Select(e => e.Description))));
        }

        public async Task<Result> LoginUserAsync(LoginUserQuery query)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(query.Email);

            if (user == null) return Result.Failure(ClientErrors.UserNotFound);

            SignInResult result = await signInManager.PasswordSignInAsync(user, query.Password, false, false);

            if (result.Succeeded && !user.EmailConfirmed)
            {
                return Result.Failure(ClientErrors.UserAccountNotVerified);
            }

            if (!result.Succeeded && result.IsLockedOut)
                return Result.Failure(ClientErrors.UserLockout);

            if (!result.Succeeded && !result.RequiresTwoFactor)
                return Result.Failure(ClientErrors.InvalidCredentials);

            return Result.Success(user, "Login Successfully");
        }

        public TokenModel GetAccessToken(ApplicationUser user, IList<string> userRoles)
        {
            List<Claim> authClaims = new List<Claim>
                            {
                                new Claim(AppConsts.ClaimTypes.UserName, user.UserName!),
                                new Claim(AppConsts.ClaimTypes.Email, user.Email!),
                                new Claim(AppConsts.ClaimTypes.UserFullName, string.Concat(user.FirstName ," ",user.LastName)),
                                new Claim(ClaimTypes.Name, user.Id.ToString()),
                                new Claim(AppConsts.ClaimTypes.UserIdClaim, user.Id.ToString()),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            };


            foreach (string roleClaim in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleClaim));
            }

            JwtSecurityToken authToken = GetToken(authClaims, DateTime.Now.AddMinutes(15));
            string token = new JwtSecurityTokenHandler().WriteToken(authToken);

            return new TokenModel { Token = token, Expiration = authToken.ValidTo };
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims, DateTime expiration)
        {
            // Get the secret key from configuration for signing the JWT
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConsts.Jwt.Secret));

            // Create signing credentials using the HMAC-SHA256 algorithm and the secret key
            SigningCredentials credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT security token with issuer, audience, claims, and signing credentials
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: AppConsts.Jwt.ValidIssuer,
                audience: AppConsts.Jwt.ValidAudience,
                expires: expiration,
                claims: authClaims,
                signingCredentials: credentials
            );

            // Return the generated token
            return token;
        }

        public async Task<IList<string>> GetUsersRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<UserResponse?> GetUserByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return user.Adapt<UserResponse?>();
        }
    }
}
