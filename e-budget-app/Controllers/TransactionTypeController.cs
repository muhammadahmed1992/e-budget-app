using Microsoft.AspNetCore.Mvc;

namespace e_budget_app.Controllers
{
    public class TransactionTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
