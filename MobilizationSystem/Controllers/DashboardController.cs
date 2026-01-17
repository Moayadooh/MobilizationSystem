using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly MobilizationDbContext _context;

        public DashboardController(MobilizationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalRequests = _context.MobilizationRequests.Count();
            ViewBag.Active = _context.MobilizationRequests.Count(x => x.Status == RequestStatus.Activated);
            ViewBag.AvailablePersons = _context.Persons.Count(x => x.IsAvailable);
            return View();
        }
    }
}
