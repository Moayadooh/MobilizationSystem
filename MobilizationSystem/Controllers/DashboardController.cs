using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;
using MobilizationSystem.Models;

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
            var vm = new DashboardViewModel
            {
                // Requests
                TotalRequests = _context.MobilizationRequests.Count(),
                PendingRequests = _context.MobilizationRequests.Count(r =>
                    r.Status == RequestStatus.Submitted ||
                    r.Status == RequestStatus.Approved),

                ActiveRequests = _context.MobilizationRequests.Count(r =>
                    r.Status == RequestStatus.Activated),

                CompletedRequests = _context.MobilizationRequests.Count(r =>
                    r.Status == RequestStatus.Completed),

                CancelledRequests = _context.MobilizationRequests.Count(r =>
                    r.Status == RequestStatus.Cancelled),

                // Persons
                TotalPersons = _context.Persons.Count(),
                AvailablePersons = _context.Persons.Count(p => p.IsAvailable),
                AssignedPersons = _context.Persons.Count(p => !p.IsAvailable),

                // Resources
                TotalResources = _context.Resources.Count(),
                AvailableResources = _context.Resources.Count(r => r.IsAvailable),
                AssignedResources = _context.Resources.Count(r => !r.IsAvailable)
            };

            return View(vm);
        }
    }
}
