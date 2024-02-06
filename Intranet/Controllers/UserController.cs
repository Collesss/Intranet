using Intranet.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intranet.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewUser()
        { 
            return View(new UserViewModel() { Name = User.Identity.Name, Email = "test@test.ru", WorkPhone="9009" }); 
        }

        public IActionResult ViewUsers()
        {
            return View();
        }
    }
}
