using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AuditLogsController : Controller
    {
        private readonly MobilizationDbContext _context;

        public AuditLogsController(MobilizationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var logs = _context.AuditLogs
                .OrderByDescending(l => l.CreatedAt)
                .Take(500)
                .ToList();

            return View(logs);
        }
    }
}
