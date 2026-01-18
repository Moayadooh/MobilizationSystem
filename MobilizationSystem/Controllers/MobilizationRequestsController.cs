using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;
using MobilizationSystem.Models;
using MobilizationSystem.Services;

namespace MobilizationSystem.Controllers
{
    //[Authorize]
    public class MobilizationRequestsController : Controller
    {
        private readonly MobilizationDbContext _context;
        //private readonly UserManager<IdentityUser> _userManager;
        private readonly IMobilizationService _mobilizationService;

        public MobilizationRequestsController(MobilizationDbContext context/*, UserManager<IdentityUser> userManager*/, IMobilizationService mobilizationService)
        {
            _context = context;
            //_userManager = userManager;
            _mobilizationService = mobilizationService;
        }

        private async Task PopulateLookupLists(CreateMobilizationRequestViewModel vm)
        {
            vm.Persons = await _context.Persons
                .Where(p => p.IsAvailable)
                .OrderBy(p => p.FullName)
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.FullName })
                .ToListAsync();

            vm.Resources = await _context.Resources
                .Where(r => r.IsAvailable)
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new CreateMobilizationRequestViewModel();
            await PopulateLookupLists(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMobilizationRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateLookupLists(vm);
                return View(vm);
            }

            var request = new MobilizationRequest
            {
                Purpose = vm.Purpose,
                RequestDate = DateTime.UtcNow,
                Status = RequestStatus.Submitted
            };

            // Add persons
            foreach (var pid in vm.SelectedPersonIds.Distinct())
            {
                request.MobilizationRequestPersons.Add(new MobilizationRequestPerson
                {
                    PersonId = pid
                });

                // mark person unavailable if desired:
                var person = await _context.Persons.FindAsync(pid);
                if (person != null) person.IsAvailable = false;
            }

            // Add resources
            foreach (var rid in vm.SelectedResourceIds.Distinct())
            {
                request.MobilizationRequestResources.Add(new MobilizationRequestResource
                {
                    ResourceId = rid
                });

                var resource = await _context.Resources.FindAsync(rid);
                if (resource != null) resource.IsAvailable = false;
            }

            _context.MobilizationRequests.Add(request);
            await _context.SaveChangesAsync();

            // Audit
            _context.AuditLogs.Add(new AuditLog
            {
                UserName = User.Identity!.Name ?? "unknown",
                Action = "CreateMobilizationRequest",
                TableName = nameof(MobilizationRequest),
                RecordId = request.Id,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            //return RedirectToAction("Index", "Dashboard");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
            => View(_context.MobilizationRequests.ToList());

        //[Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            //bool isFinalApproval = User.IsInRole("Admin");

            await _mobilizationService.ApproveAsync(
                id,
                "unknown", //User.Identity!.Name!,
                true //isFinalApproval
            );

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Complete(int id)
        {
            await _mobilizationService.CompleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
