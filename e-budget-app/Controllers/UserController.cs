using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using e_budget_app.Entities;
using e_budget_app.Utilities.Dtos;

namespace e_budget_app.Controllers
{
    [Route("/api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string? _connectionString;
        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        string query = "SELECT * FROM users WHERE UserId = @UserId";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserId", id);

                            using (SqlDataReader dataReader = cmd.ExecuteReader())
                            {
                                if (dataReader.Read())
                                {
                                    User user = new User
                                    {
                                        Id = (int)dataReader["UserId"],
                                        UserName = dataReader["UserName"].ToString(),
                                        Gmail = dataReader["Gmail"].ToString(),
                                        Password = dataReader["Password"].ToString()
                                    };

                                    return Ok(user);
                                }
                                else
                                {
                                    return NotFound();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM users", connection))
                        {
                            List<User> users = new List<User>();

                            connection.Open();

                            using (SqlDataReader dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    User user = new User
                                    {
                                        Id = (int)dataReader["UserId"],
                                        UserName = dataReader["UserName"].ToString(),
                                        Gmail = dataReader["Gmail"].ToString(),
                                        Password = dataReader["Password"].ToString()
                                    };

                                    users.Add(user);
                                }
                            }
                            return Ok(users);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        string query = "Insert into Users(UserName, Password, Gmail) Values(@UserName, @Password, @Gmail); " +
                            "SELECT CAST(scope_identity() AS int)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserName", userDto.UserName);
                            cmd.Parameters.AddWithValue("@Gmail", userDto.Gmail);
                            cmd.Parameters.AddWithValue("@Password", userDto.Password);

                            connection.Open();

                            Int32? userId = (Int32)cmd.ExecuteScalar();

                            if (userId != null)
                            {
                                return Ok($"User Created with ID: {userId}");
                            }
                            else
                            {
                                return BadRequest("User Creation Failed");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserAtId([FromRoute] int id, [FromBody] UserDto userDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        string query = "UPDATE Users " +
                            "SET UserName = @UserName, " +
                            "Password = @Password, " +
                            "Gmail = @Gmail " +
                            "WHERE UserID = @UserID;";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", id);
                            cmd.Parameters.AddWithValue("@UserName", userDto.UserName);
                            cmd.Parameters.AddWithValue("@Gmail", userDto.Gmail);
                            cmd.Parameters.AddWithValue("@Password", userDto.Password);

                            connection.Open();

                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                return Ok(new { status = "200", message = $"User Updated at {id}" });
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserAtId ([FromRoute] int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        string query = "Delete Users WHERE UserID = @UserID;";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", id);

                            connection.Open();

                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                return Ok(new { status = "200", message = $"User Deleted at {id}" });
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
