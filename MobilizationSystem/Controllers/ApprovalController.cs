using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Controllers
{
    //[Authorize]
    public class ApprovalController : Controller
    {
        private readonly MobilizationDbContext _context;

        public ApprovalController(MobilizationDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Officer,Admin")]
        public IActionResult Approve(int id)
        {
            var request = _context.MobilizationRequests.Find(id);
            if (request == null) return NotFound();

            request.Status = User.IsInRole("Admin")
                ? RequestStatus.Activated
                : RequestStatus.Approved;

            _context.Approvals.Add(new Approval
            {
                MobilizationRequestId = id,
                ApprovedBy = User.Identity!.Name!,
                ApprovalDate = DateTime.Now,
                IsApproved = true
            });

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
