using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserAccessApp.WebApi.Dtos;
using UserAccessApp.WebApi.Repositories;

namespace UserAccessApp.WebApi.Endpoints
{
    public static class UsersApi
    {
        public static void ConfigureUsersApi(this RouteGroupBuilder routes)
        {
            routes.MapGet("/users", async (IUserRepository userRepository, IMapper mapper) =>
            {
                var users = (await userRepository.GetAll()).Select(x => mapper.Map<UserDto>(x)).ToList();

                if (users == null || users.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(users);
            })
                .WithName("GetUsers")
                .WithTags("users");

            routes.MapPost("/users/batch-update", async (IUserRepository userRepository, [FromBody] List<UserUpdateDto> updates) =>
            {
                if (updates == null || !updates.Any())
                {
                    return Results.BadRequest("No updates provided.");
                }

                var result = await userRepository.UpdateUsersBatch(updates);

                if (result > 0)
                {
                    return Results.Ok(new { Message = "Users updated successfully." });
                }

                return Results.StatusCode(500);
            })
            .WithName("BatchUpdateUsers")
            .WithTags("users");
        }
    }
}
