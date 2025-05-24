using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private const string ConnectionString = "Data Source=db.sqlite";

        [HttpPost()]
        [Route("Home/ApiLogin")]
        public IActionResult Login([FromBody] User user)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(*) FROM Users WHERE Username = '{user.Username}' AND Password = '{user.Password}'";
            long count = (long)cmd.ExecuteScalar();

            if (count > 0)
                return Ok(new { success = true, message = "Login successful" });

            return BadRequest(new { success = false, message = "Invalid credentials" });
        }

        [HttpPost()]
        [Route("Home/ApiRegister")]
        public IActionResult Register([FromBody] User user)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT COUNT(*) FROM Users WHERE Username = '{user.Username}'";
            long count = (long)checkCmd.ExecuteScalar();

            if (count > 0)
                return Conflict(new { success = false, message = "User already exists" });

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO Users (Username, Password) VALUES ('{user.Username}', '{user.Password}')";
            insertCmd.ExecuteNonQuery();

            return Ok(new { success = true, message = "Registration successful" });
        }
    }
}
