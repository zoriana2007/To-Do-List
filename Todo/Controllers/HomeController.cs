using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Todo.Models.ViewModels;

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
        //return View();
        var todoListViewModel=GetAllTodos();
        return View(todoListViewModel);

    }
    internal TodoViewModel GetAllTodos()
    {
        List<TodoItem> todoList = new();
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "SELECT * FROM todo";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            todoList.Add(
                                new TodoItem
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                }
                            );
                        }
                    }
                    else
                    {
                        return new TodoViewModel
                        {
                            TodoList = todoList
                        };
                    }
                }
                ;
            }

        }
        return new TodoViewModel
        {
            TodoList = todoList
        };
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
        return Redirect("http://localhost:5113/");//5005
    }
}
