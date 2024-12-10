using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UserAccessApp.WebApi.Dtos;
using UserAccessApp.WebApi.Models;

namespace UserAccessApp.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ILogger<UserRepository> _logger;
        private readonly string _connectionString;
        public UserRepository(ILogger<UserRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var users = await connection.QueryAsync<User>("SELECT * FROM Users");
                    return users;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while retrieving users.");
                    return [];
                }
            }
        }

        public async Task<bool> Update(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var result = await connection.ExecuteAsync(
                        "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email," +
                        " Password = @Password, PasswordSalt = @PasswordSalt, LastModifiedDate = GETDATE(), IsActive = @IsActive WHERE Id = @Id",
                        user);
                    return result > 0; // Return true if any rows were affected
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the user.");
                    return false; // Indicate failure to update
                }
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT TOP(1) * FROM Users WHERE Email = @email", new { Email = email });
                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while retrieving the user with Email = {email}.");
                    return null;
                }
            }
        }

        public async Task<int> UpdateUsersBatch(IEnumerable<UserUpdateDto> updates)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Create a DataTable to hold the updates
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("UserId", typeof(int));
                    dataTable.Columns.Add("IsActive", typeof(bool));

                    // Populate the DataTable with updates
                    foreach (var update in updates)
                    {
                        dataTable.Rows.Add(update.UserId, update.IsActive);
                    }

                    // Use the stored procedure with TVP
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserUpdates", dataTable.AsTableValuedParameter("UserUpdateType"));

                    // Call the stored procedure
                    var result = await connection.ExecuteAsync("UpdateUserStatus", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the users.");
                    return 0;
                }
            }
        }
    }
}
