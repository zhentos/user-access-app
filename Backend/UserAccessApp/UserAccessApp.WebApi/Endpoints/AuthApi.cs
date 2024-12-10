using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserAccessApp.Infrastructure;
using UserAccessApp.WebApi.Dtos;
using UserAccessApp.WebApi.Infrastructure;
using UserAccessApp.WebApi.Repositories;


namespace UserAccessApp.WebApi.Endpoints
{
    public static class AuthApi
    {
        public static void ConfigureAuthApi(this RouteGroupBuilder routes)
        {
            routes.MapPost("auth/login", async ([FromBody] UserLoginDto dto,
               IUserRepository userRepository,
               TokenProvider provider,
               IValidator<UserLoginDto> validator,
               IPasswordHasher passwordHasher) =>
            {
                var validationResult = await validator.ValidateAsync(dto);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var user = await userRepository.GetByEmail(dto.Email);

                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var isPasswordValid = passwordHasher.VerifyPassword(dto.Password, user.Password, user.PasswordSalt);

                if ((!isPasswordValid))
                {
                    return Results.Unauthorized();
                }

                var token = provider.Create(user);

                var result = new LoginResponseDto
                {
                    AccessToken = token,
                    UserId = user.Id,
                    IsActive = user.IsActive,
                    UserName = $"{user.FirstName} {user.LastName}"
                };

                return Results.Ok(result);
            })
                 .WithName("Login")
                .WithTags("auth");
        }
    }
}
