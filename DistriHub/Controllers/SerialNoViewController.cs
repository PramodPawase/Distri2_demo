using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DistriHub.Repository;

namespace DistriHub.Controllers
{
    [AllowAnonymous]
    public class SerialNoViewController : Controller
    {
        private readonly IRepository _repo;

        public SerialNoViewController(IRepository repo)
        {
            _repo = repo;
        }

        // GET /SerialNoView?serial=...
        public async Task<IActionResult> Index([FromQuery] string? serial)
        {
            var products = await _repo.GetProductDetailsAsync(serial);
            ViewData["Serial"] = serial ?? string.Empty;
            return View(products);
        }
    }
}
