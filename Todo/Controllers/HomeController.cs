using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
     public RedirectResult Insert(TodoItem todo)
{
    using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
    {
        using (var tableCmd = con.CreateCommand())
        {
            con.Open();
            tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
   return Redirect("http://localhost:5005/");
}
}
